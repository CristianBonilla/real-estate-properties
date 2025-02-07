namespace RealEstateProperties.Contracts.DTO.User
{
  public class UserResponse
  {
    public Guid UserId { get; set; }
    public required string DocumentNumber { get; set; }
    public required string Mobile { get; set; }
    public required string Username { get; set; }
    public required string Email { get; set; }
    public required string Firstname { get; set; }
    public required string Lastname { get; set; }
    public bool IsActive { get; set; }
    public DateTimeOffset Created { get; set; }
  }
}
