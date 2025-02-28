using RealEstateProperties.Domain.Entities;

namespace RealEstateProperties.Contracts.Services
{
  public interface IOwnerService
  {
    Task<OwnerEntity> AddOwner(OwnerEntity owner);
    Task<OwnerEntity> DeleteOwner(Guid ownerId);
    IAsyncEnumerable<OwnerEntity> GetOwners();
    Task<OwnerEntity> FindOwnerById(Guid ownerId);
    Task<OwnerEntity> AddOrUpdateOwnerPhoto(Guid ownerId, byte[] photo, string photoName);
  }
}
