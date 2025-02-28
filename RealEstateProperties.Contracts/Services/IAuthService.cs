using RealEstateProperties.Domain.Entities.Auth;

namespace RealEstateProperties.Contracts.Services
{
  public interface IAuthService
  {
    Task<UserEntity> AddUser(UserEntity user);
    Task<bool> UserExists(string documentNumber, string email);
    IAsyncEnumerable<UserEntity> GetUsers();
    Task<UserEntity> FindUserById(Guid userId);
    Task<UserEntity> FindUserByUsernameOrEmail(string usernameOrEmail);
  }
}
