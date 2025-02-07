using RealEstateProperties.Domain.Entities;

namespace RealEstateProperties.Contracts.Services
{
  public interface IPropertiesService
  {
    Task<PropertyEntity> AddProperty(PropertyEntity property);
    Task<PropertyEntity> DeleteProperty(Guid propertyId);
    Task<PropertyEntity> UpdatePropertyPrice(Guid propertyId, decimal price);
    Task<PropertyImageEntity> AddPropertyImage(PropertyImageEntity propertyImage);
    Task<PropertyImageEntity> UpdatePropertyImage(PropertyImageEntity propertyImage);
    Task<PropertyImageEntity?> FindPropertyImage(Guid propertyImageId);
    Task<PropertyImageEntity> DeletePropertyImage(Guid propertyImageId);
    Task<PropertyTraceEntity> AddPropertyTrace(PropertyTraceEntity propertyTrace);
    IAsyncEnumerable<(OwnerEntity Owner, PropertyEntity? Property, PropertyTraceEntity? PropertyTrace)> GetProperties();
    IAsyncEnumerable<(OwnerEntity Owner, PropertyEntity? Property, PropertyTraceEntity? PropertyTrace)> GetProperties(string text);
    IAsyncEnumerable<PropertyImageEntity> GetImagesByPropertyId(Guid propertyId);
    IAsyncEnumerable<PropertyTraceEntity> GetTracesByPropertyId(Guid propertyId);
  }
}
