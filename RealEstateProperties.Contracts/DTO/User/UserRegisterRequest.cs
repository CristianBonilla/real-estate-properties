namespace RealEstateProperties.Contracts.DTO.User
{
  public class UserRegisterRequest
  {
    public required string DocumentNumber { get; set; }
    public required string Mobile { get; set; }
    public required string Username { get; set; }
    public required string Password { get; set; }
    public required string Email { get; set; }
    public required string Firstname { get; set; }
    public required string Lastname { get; set; }
    public required bool IsActive { get; set; }
  }
}
