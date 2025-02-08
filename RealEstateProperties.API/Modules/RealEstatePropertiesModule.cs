using Autofac;
using RealEstateProperties.Contracts.Services;
using RealEstateProperties.Domain.Services;
using RealEstateProperties.Infrastructure.Repositories.RealEstateProperties;
using RealEstateProperties.Infrastructure.Repositories.RealEstateProperties.Interfaces;

namespace RealEstateProperties.API.Modules
{
  class RealEstatePropertiesModule : Module
  {
    protected override void Load(ContainerBuilder builder)
    {
      builder.RegisterType<OwnerRepository>()
        .As<IOwnerRepository>()
        .InstancePerLifetimeScope();
      builder.RegisterType<PropertyRepository>()
        .As<IPropertyRepository>()
        .InstancePerLifetimeScope();
      builder.RegisterType<PropertyImageRepository>()
        .As<IPropertyImageRepository>()
        .InstancePerLifetimeScope();
      builder.RegisterType<PropertyTraceRepository>()
        .As<IPropertyTraceRepository>()
        .InstancePerLifetimeScope();

      builder.RegisterType<AuthService>()
        .As<IAuthService>()
        .InstancePerLifetimeScope();
      builder.RegisterType<OwnerService>()
        .As<IOwnerService>()
        .InstancePerLifetimeScope();
      builder.RegisterType<PropertiesService>()
        .As<IPropertiesService>()
        .InstancePerLifetimeScope();
    }
  }
}
