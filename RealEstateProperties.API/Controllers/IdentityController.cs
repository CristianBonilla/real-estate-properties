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
  }
}
