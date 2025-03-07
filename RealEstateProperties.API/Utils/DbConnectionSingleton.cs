using System.Data.Common;
using System.Net.Sockets;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using RealEstateProperties.Contracts.Enums;

namespace RealEstateProperties.API.Utils;

class DbConnectionSingleton
{
  static Lazy<DbConnectionSingleton>? _instance;
  readonly IHost _host;
  int _delay;
  
  private DbConnectionSingleton(IHost host) => _host = host;

  public static DbConnectionSingleton Start(IHost host)
  {
    _instance ??= new(() => new(host));

    return _instance.Value;
  }

  public async Task Connect<TContext>(DbStart start) where TContext : DbContext
  {
    AsyncServiceScope scope = _host.Services.CreateAsyncScope();
    TContext context = scope.ServiceProvider.GetRequiredService<TContext>();
    DatabaseFacade database = context.Database;
    try
    {
      await using (scope.ConfigureAwait(false))
      {
        await (start switch
        {
          DbStart.OpenConnection => database.OpenConnectionAsync(),
          DbStart.EnsureCreated => database.EnsureCreatedAsync(),
          DbStart.Migration => database.MigrateAsync(),
          _ => throw new ArgumentOutOfRangeException(nameof(start), $"Not expected DB start value: {start}")
        });
        _delay = 0;
        Console.WriteLine($"{typeof(TContext).Name} DB connection started successfully.");
      }
    }
    catch (InvalidOperationException)
    {
      Console.WriteLine("Unhandled exception while DB connection.");

      throw;
    }
    catch (DbException exception) when (exception.InnerException is SocketException socketException && socketException.SocketErrorCode == SocketError.HostNotFound)
    {
      Console.WriteLine("Unidentified or nonexistent DB connection.");
      Console.WriteLine(exception.Message);

      throw socketException;
    }
    catch (DbException exception) when (exception.InnerException is null)
    {
      await Task.Delay(TimeSpan.FromSeconds(1));
      Console.WriteLine($"{++_delay} seconds have passed, retrying DB connection...");
      await Connect<TContext>(start);
    }
  }
}
