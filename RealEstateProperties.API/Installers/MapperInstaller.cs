using RealEstateProperties.API.Mappers;

namespace RealEstateProperties.API.Installers
{
  class MapperInstaller : IInstaller
  {
    public void InstallServices(IServiceCollection services, IConfiguration configuration, IWebHostEnvironment env)
    {
      services.AddAutoMapper(
        typeof(AuthProfile),
        typeof(RealEstatePropertiesProfile));
    }
  }
}
