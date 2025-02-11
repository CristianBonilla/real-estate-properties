using Microsoft.OpenApi.Models;

namespace RealEstateProperties.API.Options
{
  class SwaggerOptions
  {
    public required string JsonRoute { get; set; }
    public required string UIEndpoint { get; set; }
    public required string Title { get; set; }
    public string? Description { get; set; }
    public OpenApiContact? Contact { get; set; }
    public OpenApiSecurityScheme? SecurityScheme { get; set; }
    public required string Version { get; set; }
  }
}
