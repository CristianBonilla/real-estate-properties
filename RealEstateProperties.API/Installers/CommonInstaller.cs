using Microsoft.AspNetCore.Mvc;
using Asp.Versioning;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;
using RealEstateProperties.Domain.Helpers;
using RealEstateProperties.API.Filters;

namespace RealEstateProperties.API.Installers
{
  class CommonInstaller : IInstaller
  {
    public void InstallServices(IServiceCollection services, IConfiguration configuration, IWebHostEnvironment env)
    {
      services.AddMvc(options => options.Filters.Add<ServiceErrorExceptionFilterAttribute>());
      services.AddControllers()
        .AddNewtonsoftJson(JsonSerializer);
      // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
      // services.AddEndpointsApiExplorer();
      services.AddApiVersioning(options =>
      {
        options.DefaultApiVersion = new ApiVersion(1, 0);
        options.AssumeDefaultVersionWhenUnspecified = true;
        options.ReportApiVersions = true;
      }).AddApiExplorer(options =>
      {
        options.GroupNameFormat = "'v'VVV";
        options.SubstituteApiVersionInUrl = true;
      });
      services.AddRouting(options => options.LowercaseUrls = true);
      // Another alternative - services.Configure<RouteOptions>(options => options.LowercaseUrls = true);
      services.AddCors(options =>
      {
        options.AddPolicy(ApiConfigKeys.AllowOrigins, builder =>
        {
          builder.AllowAnyOrigin()
            .AllowAnyHeader()
            .WithMethods("GET", "POST", "PUT", "DELETE");
        });
      });
    }

    private void JsonSerializer(MvcNewtonsoftJsonOptions options)
    {
      JsonSerializerSettings settings = options.SerializerSettings;
      settings.Converters.Add(new StringEnumConverter());
      settings.ContractResolver = new CamelCasePropertyNamesContractResolver();
      settings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
      settings.Formatting = Formatting.None;
    }
  }
}
