using RealEstateProperties.Contracts.DTO.User;

namespace RealEstateProperties.Contracts.DTO.Auth
{
  public record AuthResult(string Token, UserResponse User);
}
