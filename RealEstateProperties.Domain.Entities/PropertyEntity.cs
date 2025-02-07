namespace RealEstateProperties.Domain.Entities
{
  public class PropertyEntity
  {
    public Guid PropertyId { get; set; }
    public required Guid OwnerId { get; set; }
    public required string Name { get; set; }
    public required string Address { get; set; }
    public required decimal Price { get; set; }
    public int CodeInternal { get; set; }
    public required int Year { get; set; }
    public DateTimeOffset Created { get; set; }
    public byte[] Version { get; set; } = null!;
    public OwnerEntity Owner { get; set; } = null!;
    public ICollection<PropertyImageEntity> Images { get; set; } = [];
    public ICollection<PropertyTraceEntity> Traces { get; set; } = [];
  }
}
