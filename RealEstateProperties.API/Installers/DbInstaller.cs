using Microsoft.EntityFrameworkCore;
using RealEstateProperties.Domain.Helpers;
using RealEstateProperties.Infrastructure.Contexts.RealEstateProperties;

namespace RealEstateProperties.API.Installers
{
  class DbInstaller : IInstaller
  {
    public void InstallServices(IServiceCollection services, IConfiguration configuration, IWebHostEnvironment env)
    {
      string? connectionString = configuration.GetConnectionString(ApiConfigKeys.RealEstatePropertiesConnection) ?? throw new InvalidOperationException($"Connection string '{ApiConfigKeys.RealEstatePropertiesConnection}' not established");
      services.AddDbContextPool<RealEstatePropertiesContext>(options => options.UseSqlServer(connectionString));
    }
  }
}
