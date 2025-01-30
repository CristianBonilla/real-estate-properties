using Autofac;
using RealEstateProperties.Contracts.SeedData;
using RealEstateProperties.Domain.SeedWork;

namespace RealEstateProperties.API.Modules
{
  class DbModule : Module
  {
    protected override void Load(ContainerBuilder builder)
    {
      builder.RegisterType<SeedData>()
        .As<ISeedData>()
        .InstancePerLifetimeScope();
    }
  }
}
