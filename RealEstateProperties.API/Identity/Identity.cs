using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Security.Claims;
using System.Text;
using AutoMapper;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using RealEstateProperties.API.Options;
using RealEstateProperties.Contracts.DTO.Auth;
using RealEstateProperties.Contracts.DTO.User;
using RealEstateProperties.Contracts.Exceptions;
using RealEstateProperties.Contracts.Identity;
using RealEstateProperties.Contracts.Services;
using RealEstateProperties.Domain.Entities.Auth;
using RealEstateProperties.Domain.Helpers;

namespace RealEstateProperties.API.Identity
{
  class Identity(
    IMapper mapper,
    IAuthService authService,
    JwtOptions jwtOptions) : IIdentity
  {
    readonly IMapper _mapper = mapper;
    readonly IAuthService _authService = authService;
    readonly JwtOptions _jwtOptions = jwtOptions;

    public async Task<AuthResult> Login(UserLoginRequest userLoginRequest)
    {
      UserEntity user = await _authService.FindUserByUsername(userLoginRequest.UsernameOrEmail)
        ?? await _authService.FindUserByEmail(userLoginRequest.UsernameOrEmail)
        ?? throw new ServiceErrorException(HttpStatusCode.Unauthorized, $"User is not registered or is incorrect \"{userLoginRequest.UsernameOrEmail}\"");
      bool userValidPassoword = HashPasswordHelper.Verify(userLoginRequest.Password, user.Password, user.Salt);
      if (!userValidPassoword)
        throw new ServiceErrorException(HttpStatusCode.Unauthorized, $"User password is invalid \"{userLoginRequest.Password}\"");

      return GenerateAuthForUser(user);
    }

    public async Task<AuthResult> Register(UserRegisterRequest userRequest)
    {
      bool existingUser = await UserExists(userRequest);
      if (!existingUser)
        throw new ServiceErrorException(HttpStatusCode.Unauthorized, $"User with provided email or username already exists");
      UserEntity user = _mapper.Map<UserEntity>(userRequest);
      UserEntity addedUser = await _authService.AddUser(user);

      return GenerateAuthForUser(addedUser);
    }

    public async Task<bool> UserExists(UserRegisterRequest userRequest)
    {
      bool existingUser = await _authService.UserExists(user => user.Username == userRequest.Username || user.Email == userRequest.Email);

      return existingUser;
    }

    private AuthResult GenerateAuthForUser(UserEntity user)
    {
      JwtSecurityTokenHandler tokenHandler = new();
      byte[] key = Encoding.ASCII.GetBytes(_jwtOptions.Secret);
      SecurityTokenDescriptor tokenDescriptor = new()
      {
        Subject = new([
          new(JwtRegisteredClaimNames.Sub, user.Email),
          new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
          new(JwtRegisteredClaimNames.Email, user.Email),
          new(JwtRegisteredClaimNames.NameId, user.UserId.ToString()),
          new(ClaimTypes.NameIdentifier, user.Username),
          new(ClaimTypes.UserData, UserToJson(user))
        ]),
        Expires = DateTime.UtcNow.AddDays(7),
        SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha512Signature)
      };
      SecurityToken token = tokenHandler.CreateToken(tokenDescriptor);

      return new(tokenHandler.WriteToken(token), _mapper.Map<UserResponse>(user));
    }

    private static string UserToJson(UserEntity user) => JsonConvert.SerializeObject(user, Formatting.Indented);
  }
}
