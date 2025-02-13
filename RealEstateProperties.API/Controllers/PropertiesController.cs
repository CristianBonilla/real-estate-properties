using System.IO.Compression;
using System.Net;
using Asp.Versioning;
using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RealEstateProperties.API.Filters;
using RealEstateProperties.Contracts.DTO.Properties;
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

    [HttpGet("traces/{propertyId}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IAsyncEnumerable<PropertyTraceResponse>))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async IAsyncEnumerable<PropertyTraceResponse> GetTracesByPropertyId(Guid propertyId)
    {
      var propertyTraces = _propertiesService.GetTracesByPropertyId(propertyId);
      await foreach (PropertyTraceEntity propertyTrace in propertyTraces)
        yield return _mapper.Map<PropertyTraceResponse>(propertyTrace);
    }

    [HttpGet("images/{propertyId}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
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
  }
}
