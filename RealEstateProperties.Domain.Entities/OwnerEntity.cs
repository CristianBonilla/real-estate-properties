namespace RealEstateProperties.Domain.Entities
{
  public class OwnerEntity : OwnerPhoto
  {
    public Guid OwnerId { get; set; }
    public required string Name { get; set; }
    public required string Address { get; set; }
    public required DateTimeOffset Birthday { get; set; }
    public DateTimeOffset Created { get; set; }
    public byte[] Version { get; set; } = null!;
    public ICollection<PropertyEntity> Properties { get; set; } = [];
  }

  public abstract class OwnerPhoto
  {
    public byte[]? Photo { get; set; } = null;
    public string? PhotoName { get; set; } = null;
  }
}
