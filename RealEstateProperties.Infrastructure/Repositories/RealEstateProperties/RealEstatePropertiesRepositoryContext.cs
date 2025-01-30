using RealEstateProperties.Infrastructure.Contexts.RealEstateProperties;
using RealEstateProperties.Infrastructure.Repositories.RealEstateProperties.Interfaces;

namespace RealEstateProperties.Infrastructure.Repositories.RealEstateProperties
{
  public class RealEstatePropertiesRepositoryContext(RealEstatePropertiesContext context) : RepositoryContext<RealEstatePropertiesContext>(context), IRealEstatePropertiesRepositoryContext { }
}
