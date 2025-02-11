using RealEstateProperties.Contracts.DTO.Owner;

namespace RealEstateProperties.Contracts.DTO.Properties
{
  public class PropertiesResult
  {
    public required OwnerResponse Owner { get; set; }
    public required IEnumerable<PropertyResponse?> Properties { get; set; }
  }
}
