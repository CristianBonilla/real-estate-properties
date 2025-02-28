using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using RealEstateProperties.API.Options;
using RealEstateProperties.Domain.Helpers;

namespace RealEstateProperties.API.Installers
{
  class JwtInstaller : IInstaller
  {
    public void InstallServices(IServiceCollection services, IConfiguration configuration, IWebHostEnvironment env)
    {
      IConfigurationSection jwtSection = configuration.GetSection(nameof(JwtOptions));
      services.Configure<JwtOptions>(jwtSection);
      JwtOptions jwtOptions = jwtSection.Get<JwtOptions>()!;
      services.AddSingleton(jwtOptions);
      byte[] secretKey = JwtSigningKeyHelper.GetSecretKey(jwtOptions.Secret);
      services.AddAuthentication(auth =>
      {
        auth.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        auth.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
        auth.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
      })
      .AddJwtBearer(jwt =>
      {
        jwt.RequireHttpsMetadata = !env.IsDevelopment(); // Development only
        jwt.SaveToken = true;
        jwt.TokenValidationParameters = new()
        {
          ValidateIssuerSigningKey = true,
          IssuerSigningKey = new SymmetricSecurityKey(secretKey),
          ValidateIssuer = false,
          ValidateAudience = false,
          RequireExpirationTime = false,
          ValidateLifetime = true
        };
      });
      services.AddAuthorization();
    }
  }
}
