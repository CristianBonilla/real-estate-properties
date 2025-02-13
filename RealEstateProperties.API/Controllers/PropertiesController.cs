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
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async IAsyncEnumerable<PropertyTraceResponse> GetTracesByPropertyId(Guid propertyId)
    {
      var propertyTraces = _propertiesService.GetTracesByPropertyId(propertyId);
      await foreach (PropertyTraceEntity propertyTrace in propertyTraces)
        yield return _mapper.Map<PropertyTraceResponse>(propertyTrace);
    }

    [HttpGet("images/{propertyId}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetImagesByPropertyId(Guid propertyId)
    {
      var propertyImages = await _propertiesService.GetImagesByPropertyId(propertyId)
        .ToArrayAsync();
      if (propertyImages.Length == 0)
        throw new ServiceErrorException(HttpStatusCode.NotFound, $"Property images not found with property identifier \"{propertyId}\"");
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
      string date = DateTime.Now.ToString("yyyy-MM-dd HH-mm-ss");
      string zipName = $"{propertyId}__{date}.zip";
      memoryStream.Seek(0, SeekOrigin.Begin);

      return File(memoryStream.ToArray(), "application/zip", zipName);
    }
  }
}
