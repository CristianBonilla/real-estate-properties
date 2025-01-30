using Autofac.Extensions.DependencyInjection;
using RealEstateProperties.API.Extensions;
using RealEstateProperties.Infrastructure.Contexts.RealEstateProperties;

namespace RealEstateProperties.API
{
  public class Program
  {
    public static async Task Main(string[] args)
    {
      IHost host = CreateHostBuilder(args).Build();
      await host.DbStart<RealEstatePropertiesContext>().Migrate();
      await host.RunAsync();
    }

    public static IHostBuilder CreateHostBuilder(string[] args) =>
      Host.CreateDefaultBuilder(args)
        .UseServiceProviderFactory(new AutofacServiceProviderFactory())
        .ConfigureWebHostDefaults(builder =>
        {
          builder.UseStartup<Startup>();
        });
  }
}
