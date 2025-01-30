namespace RealEstateProperties.Domain.Helpers
{
  public record struct ApiConfigKeys
  {
    public const string AllowOrigins = nameof(AllowOrigins);
    public const string Bearer = nameof(Bearer);
    public const string RealEstatePropertiesConnection = nameof(RealEstatePropertiesConnection);
  }
}
