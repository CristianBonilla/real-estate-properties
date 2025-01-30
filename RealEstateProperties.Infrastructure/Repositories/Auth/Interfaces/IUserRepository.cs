using RealEstateProperties.Contracts.Repository;
using RealEstateProperties.Domain.Entities.Auth;
using RealEstateProperties.Infrastructure.Contexts.RealEstateProperties;

namespace RealEstateProperties.Infrastructure.Repositories.Auth.Interfaces
{
  public interface IUserRepository : IRepository<RealEstatePropertiesContext, UserEntity> { }
}
