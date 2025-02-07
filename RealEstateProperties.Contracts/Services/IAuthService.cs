using System.Linq.Expressions;
using RealEstateProperties.Domain.Entities.Auth;

namespace RealEstateProperties.Contracts.Services
{
  public interface IAuthService
  {
    Task<UserEntity> AddUser(UserEntity user);
    Task<UserEntity?> FindUserById(Guid userId);
    Task<UserEntity?> FindUserByUsername(string username);
    Task<UserEntity?> FindUserByEmail(string email);
    Task<bool> UserExists(Expression<Func<UserEntity, bool>> expression);
    IAsyncEnumerable<UserEntity> GetUsers();
  }
}
