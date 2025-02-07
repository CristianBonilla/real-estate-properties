using Autofac;
using RealEstateProperties.Infrastructure.Repositories.Auth.Interfaces;
using RealEstateProperties.Infrastructure.Repositories.Auth;
using RealEstateProperties.Contracts.Identity;
using RealEstateProperties.API.Identity;

namespace RealEstateProperties.API.Modules
{
  class AuthModule : Module
  {
    protected override void Load(ContainerBuilder builder)
    {
      builder.RegisterType<UserRepository>()
        .As<IUserRepository>()
        .InstancePerLifetimeScope();
      builder.RegisterType<AuthIdentity>()
        .As<IAuthIdentity>()
        .InstancePerLifetimeScope();
    }
  }
}
