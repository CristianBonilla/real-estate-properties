using Autofac;
using RealEstateProperties.Infrastructure.Repositories.Auth.Interfaces;
using RealEstateProperties.Infrastructure.Repositories.Auth;
using RealEstateProperties.Contracts.Identity;
using RealEstateProperties.API.Identity;
using RealEstateProperties.Contracts.Services;
using RealEstateProperties.Domain.Services;

namespace RealEstateProperties.API.Modules
{
  class AuthModule : Module
  {
    protected override void Load(ContainerBuilder builder)
    {
      builder.RegisterType<UserRepository>()
        .As<IUserRepository>()
        .InstancePerLifetimeScope();
      builder.RegisterType<AuthService>()
        .As<IAuthService>()
        .InstancePerLifetimeScope();
      builder.RegisterType<AuthIdentity>()
        .As<IAuthIdentity>()
        .InstancePerLifetimeScope();
    }
  }
}
