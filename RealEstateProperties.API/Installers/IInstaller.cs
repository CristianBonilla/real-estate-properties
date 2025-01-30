namespace RealEstateProperties.API.Installers
{
  interface IInstaller
  {
    void InstallServices(IServiceCollection services, IConfiguration configuration, IWebHostEnvironment env);
  }
}
