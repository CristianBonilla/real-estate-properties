namespace RealEstateProperties.Domain.Entities
{
  public class PropertyImageEntity : PropertyImage
  {
    public Guid PropertyImageId { get; set; }
    public Guid PropertyId { get; set; }
    public bool Enabled { get; set; }
    public DateTimeOffset Created { get; set; }
    public byte[] Version { get; set; } = null!;
    public PropertyEntity Property { get; set; } = null!;
  }

  public abstract class PropertyImage
  {
    public required byte[] Image { get; set; }
    public required string ImageName { get; set; }
  }
}
