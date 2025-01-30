using RealEstateProperties.Domain.Entities;
using RealEstateProperties.Infrastructure.Contexts.RealEstateProperties;
using RealEstateProperties.Infrastructure.Repositories.RealEstateProperties.Interfaces;

namespace RealEstateProperties.Infrastructure.Repositories.RealEstateProperties
{
  public class PropertyImageRepository(IRealEstatePropertiesRepositoryContext context) : Repository<RealEstatePropertiesContext, PropertyImageEntity>(context), IPropertyImageRepository { }
}
