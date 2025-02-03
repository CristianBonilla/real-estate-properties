using RealEstateProperties.Domain.Entities.Auth;

namespace RealEstateProperties.Contracts.Services
{
  public interface IAuthService
  {
    Task<UserEntity> AddUser(UserEntity user);
    Task<UserEntity?> FindUserById(Guid userId);
    Task<UserEntity?> FindUserByUsername(string username);
    Task<UserEntity?> FindUserByEmail(string email);
    IAsyncEnumerable<UserEntity> GetUsers();
  }
}
