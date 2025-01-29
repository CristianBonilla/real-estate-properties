namespace RealEstateProperties.Domain.Entities
{
  public class PropertyEntity
  {
    public Guid PropertyId { get; set; }
    public Guid OwnerId { get; set; }
    public required string Name { get; set; }
    public required string Address { get; set; }
    public required decimal Price { get; set; }
    public Guid CodeInternal { get; set; }
    public int Year { get; set; }
    public DateTimeOffset Created { get; set; }
    public byte[] Version { get; set; } = null!;
    public OwnerEntity Owner { get; set; } = null!;
    public ICollection<PropertyImageEntity> Images { get; set; } = [];
    public ICollection<PropertyTraceEntity> Traces { get; set; } = [];
  }
}
