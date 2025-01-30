namespace RealEstateProperties.API.Options
{
  class JwtOptions
  {
    public required string Secret { get; set; }
    public required int ExpiresInDays { get; set; }
  }
}
