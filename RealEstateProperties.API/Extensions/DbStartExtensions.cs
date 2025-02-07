using System.Data.Common;
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
        if (delay == 60)
          throw new NotImplementedException("1 minute passed, failed to connect to DB");
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
              _ => throw new ArgumentOutOfRangeException(nameof(start), $"Not expected direction value: {start}")
            });
            delay = 0;
          }
        }
        catch (Exception exception) when (exception is InvalidOperationException || exception is DbException)
        {
          await Task.Delay(TimeSpan.FromSeconds(1));
          Console.WriteLine($"{++delay} seconds have passed, retrying DB connection...");
          await Connect(start);
        }
      }
    }
  }
}
