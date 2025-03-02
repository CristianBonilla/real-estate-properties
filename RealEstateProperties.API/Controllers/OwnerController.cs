using Asp.Versioning;
using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RealEstateProperties.API.Filters;
using RealEstateProperties.API.Utils;
using RealEstateProperties.Contracts.DTO.Owner;
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

    [HttpDelete("{ownerId}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(OwnerResponse))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> DeleteOwner(Guid ownerId)
    {
      OwnerEntity owner = await _ownerService.DeleteOwner(ownerId);

      return Ok(_mapper.Map<OwnerResponse>(owner));
    }

    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IAsyncEnumerable<OwnerResponse>))]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async IAsyncEnumerable<OwnerResponse> GetOwners()
    {
      var owners = _ownerService.GetOwners();
      await foreach (OwnerEntity owner in owners)
        yield return _mapper.Map<OwnerResponse>(owner);
    }

    [HttpGet("{ownerId}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(OwnerResponse))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> FindOwnerById(Guid ownerId)
    {
      OwnerEntity owner = await _ownerService.FindOwnerById(ownerId);

      return Ok(_mapper.Map<OwnerResponse>(owner));
    }

    [HttpPut("photo/{ownerId}")]
    [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(OwnerResponse))]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> AddOrUpdateOwnerPhoto(Guid ownerId, IFormFile photo)
    {
      if (photo.Length <= 0)
        return StatusCode(StatusCodes.Status400BadRequest, "There is no owner photo to process");
      byte[] photoBytes = await ImageStreamUtils.GetImageBytes(photo);
      OwnerEntity owner = await _ownerService.AddOrUpdateOwnerPhoto(ownerId, photoBytes, photo.FileName);
      OwnerResponse ownerResponse = _mapper.Map<OwnerResponse>(owner);

      return CreatedAtAction(nameof(AddOrUpdateOwnerPhoto), ownerResponse);
    }

    [HttpGet("photo/{ownerId}/file")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(FileContentResult))]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetOwnerPhotoFile(Guid ownerId)
    {
      OwnerEntity owner = await _ownerService.FindOwnerById(ownerId);
      if (owner.Photo is null)
        return StatusCode(StatusCodes.Status400BadRequest, "There is no owner photo to process");

      return File(owner.Photo, "application/octect-stream", owner.PhotoName);
    }
  }
}
