using RealEstateProperties.Domain.Entities;
using RealEstateProperties.Infrastructure.Contexts.RealEstateProperties;
using RealEstateProperties.Infrastructure.Repositories.RealEstateProperties.Interfaces;

namespace RealEstateProperties.Infrastructure.Repositories.RealEstateProperties
{
  public class PropertyRepository(IRealEstatePropertiesRepositoryContext context) : Repository<RealEstatePropertiesContext, PropertyEntity>(context), IPropertyRepository { }
}
