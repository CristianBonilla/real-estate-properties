using Autofac;
using RealEstateProperties.Infrastructure.Repositories.Auth.Interfaces;
using RealEstateProperties.Infrastructure.Repositories.Auth;

namespace RealEstateProperties.API.Modules
{
  class AuthModule : Module
  {
    protected override void Load(ContainerBuilder builder)
    {
      builder.RegisterType<UserRepository>()
        .As<IUserRepository>()
        .InstancePerLifetimeScope();
    }
  }
}
