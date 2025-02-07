using RealEstateProperties.Domain.Entities;

namespace RealEstateProperties.Contracts.DTO.Properties
{
  public class PropertyImageRequest : PropertyImage
  {
    public required Guid PropertyId { get; set; }
    public required bool Enabled { get; set; }
  }
}
