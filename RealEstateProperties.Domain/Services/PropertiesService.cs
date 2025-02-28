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
      CheckOwnerExists(property.OwnerId);
      Random random = new();
      property.CodeInternal = random.Next();
      PropertyEntity addedProperty = _propertyRepository.Create(property);
      _ = await _context.SaveAsync();

      return addedProperty;
    }

    public async Task<PropertyEntity> UpdateProperty(Guid propertyId, PropertyEntity property)
    {
      CheckPropertyExists(propertyId);
      CheckOwnerExists(property.OwnerId);
      PropertyEntity updatedProperty = _propertyRepository.Update(property);
      await _context.SaveAsync();

      return updatedProperty;
    }

    public async Task<PropertyEntity> DeleteProperty(Guid propertyId)
    {
      PropertyEntity property = GetProperty(propertyId);
      PropertyEntity deletedProperty = _propertyRepository.Delete(property);
      _ = await _context.SaveAsync();

      return deletedProperty;
    }

    public IAsyncEnumerable<(OwnerEntity Owner, PropertyEntity? Property, PropertyTraceEntity? PropertyTrace)> GetProperties()
    {
      var owners = _ownerRepository.GetAll()
        .GroupJoin(
          _propertyRepository.GetAll(),
          owner => owner.OwnerId,
          property => property.OwnerId,
          (owner, properties) => (owner, properties))
        .SelectMany(
          owner => owner.properties.DefaultIfEmpty(),
          (owner, property) => (owner.owner, property))
        .GroupJoin(
          _propertyTraceRepository.GetAll(),
          owner => owner.property?.PropertyId,
          propertyTrace => propertyTrace.PropertyId,
          (owner, propertyTraces) => (owner.owner, owner.property, propertyTraces))
        .SelectMany(
          owner => owner.propertyTraces.DefaultIfEmpty(),
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

    public Task<PropertyEntity> FindPropertyById(Guid propertyId) => Task.FromResult(GetProperty(propertyId));

    public async Task<PropertyImageEntity> AddPropertyImage(Guid propertyId, byte[] image, string imageName)
    {
      CheckPropertyExists(propertyId);
      PropertyImageEntity propertyImage = new()
      {
        PropertyId = propertyId,
        Enabled = true,
        Image = image,
        ImageName = imageName
      };
      PropertyImageEntity addedPropertyImage = _propertyImageRepository.Create(propertyImage);
      _ = await _context.SaveAsync();

      return addedPropertyImage;
    }

    public async Task<PropertyImageEntity> UpdatePropertyImage(Guid propertyId, Guid propertyImageId, byte[] image, string imageName)
    {
      PropertyImageEntity propertyImage = GetPropertyImage(propertyId, propertyImageId);
      propertyImage.Enabled = true;
      propertyImage.Image = image;
      propertyImage.ImageName = imageName;
      PropertyImageEntity updatedPropertyImage = _propertyImageRepository.Update(propertyImage);
      _ = await _context.SaveAsync();

      return updatedPropertyImage;
    }

    public async Task<PropertyImageEntity> DeletePropertyImage(Guid propertyId, Guid propertyImageId)
    {
      PropertyImageEntity propertyImage = GetPropertyImage(propertyId, propertyImageId);
      propertyImage = _propertyImageRepository.Delete(propertyImage);
      _ = await _context.SaveAsync();

      return propertyImage;
    }

    public (string PropertyName, IEnumerable<PropertyImageEntity> PropertyImages) GetPropertyImages(Guid propertyId)
    {
      PropertyEntity property = GetProperty(propertyId);
      var propertyImages = _propertyImageRepository.GetByFilter(propertyImage => propertyImage.PropertyId == propertyId);

      return (property.Name, propertyImages);
    }

    public async Task<PropertyTraceEntity> AddPropertyTrace(PropertyTraceEntity propertyTrace)
    {
      CheckPropertyExists(propertyTrace.PropertyId);
      PropertyTraceEntity addPropertyTrace = _propertyTraceRepository.Create(propertyTrace);
      _ = await _context.SaveAsync();

      return addPropertyTrace;
    }

    public IAsyncEnumerable<PropertyTraceEntity> GetPropertyTraces(Guid propertyId)
    {
      CheckPropertyExists(propertyId);
      var propertyTraces = _propertyTraceRepository.GetByFilter(propertyTrace => propertyTrace.PropertyId == propertyId)
        .ToAsyncEnumerable();

      return propertyTraces;
    }

    private void CheckOwnerExists(Guid ownerId)
    {
      bool existingOwner = _ownerRepository.Exists(owner => owner.OwnerId == ownerId);
      if (!existingOwner)
        throw new ServiceErrorException(HttpStatusCode.NotFound, $"Owner not found with owner identifier \"{ownerId}\"");
    }

    private void CheckPropertyExists(Guid propertyId)
    {
      bool existingProperty = _propertyRepository.Exists(property => property.PropertyId == propertyId);
      if (!existingProperty)
        throw new ServiceErrorException(HttpStatusCode.NotFound, $"Property not found with property identifier \"{propertyId}\"");
    }

    private PropertyEntity GetProperty(Guid propertyId)
    {
      PropertyEntity property = _propertyRepository.Find([propertyId])
        ?? throw new ServiceErrorException(HttpStatusCode.NotFound, $"Property not found with property identifier \"{propertyId}\"");

      return property;
    }

    private PropertyImageEntity GetPropertyImage(Guid propertyId, Guid propertyImageId)
    {
      PropertyImageEntity propertyImage = _propertyImageRepository.Find(propertyImage => propertyImage.PropertyId == propertyId && propertyImage.PropertyImageId == propertyImageId)
        ?? throw new ServiceErrorException(HttpStatusCode.NotFound, $"Property image not found with property identifier \"{propertyId}\" or property image identifier \"{propertyImageId}\"");

      return propertyImage;
    }
  }
}
