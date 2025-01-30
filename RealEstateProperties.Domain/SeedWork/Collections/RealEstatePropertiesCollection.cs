using RealEstateProperties.Domain.SeedWork.Collections.RealEstateProperties;

namespace RealEstateProperties.Domain.SeedWork.Collections
{
  class RealEstatePropertiesCollection
  {
    public static OwnerCollection Owners => new();
    public static PropertyCollection Properties => new();
    public static PropertyImageCollection PropertyImages => new();
    public static PropertyTraceCollection PropertyTraces => new();
  }
}
