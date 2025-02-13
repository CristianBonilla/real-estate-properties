using RealEstateProperties.Domain.Entities;

namespace RealEstateProperties.Contracts.Services
{
  public interface IPropertiesService
  {
    Task<PropertyEntity> AddProperty(PropertyEntity property);
    Task<PropertyEntity> DeleteProperty(Guid propertyId);
    Task<PropertyEntity> UpdatePropertyPrice(Guid propertyId, decimal price);
    Task<PropertyImageEntity> AddPropertyImage(Guid propertyId, byte[] image, string imageName);
    Task<PropertyImageEntity> UpdatePropertyImage(Guid propertyId, Guid propertyImageId, byte[] image, string imageName);
    Task<PropertyImageEntity> FindPropertyImage(Guid propertyId, Guid propertyImageId);
    Task<PropertyImageEntity> DeletePropertyImage(Guid propertyId, Guid propertyImageId);
    Task<PropertyTraceEntity> AddPropertyTrace(PropertyTraceEntity propertyTrace);
    IAsyncEnumerable<(OwnerEntity Owner, PropertyEntity? Property, PropertyTraceEntity? PropertyTrace)> GetProperties();
    IAsyncEnumerable<(OwnerEntity Owner, PropertyEntity? Property, PropertyTraceEntity? PropertyTrace)> GetProperties(string text);
    (string PropertyName, IEnumerable<PropertyImageEntity> PropertyImages) GetImagesByPropertyId(Guid propertyId);
    IAsyncEnumerable<PropertyTraceEntity> GetTracesByPropertyId(Guid propertyId);
  }
}
