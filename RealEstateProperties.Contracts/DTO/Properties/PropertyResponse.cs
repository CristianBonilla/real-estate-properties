namespace RealEstateProperties.Contracts.DTO.Properties
{
  public class PropertyResponse
  {
    public Guid PropertyId { get; set; }
    public required Guid OwnerId { get; set; }
    public required string Name { get; set; }
    public required string Address { get; set; }
    public required decimal Price { get; set; }
    public int CodeInternal { get; set; }
    public required int Year { get; set; }
    public DateTimeOffset Created { get; set; }
    public required IAsyncEnumerable<PropertyTraceResponse?> PropertyTraces { get; set; }
  }
}
