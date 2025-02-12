using System.Net;
using Asp.Versioning;
using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RealEstateProperties.API.Filters;
using RealEstateProperties.Contracts.DTO.Owner;
using RealEstateProperties.Contracts.Exceptions;
using RealEstateProperties.Contracts.Services;
using RealEstateProperties.Domain.Entities;

namespace RealEstateProperties.API.Controllers
{
  [Route("api/v{version:apiVersion}/[controller]")]
  [ApiController]
  [ApiVersion("1.0")]
  [Produces("application/json")]
  [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
  [ServiceErrorExceptionFilter]
  public class OwnerController(IMapper mapper, IOwnerService ownerService) : ControllerBase
  {
    readonly IMapper _mapper = mapper;
    readonly IOwnerService _ownerService = ownerService;

    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IAsyncEnumerable<OwnerResponse>))]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async IAsyncEnumerable<OwnerResponse> GetOwners()
    {
      var owners = _ownerService.GetOwners();
      await foreach (OwnerEntity owner in owners)
        yield return _mapper.Map<OwnerResponse>(owner);
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(OwnerResponse))]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> AddOwner([FromBody] OwnerRequest ownerRequest)
    {
      OwnerEntity owner = _mapper.Map<OwnerEntity>(ownerRequest);
      OwnerEntity addedOwner = await _ownerService.AddOwner(owner);
      OwnerResponse ownerResponse = _mapper.Map<OwnerResponse>(addedOwner);

      return CreatedAtAction(nameof(AddOwner), ownerResponse);
    }

    [HttpGet("{ownerId}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(OwnerResponse))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> FindOwnerById(Guid ownerId)
    {
      OwnerEntity owner = await _ownerService.FindOwnerById(ownerId)
        ?? throw new ServiceErrorException(HttpStatusCode.NotFound, $"Owner not found with owner identifier \"{ownerId}\"");

      return Ok(_mapper.Map<OwnerResponse>(owner));
    }

    [HttpPut("photo/{ownerId}")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> AddOrUpdateOwnerPhoto(Guid ownerId, IFormFile photo)
    {
      if (photo.Length <= 0)
        throw new ServiceErrorException(HttpStatusCode.BadRequest, "There is no owner photo to process");
      using MemoryStream memoryStream = new();
      await photo.CopyToAsync(memoryStream);
      byte[] photoBytes = memoryStream.ToArray();
      _ = await _ownerService.AddOrUpdateOwnerPhoto(ownerId, photoBytes, photo.FileName);

      return File(photoBytes, "application/octet-stream", photo.FileName);
    }
  }
}
