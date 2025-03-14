using RealEstateProperties.Contracts.Repository;
using RealEstateProperties.Domain.Entities;
using RealEstateProperties.Infrastructure.Contexts.RealEstateProperties;

namespace RealEstateProperties.Infrastructure.Repositories.RealEstateProperties.Interfaces;

public interface IOwnerRepository : IRepository<RealEstatePropertiesContext, OwnerEntity> { }
