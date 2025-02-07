namespace RealEstateProperties.Contracts.DTO.Owner
{
  public class OwnerRequest
  {
    public required string Name { get; set; }
    public required string Address { get; set; }
    public required DateTimeOffset Birthday { get; set; }
  }
}
