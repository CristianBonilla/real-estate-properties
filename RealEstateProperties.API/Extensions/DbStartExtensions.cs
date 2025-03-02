using System.Data.Common;
using System.Net.Sockets;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace RealEstateProperties.API.Extensions
{
  static class DbStartExtensions
  {
    enum DbStartType
    {
      OpenConnection = 1,
      EnsureCreated = 2,
      Migrate = 3
    }

    public static (Func<Task> OpenConnection, Func<Task> EnsureCreated, Func<Task> Migrate) DbStart<TContext>(this IHost host) where TContext : DbContext
    {
      int delay = 0;

      return (() => Connect(DbStartType.OpenConnection), () => Connect(DbStartType.EnsureCreated), () => Connect(DbStartType.Migrate));

      async Task Connect(DbStartType start)
      {
        AsyncServiceScope scope = host.Services.CreateAsyncScope();
        TContext context = scope.ServiceProvider.GetRequiredService<TContext>();
        DatabaseFacade database = context.Database;
        try
        {
          await using (scope.ConfigureAwait(false))
          {
            await (start switch
            {
              DbStartType.OpenConnection => database.OpenConnectionAsync(),
              DbStartType.EnsureCreated => database.EnsureCreatedAsync(),
              DbStartType.Migrate => database.MigrateAsync(),
              _ => throw new ArgumentOutOfRangeException(nameof(start), $"Not expected DB start type: {start}")
            });
            delay = 0;
            Console.WriteLine($"{typeof(TContext).Name} DB connection started successfully.");
          }
        }
        catch (InvalidOperationException)
        {
          Console.WriteLine("Unhandled exception while DB start connection.");

          throw;
        }
        catch (DbException exception) when (exception.InnerException is SocketException socketException && socketException.SocketErrorCode == SocketError.HostNotFound)
        {
          Console.WriteLine("Unidentified or nonexistent DB start connection.");
          Console.WriteLine(exception.Message);

          throw socketException;
        }
        catch (DbException exception) when (exception.InnerException is null)
        {
          await Task.Delay(TimeSpan.FromSeconds(1));
          Console.WriteLine($"{++delay} seconds have passed, retrying DB start connection...");
          await Connect(start);
        }
      }
    }
  }
}
