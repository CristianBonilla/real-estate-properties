using System.IO.Compression;
using Asp.Versioning;
using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RealEstateProperties.API.Filters;
using RealEstateProperties.Contracts.DTO.Properties;
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
  public class PropertiesController(IMapper mapper, IPropertiesService propertiesService) : ControllerBase
  {
    readonly IMapper _mapper = mapper;
    readonly IPropertiesService _propertiesService = propertiesService;

    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IAsyncEnumerable<PropertiesResult>))]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public IAsyncEnumerable<PropertiesResult> GetProperties()
      => _mapper.Map<IAsyncEnumerable<PropertiesResult>>(_propertiesService.GetProperties());

    [HttpGet("{text}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IAsyncEnumerable<PropertiesResult>))]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public IAsyncEnumerable<PropertiesResult> GetProperties(string text)
      => _mapper.Map<IAsyncEnumerable<PropertiesResult>>(_propertiesService.GetProperties(text));

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(PropertyResponse))]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> AddProperty([FromBody] PropertyRequest propertyRequest)
    {
      PropertyEntity property = _mapper.Map<PropertyEntity>(propertyRequest);
      PropertyEntity addedProperty = await _propertiesService.AddProperty(property);
      PropertyResponse propertyResponse = _mapper.Map<PropertyResponse>(addedProperty);

      return CreatedAtAction(nameof(AddProperty), propertyResponse);
    }

    [HttpPut("{propertyId}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(PropertyResponse))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> UpdateProperty(Guid propertyId, [FromBody] PropertyRequest propertyRequest)
    {
      PropertyEntity propertyFound = await _propertiesService.FindPropertyById(propertyId);
      PropertyEntity property = _mapper.Map(propertyRequest, propertyFound);
      PropertyResponse propertyResponse = await UpdateProperty(propertyId, property);

      return Ok(propertyResponse);
    }

    [HttpPut("price/{propertyId}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(PropertyResponse))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> ChangePropertyPrice(Guid propertyId, [FromQuery] decimal price)
    {
      PropertyEntity property = await _propertiesService.FindPropertyById(propertyId);
      property.Price = price;
      PropertyResponse propertyResponse = await UpdateProperty(propertyId, property);

      return Ok(propertyResponse);
    }

    [HttpDelete("{propertyId}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(PropertyResponse))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> DeleteProperty(Guid propertyId)
    {
      PropertyEntity deletedProperty = await _propertiesService.DeleteProperty(propertyId);
      PropertyResponse property = _mapper.Map<PropertyResponse>(deletedProperty);

      return Ok(property);
    }

    [HttpGet("images/{propertyId}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(FileContentResult))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetImagesByPropertyId(Guid propertyId)
    {
      var (propertyName, propertyImages) = _propertiesService.GetImagesByPropertyId(propertyId);
      int length = propertyImages.Count();
      if (length == 1)
        return StatusCode(StatusCodes.Status400BadRequest, $"There are no images to process");
      using MemoryStream memoryStream = new();
      if (length == 1)
      {
        PropertyImageEntity propertyImage = propertyImages.First();
        await memoryStream.WriteAsync(propertyImage.Image.AsMemory(0, propertyImage.Image.Length));
        memoryStream.Seek(0, SeekOrigin.Begin);
        byte[] imageBytes = memoryStream.ToArray();

        return File(imageBytes, "application/octet-stream", propertyImage.ImageName);
      }
      using (ZipArchive zip = new(memoryStream, ZipArchiveMode.Create, true))
      {
        foreach (PropertyImageEntity propertyImage in propertyImages)
        {
          ZipArchiveEntry entry = zip.CreateEntry(propertyImage.ImageName, CompressionLevel.Fastest);
          using Stream stream = entry.Open();
          await stream.WriteAsync(propertyImage.Image.AsMemory(0, propertyImage.Image.Length));
        }
      }
      memoryStream.Seek(0, SeekOrigin.Begin);
      byte[] zipBytes = memoryStream.ToArray();
      string date = DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss tt");
      string zipName = $"{propertyName} {date}.zip";

      return File(zipBytes, "application/zip", zipName);
    }

    [HttpPost("images/{propertyId}")]
    [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(FileContentResult))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> AddPropertyImage(Guid propertyId, IFormFile image)
    {
      if (image.Length <= 0)
        return StatusCode(StatusCodes.Status400BadRequest, "There is no property image to process");
      using MemoryStream memoryStream = new();
      await image.CopyToAsync(memoryStream);
      byte[] imageBytes = memoryStream.ToArray();
      _ = await _propertiesService.AddPropertyImage(propertyId, imageBytes, image.FileName);

      return File(imageBytes, "application/octect-stream", image.FileName);
    }

    [HttpPost("traces")]
    [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(PropertyTraceResponse))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> AddPropertyTrace([FromBody] PropertyTraceRequest propertyTraceRequest)
    {
      PropertyTraceEntity propertyTrace = _mapper.Map<PropertyTraceEntity>(propertyTraceRequest);
      PropertyTraceEntity addedPropertyTrace = await _propertiesService.AddPropertyTrace(propertyTrace);
      PropertyTraceResponse propertyTraceResponse = _mapper.Map<PropertyTraceResponse>(addedPropertyTrace);

      return Ok(propertyTraceResponse);
    }

    [HttpGet("traces/{propertyId}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<PropertyTraceResponse>))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetTracesByPropertyId(Guid propertyId)
    {
      var propertyTraces = await GetPropertyTraces(propertyId);

      return Ok(propertyTraces);
    }

    private async Task<PropertyResponse> UpdateProperty(Guid propertyId, PropertyEntity property)
    {
      PropertyEntity updatedProperty = await _propertiesService.UpdateProperty(propertyId, property);
      PropertyResponse propertyResponse = _mapper.Map<PropertyResponse>(updatedProperty);
      propertyResponse.PropertyTraces = await _propertiesService.GetTracesByPropertyId(propertyId)
        .Select(_mapper.Map<PropertyTraceResponse>)
        .ToArrayAsync();

      return propertyResponse;
    }

    private async Task<IEnumerable<PropertyTraceResponse>> GetPropertyTraces(Guid propertyId)
    {
      var propertyTraces = await _propertiesService.GetTracesByPropertyId(propertyId)
        .Select(_mapper.Map<PropertyTraceResponse>)
        .ToArrayAsync();

      return propertyTraces;
    }
  }
}
