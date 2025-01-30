using Autofac;
using RealEstateProperties.Contracts.Repository;
using RealEstateProperties.Infrastructure.Repositories;
using RealEstateProperties.Infrastructure.Repositories.RealEstateProperties;
using RealEstateProperties.Infrastructure.Repositories.RealEstateProperties.Interfaces;

namespace RealEstateProperties.API.Modules
{
  class RepositoriesModule : Module
  {
    protected override void Load(ContainerBuilder builder)
    {
      builder.RegisterGeneric(typeof(RepositoryContext<>))
        .As(typeof(IRepositoryContext<>))
        .InstancePerLifetimeScope();
      builder.RegisterGeneric(typeof(Repository<,>))
        .As(typeof(IRepository<,>))
        .InstancePerLifetimeScope();
      builder.RegisterType<RealEstatePropertiesRepositoryContext>()
        .As<IRealEstatePropertiesRepositoryContext>()
        .InstancePerLifetimeScope();
    }
  }
}
