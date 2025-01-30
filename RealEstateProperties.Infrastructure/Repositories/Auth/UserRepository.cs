using RealEstateProperties.Domain.Entities.Auth;
using RealEstateProperties.Infrastructure.Contexts.RealEstateProperties;
using RealEstateProperties.Infrastructure.Repositories.Auth.Interfaces;
using RealEstateProperties.Infrastructure.Repositories.RealEstateProperties.Interfaces;

namespace RealEstateProperties.Infrastructure.Repositories.Auth
{
  public class UserRepository(IRealEstatePropertiesRepositoryContext context) : Repository<RealEstatePropertiesContext, UserEntity>(context), IUserRepository { }
}
