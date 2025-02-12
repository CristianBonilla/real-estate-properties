using System.Net;
using RealEstateProperties.Contracts.Exceptions;
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
      OwnerEntity addOwner = _ownerRepository.Create(owner);
      _ = await _context.SaveAsync();

      return addOwner;
    }

    public async Task<OwnerEntity> AddOrUpdateOwnerPhoto(Guid ownerId, byte[] photo, string photoName)
    {
      OwnerEntity owner = _ownerRepository.Find([ownerId])
        ?? throw new ServiceErrorException(HttpStatusCode.NotFound, $"Owner not found with owner identifier \"{ownerId}\"");
      owner.Photo = photo;
      owner.PhotoName = photoName;
      OwnerEntity updatedOwner = _ownerRepository.Update(owner);
      _ = await _context.SaveAsync();

      return updatedOwner;
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
