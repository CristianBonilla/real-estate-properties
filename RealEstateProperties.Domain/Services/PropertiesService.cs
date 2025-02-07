using System.Net;
using RealEstateProperties.Contracts.Exceptions;
using RealEstateProperties.Contracts.Services;
using RealEstateProperties.Domain.Entities;
using RealEstateProperties.Domain.Helpers;
using RealEstateProperties.Infrastructure.Repositories.RealEstateProperties.Interfaces;

namespace RealEstateProperties.Domain.Services
{
  public class PropertiesService(
    IRealEstatePropertiesRepositoryContext context,
    IOwnerRepository ownerRepository,
    IPropertyRepository propertyRepository,
    IPropertyImageRepository propertyImageRepository,
    IPropertyTraceRepository propertyTraceRepository) : IPropertiesService
  {
    readonly IRealEstatePropertiesRepositoryContext _context = context;
    readonly IOwnerRepository _ownerRepository = ownerRepository;
    readonly IPropertyRepository _propertyRepository = propertyRepository;
    readonly IPropertyImageRepository _propertyImageRepository = propertyImageRepository;
    readonly IPropertyTraceRepository _propertyTraceRepository = propertyTraceRepository;

    public async Task<PropertyEntity> AddProperty(PropertyEntity property)
    {
      Random random = new();
      property.CodeInternal = random.Next();
      PropertyEntity addProperty = _propertyRepository.Create(property);
      _ = await _context.SaveAsync();

      return addProperty;
    }

    public async Task<PropertyEntity> DeleteProperty(Guid propertyId)
    {
      PropertyEntity? property = GetProperty(propertyId)
        ?? throw new ServiceErrorException(HttpStatusCode.NotFound, $"Property not found with property identifier \"{propertyId}\"");
      property = _propertyRepository.Delete(property);
      _ = await _context.SaveAsync();

      return property;
    }

    public async Task<PropertyEntity> UpdatePropertyPrice(Guid propertyId, decimal price)
    {
      PropertyEntity? property = GetProperty(propertyId)
        ?? throw new ServiceErrorException(HttpStatusCode.NotFound, $"Property not found with property identifier \"{propertyId}\"");
      property = _propertyRepository.Update(property);
      await _context.SaveAsync();

      return property;
    }

    public async Task<PropertyImageEntity> AddPropertyImage(PropertyImageEntity propertyImage)
    {
      propertyImage.Enabled = true;
      PropertyImageEntity addPropertyImage = _propertyImageRepository.Create(propertyImage);
      _ = await _context.SaveAsync();

      return addPropertyImage;
    }

    public async Task<PropertyImageEntity> UpdatePropertyImage(PropertyImageEntity propertyImage)
    {
      PropertyImageEntity updatePropertyImage = _propertyImageRepository.Update(propertyImage);
      _ = await _context.SaveAsync();

      return updatePropertyImage;
    }

    public Task<PropertyImageEntity?> FindPropertyImage(Guid propertyImageId) => Task.FromResult(GetPropertyImage(propertyImageId));

    public async Task<PropertyImageEntity> DeletePropertyImage(Guid propertyImageId)
    {
      PropertyImageEntity? propertyImage = GetPropertyImage(propertyImageId)
        ?? throw new ServiceErrorException(HttpStatusCode.NotFound, $"Property image not found with property image identifier \"{propertyImageId}\"");
      propertyImage = _propertyImageRepository.Delete(propertyImage);
      _ = await _context.SaveAsync();

      return propertyImage;
    }

    public async Task<PropertyTraceEntity> AddPropertyTrace(PropertyTraceEntity propertyTrace)
    {
      PropertyTraceEntity addPropertyTrace = _propertyTraceRepository.Create(propertyTrace);
      _ = await _context.SaveAsync();

      return addPropertyTrace;
    }

    public IAsyncEnumerable<(OwnerEntity Owner, PropertyEntity? Property, PropertyTraceEntity? PropertyTrace)> GetProperties()
    {
      var owners = _ownerRepository.GetAll()
        .GroupJoin(
          _propertyRepository.GetAll(),
          owner => owner.OwnerId,
          property => property.OwnerId,
          (owner, properties) => (owner, properties))
        .SelectMany(owner => owner.properties.DefaultIfEmpty(),
          (owner, property) => (owner.owner, property))
        .GroupJoin(
          _propertyTraceRepository.GetAll(),
          owner => owner.property?.PropertyId,
          propertyTrace => propertyTrace.PropertyId,
          (owner, propertyTraces) => (owner.owner, owner.property, propertyTraces))
        .SelectMany(owner => owner.propertyTraces.DefaultIfEmpty(),
          (owner, propertyTrace) => (owner.owner, owner.property, propertyTrace))
        .ToAsyncEnumerable();

      return owners;
    }

    public async IAsyncEnumerable<(OwnerEntity Owner, PropertyEntity? Property, PropertyTraceEntity? PropertyTrace)> GetProperties(string text)
    {
      var properties = GetProperties();
      await foreach (var (owner, property, propertyTrace) in properties)
      {
        bool ownerMatch = MatchesHelper.MatchesByText(
          owner,
          text,
          owner => owner.Name,
          owner => owner.Address);
        bool propertyMatch = property is not null && MatchesHelper.MatchesByText(
          property,
          text,
          property => property.Name,
          property => property.CodeInternal,
          property => property.Price,
          property => property.Year);
        bool propertyTraceMatch = propertyTrace is not null && MatchesHelper.MatchesByText(
          propertyTrace,
          text,
          propertyTrace => propertyTrace.Name,
          propertyTrace => propertyTrace.Value,
          propertyTrace => propertyTrace.Tax);
        if (ownerMatch || propertyMatch || propertyTraceMatch)
          yield return (owner, property, propertyTrace);
      }
    }

    public IAsyncEnumerable<PropertyImageEntity> GetImagesByPropertyId(Guid propertyId)
    {
      var propertyImages = _propertyImageRepository.GetByFilter(propertyImage => propertyImage.PropertyId == propertyId)
        .ToAsyncEnumerable();

      return propertyImages;
    }

    public IAsyncEnumerable<PropertyTraceEntity> GetTracesByPropertyId(Guid propertyId)
    {
      var propertyTraces = _propertyTraceRepository.GetByFilter(propertyTrace => propertyTrace.PropertyId == propertyId)
        .ToAsyncEnumerable();

      return propertyTraces;
    }

    private PropertyEntity? GetProperty(Guid propertyId)
    {
      PropertyEntity? property = _propertyRepository.Find([propertyId]);

      return property;
    }

    private PropertyImageEntity? GetPropertyImage(Guid propertyImageId)
    {
      PropertyImageEntity? propertyImage = _propertyImageRepository.Find([propertyImageId]);

      return propertyImage;
    }
  }
}
