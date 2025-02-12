using RealEstateProperties.Contracts.Services;
using RealEstateProperties.Domain.Entities.Auth;
using RealEstateProperties.Domain.Helpers;
using RealEstateProperties.Infrastructure.Repositories.Auth.Interfaces;
using RealEstateProperties.Infrastructure.Repositories.RealEstateProperties.Interfaces;

namespace RealEstateProperties.Domain.Services
{
  public class AuthService(IRealEstatePropertiesRepositoryContext context, IUserRepository userRepository) : IAuthService
  {
    readonly IRealEstatePropertiesRepositoryContext _context = context;
    readonly IUserRepository _userRepository = userRepository;

    public async Task<UserEntity> AddUser(UserEntity user)
    {
      var (password, salt) = HashPasswordHelper.Create(user.Password);
      user.Password = password;
      user.Salt = salt;
      user.IsActive = true;
      UserEntity addUser = _userRepository.Create(user);
      _ = await _context.SaveAsync();

      return addUser;
    }

    public Task<UserEntity?> FindUserById(Guid userId) => Task.FromResult(_userRepository.Find([userId]));

    public async Task<UserEntity?> FindUserByUsername(string username) => await GetUsers().FirstOrDefaultAsync(user => StringCommonHelper.IsStringEquivalent(user.Username, username));

    public async Task<UserEntity?> FindUserByEmail(string email) => await GetUsers().FirstOrDefaultAsync(user => StringCommonHelper.IsStringEquivalent(user.Email, email));

    public async Task<UserEntity?> FindUserByUsernameOrEmail(string usernameOrEmail)
      => await GetUsers().FirstOrDefaultAsync(user => StringCommonHelper.IsStringEquivalent(user.Username, usernameOrEmail) || StringCommonHelper.IsStringEquivalent(user.Email, usernameOrEmail));

    public async Task<bool> UserExists(string documentNumber, string username)
      => await GetUsers().AnyAsync(user => StringCommonHelper.IsStringEquivalent(user.DocumentNumber, documentNumber) || StringCommonHelper.IsStringEquivalent(user.Username, username));

    public IAsyncEnumerable<UserEntity> GetUsers()
    {
      var users = _userRepository.GetAll(users => users.OrderBy(order => order.Firstname)
        .ThenBy(order => order.Username))
        .ToAsyncEnumerable();

      return users;
    }
  }
}
