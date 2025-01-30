using RealEstateProperties.Contracts.SeedData;
using RealEstateProperties.Domain.SeedWork.Collections;

namespace RealEstateProperties.Domain.SeedWork
{
  public class SeedData : ISeedData
  {
    public SeedAuthData Auth => new()
    {
      Users = AuthCollection.Users
    };

    public SeedRealEstatePropertiesData RealEstateProperties => new()
    {
      Owners = RealEstatePropertiesCollection.Owners,
      Properties = RealEstatePropertiesCollection.Properties,
      PropertyImages = RealEstatePropertiesCollection.PropertyImages,
      PropertyTraces = RealEstatePropertiesCollection.PropertyTraces
    };
  }
}
