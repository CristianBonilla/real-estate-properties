using System.IO.Compression;
using Asp.Versioning;
using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RealEstateProperties.API.Filters;
using RealEstateProperties.API.Utils;
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
      PropertyEntity property = await _propertiesService.FindPropertyById(propertyId);
      PropertyEntity updatedProperty = _mapper.Map(propertyRequest, property);
      PropertyResponse propertyResponse = await UpdateProperty(propertyId, updatedProperty);

      return Ok(propertyResponse);
    }

    [HttpDelete("{propertyId}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(PropertyResponse))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> DeleteProperty(Guid propertyId)
    {
      PropertyEntity property = await _propertiesService.DeleteProperty(propertyId);

      return Ok(_mapper.Map<PropertyResponse>(property));
    }

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

    [HttpPost("images/{propertyId}")]
    [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(PropertyImageResponse))]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> AddPropertyImage(Guid propertyId, IFormFile image)
    {
      if (image.Length <= 0)
        return StatusCode(StatusCodes.Status400BadRequest, "There is no property image to process");
      byte[] imageBytes = await PropertyImageStreamUtil.GetImageBytes(image);
      PropertyImageEntity propertyImage = await _propertiesService.AddPropertyImage(propertyId, imageBytes, image.FileName);
      PropertyImageResponse propertyImageResponse = _mapper.Map<PropertyImageResponse>(propertyImage);

      return CreatedAtAction(nameof(AddPropertyImage), propertyImageResponse);
    }

    [HttpPut("images")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(PropertyImageResponse))]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> UpdatePropertyImage([FromQuery] Guid propertyId, [FromQuery] Guid propertyImageId, IFormFile image)
    {
      if (image.Length <= 0)
        return StatusCode(StatusCodes.Status400BadRequest, "There is no property image to process");
      byte[] imageBytes = await PropertyImageStreamUtil.GetImageBytes(image);
      PropertyImageEntity propertyImage = await _propertiesService.UpdatePropertyImage(propertyId, propertyImageId, imageBytes, image.FileName);
      PropertyImageResponse propertyImageResponse = _mapper.Map<PropertyImageResponse>(propertyImage);

      return Ok(propertyImageResponse);
    }

    [HttpDelete("images")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(PropertyImageResponse))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> DeletePropertyImage([FromQuery] Guid propertyId, [FromQuery] Guid propertyImageId)
    {
      PropertyImageEntity propertyImage = await _propertiesService.DeletePropertyImage(propertyId, propertyImageId);
      PropertyImageResponse propertyImageResponse = _mapper.Map<PropertyImageResponse>(propertyImage);

      return Ok(propertyImageResponse);
    }

    [HttpGet("images/{propertyId}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<PropertyImageResponse>))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public IActionResult GetPropertyImages(Guid propertyId)
    {
      var (_, propertyImages) = _propertiesService.GetPropertyImages(propertyId);

      return Ok(propertyImages.Select(_mapper.Map<PropertyImageResponse>));
    }

    [HttpGet("images/{propertyId}/files")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(FileContentResult))]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetPropertyImagesFiles(Guid propertyId)
    {
      var (propertyName, propertyImages) = _propertiesService.GetPropertyImages(propertyId);
      int length = propertyImages.Count();
      if (length == 1)
        return StatusCode(StatusCodes.Status400BadRequest, $"There are no images to process");
      if (length == 1)
      {
        PropertyImageEntity propertyImage = propertyImages.Single();

        return File(propertyImage.Image, "application/octet-stream", $"{propertyName} {propertyImage.ImageName}");
      }
      using MemoryStream memoryStream = new();
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

    [HttpPost("traces")]
    [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(PropertyTraceResponse))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
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
    public async Task<IActionResult> GetPropertyTraces(Guid propertyId)
    {
      var propertyTraces = await _propertiesService.GetPropertyTraces(propertyId)
        .Select(_mapper.Map<PropertyTraceResponse>)
        .ToArrayAsync();

      return Ok(propertyTraces);
    }

    private async Task<PropertyResponse> UpdateProperty(Guid propertyId, PropertyEntity property)
    {
      PropertyEntity updatedProperty = await _propertiesService.UpdateProperty(propertyId, property);
      PropertyResponse propertyResponse = _mapper.Map<PropertyResponse>(updatedProperty);
      propertyResponse.PropertyTraces = await _propertiesService.GetPropertyTraces(propertyId)
        .Select(_mapper.Map<PropertyTraceResponse>)
        .ToArrayAsync();

      return propertyResponse;
    }
  }
}
