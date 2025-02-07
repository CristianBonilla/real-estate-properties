using RealEstateProperties.Contracts.DTO.Auth;
using RealEstateProperties.Contracts.DTO.User;

namespace RealEstateProperties.Contracts.Identity
{
  public interface IIdentity
  {
    Task<AuthResult> Login(UserLoginRequest userLoginRequest);
    Task<AuthResult> Register(UserRegisterRequest user);
    Task<bool> UserExists(UserRegisterRequest user);
  }
}
