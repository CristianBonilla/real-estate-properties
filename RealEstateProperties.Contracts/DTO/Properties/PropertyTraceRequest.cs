namespace RealEstateProperties.Contracts.DTO.Properties
{
  public class PropertyTraceRequest
  {
    public required Guid PropertyId { get; set; }
    public required string Name { get; set; }
    public required decimal Value { get; set; }
    public required decimal Tax { get; set; }
    public required DateTimeOffset DateSale { get; set; }
  }
}
