using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using AutoMapper;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
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
  class AuthIdentity(
    IMapper mapper,
    IAuthService authService,
    JwtOptions jwtOptions) : IAuthIdentity
  {
    readonly IMapper _mapper = mapper;
    readonly IAuthService _authService = authService;
    readonly JwtOptions _jwtOptions = jwtOptions;

    public async Task<AuthResult> Login(UserLoginRequest userLoginRequest)
    {
      UserEntity user = await _authService.FindUserByUsernameOrEmail(userLoginRequest.UsernameOrEmail);
      bool userValidPassoword = HashPasswordHelper.Verify(userLoginRequest.Password, user.Password, user.Salt);
      if (!userValidPassoword)
        throw new ServiceErrorException(HttpStatusCode.Unauthorized, $"User password is invalid \"{userLoginRequest.Password}\"");

      return GenerateAuthForUser(user);
    }

    public async Task<AuthResult> Register(UserRegisterRequest userRegisterRequest)
    {
      bool existingUser = await UserExists(userRegisterRequest);
      if (existingUser)
        throw new ServiceErrorException(HttpStatusCode.Unauthorized, $"User with provided document Number or username already exists");
      UserEntity user = _mapper.Map<UserEntity>(userRegisterRequest);
      UserEntity addedUser = await _authService.AddUser(user);

      return GenerateAuthForUser(addedUser);
    }

    public async Task<bool> UserExists(UserRegisterRequest userRegisterRequest) => await _authService.UserExists(userRegisterRequest.DocumentNumber, userRegisterRequest.Username);

    private AuthResult GenerateAuthForUser(UserEntity user)
    {
      JwtSecurityTokenHandler tokenHandler = new();
      string secret = Convert.ToHexStringLower(Encoding.UTF8.GetBytes(_jwtOptions.Secret));
      byte[] key = Encoding.UTF8.GetBytes(secret);
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
        Expires = DateTime.UtcNow.AddDays(_jwtOptions.ExpiresInDays),
        SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha512Signature)
      };
      SecurityToken token = tokenHandler.CreateToken(tokenDescriptor);

      return new(tokenHandler.WriteToken(token), _mapper.Map<UserResponse>(user));
    }

    private static string UserToJson(UserEntity user)
    {
      string userJson = JsonConvert.SerializeObject(user, Formatting.Indented, new JsonSerializerSettings
      {
        ContractResolver = new CamelCasePropertyNamesContractResolver(),
        DefaultValueHandling = DefaultValueHandling.Ignore,
        NullValueHandling = NullValueHandling.Ignore
      });

      return userJson;
    }
  }
}
