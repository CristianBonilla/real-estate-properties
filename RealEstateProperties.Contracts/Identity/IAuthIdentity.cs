using RealEstateProperties.Contracts.DTO.Auth;
using RealEstateProperties.Contracts.DTO.User;

namespace RealEstateProperties.Contracts.Identity
{
  public interface IAuthIdentity
  {
    Task<AuthResult> Register(UserRegisterRequest userRegisterRequest);
    Task<AuthResult> Login(UserLoginRequest userLoginRequest);
    Task<bool> UserExists(UserRegisterRequest user);
  }
}
