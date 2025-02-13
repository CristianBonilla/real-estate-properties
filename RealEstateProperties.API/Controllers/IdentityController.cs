using Asp.Versioning;
using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RealEstateProperties.API.Filters;
using RealEstateProperties.Contracts.DTO.Auth;
using RealEstateProperties.Contracts.DTO.User;
using RealEstateProperties.Contracts.Identity;
using RealEstateProperties.Contracts.Services;
using RealEstateProperties.Domain.Entities.Auth;

namespace RealEstateProperties.API.Controllers
{
  [Route("api/v{version:apiVersion}/[controller]")]
  [ApiController]
  [ApiVersion("1.0")]
  [Produces("application/json")]
  [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
  [ServiceErrorExceptionFilter]
  public class IdentityController(IMapper mapper, IAuthService authService, IAuthIdentity authIdentity) : Controller
  {
    readonly IMapper _mapper = mapper;
    readonly IAuthService _authService = authService;
    readonly IAuthIdentity _authIdentity = authIdentity;

    [AllowAnonymous]
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(AuthResult))]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> Register([FromBody] UserRegisterRequest userRegisterRequest)
    {
      AuthResult auth = await _authIdentity.Register(userRegisterRequest);

      return CreatedAtAction(nameof(Register), auth);
    }

    [AllowAnonymous]
    [HttpPost("login")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(AuthResult))]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> Login([FromBody] UserLoginRequest userLoginRequest)
    {
      AuthResult auth = await _authIdentity.Login(userLoginRequest);

      return CreatedAtAction(nameof(Login), auth);
    }

    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IAsyncEnumerable<UserResponse>))]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async IAsyncEnumerable<UserResponse> GetUsers()
    {
      var users = _authService.GetUsers();
      await foreach (UserEntity user in users)
        yield return _mapper.Map<UserResponse>(user);
    }

    [HttpGet("{userId:guid}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(UserResponse))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> FindUserById(Guid userId)
    {
      UserEntity user = await _authService.FindUserById(userId);

      return Ok(_mapper.Map<UserResponse>(user));
    }

    [HttpGet("{usernameOrEmail}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(UserResponse))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> FindUserByUsernameOrEmail(string usernameOrEmail)
    {
      UserEntity user = await _authService.FindUserByUsernameOrEmail(usernameOrEmail);

      return Ok(_mapper.Map<UserResponse>(user));
    }
  } 
}
