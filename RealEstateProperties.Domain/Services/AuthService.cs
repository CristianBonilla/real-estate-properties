using RealEstateProperties.Contracts.Services;
using RealEstateProperties.Domain.Entities.Auth;
using RealEstateProperties.Domain.Helpers;
using RealEstateProperties.Infrastructure.Repositories.Auth.Interfaces;
using RealEstateProperties.Infrastructure.Repositories.RealEstateProperties.Interfaces;
using System.Linq.Expressions;

namespace RealEstateProperties.Domain.Services
{
  public class AuthService(IRealEstatePropertiesRepositoryContext context, IUserRepository userRepository) : IAuthService
  {
    readonly IRealEstatePropertiesRepositoryContext _context = context;
    readonly IUserRepository _userRepository = userRepository;

    public async Task<UserEntity> AddUser(UserEntity user)
    {
      user = _userRepository.Create(user);
      _ = await _context.SaveAsync();

      return user;
    }

    public Task<UserEntity?> FindUserById(Guid userId) => Task.FromResult(_userRepository.Find([userId]));

    public Task<UserEntity?> FindUserByUsername(string username) => Task.FromResult(_userRepository.Find(user => StringCommonHelper.IsStringEquivalent(user.Username, username)));

    public Task<UserEntity?> FindUserByEmail(string email) => Task.FromResult(_userRepository.Find(user => StringCommonHelper.IsStringEquivalent(user.Email, email)));

    public Task<bool> UserExists(Expression<Func<UserEntity, bool>> expression) => Task.FromResult(_userRepository.Exists(expression));

    public IAsyncEnumerable<UserEntity> GetUsers()
    {
      var users = _userRepository.GetAll(users => users.OrderBy(order => order.Firstname)
        .ThenBy(order => order.Username))
        .ToAsyncEnumerable();

      return users;
    }
  }
}
