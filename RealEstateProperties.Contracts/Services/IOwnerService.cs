using RealEstateProperties.Domain.Entities;

namespace RealEstateProperties.Contracts.Services
{
  public interface IOwnerService
  {
    Task<OwnerEntity> AddOwner(OwnerEntity owner);
    Task<OwnerEntity?> FindOwnerById(Guid ownerId);
    IAsyncEnumerable<OwnerEntity> GetOwners();
  }
}
