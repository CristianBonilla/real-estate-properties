namespace RealEstateProperties.Contracts.DTO.Properties
{
  public class PropertyTraceResponse
  {
    public Guid PropertyTraceId { get; set; }
    public required Guid PropertyId { get; set; }
    public required string Name { get; set; }
    public required decimal Value { get; set; }
    public required decimal Tax { get; set; }
    public required DateTimeOffset DateSale { get; set; }
    public DateTimeOffset Created { get; set; }
  }
}
