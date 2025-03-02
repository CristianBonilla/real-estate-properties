namespace RealEstateProperties.Contracts.DTO.Properties
{
  public class PropertyImageResponse
  {
    public Guid PropertyImageId { get; set; }
    public required Guid PropertyId { get; set; }
    public required bool Enabled { get; set; }
    public required string ImageName { get; set; }
    public DateTimeOffset Created { get; set; }
  }
}
