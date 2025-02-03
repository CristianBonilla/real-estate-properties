using RealEstateProperties.Contracts.Services;
using RealEstateProperties.Domain.Entities;
using RealEstateProperties.Infrastructure.Repositories.RealEstateProperties.Interfaces;

namespace RealEstateProperties.Domain.Services
{
  public class OwnerService(IRealEstatePropertiesRepositoryContext context, IOwnerRepository ownerRepository) : IOwnerService
  {
    readonly IRealEstatePropertiesRepositoryContext _context = context;
    readonly IOwnerRepository _ownerRepository = ownerRepository;

    public async Task<OwnerEntity> AddOwner(OwnerEntity owner)
    {
      owner = _ownerRepository.Create(owner);
      _ = await _context.SaveAsync();

      return owner;
    }

    public Task<OwnerEntity?> FindOwnerById(Guid ownerId) => Task.FromResult(_ownerRepository.Find([ownerId]));

    public IAsyncEnumerable<OwnerEntity> GetOwners()
    {
      var owners = _ownerRepository.GetAll(owner => owner.OrderBy(order => order.Name))
        .ToAsyncEnumerable();

      return owners;
    }
  }
}
