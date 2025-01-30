using RealEstateProperties.Contracts.SeedData;
using RealEstateProperties.Domain.Entities;

namespace RealEstateProperties.Domain.SeedWork.Collections.RealEstateProperties
{
  class PropertyImageCollection : SeedDataCollection<PropertyImageEntity>
  {
    static readonly PropertyCollection _properties = RealEstatePropertiesCollection.Properties;

    protected override PropertyImageEntity[] Collection => [
      new()
      {
        PropertyImageId = new("{2a657822-056c-4dd9-8491-489998230a7c}"),
        PropertyId = _properties[0].PropertyId,
        Image = SeedImagesData.PropertyImages[0],
        ImageName = "2a657822-056c-4dd9-8491-489998230a7c.webp",
        Enabled = true,
        Created = _properties[0].Created
      },
      new()
      {
        PropertyImageId = new("{bf86ccd4-8b41-417f-906c-8f85610e9085}"),
        PropertyId = _properties[1].PropertyId,
        Image = SeedImagesData.PropertyImages[1],
        ImageName = "bf86ccd4-8b41-417f-906c-8f85610e9085.webp",
        Enabled = true,
        Created = _properties[1].Created
      },
      new()
      {
        PropertyImageId = new("{56163b41-6d7d-46c8-a9b5-4eae28e9cef0}"),
        PropertyId = _properties[2].PropertyId,
        Image = SeedImagesData.PropertyImages[2],
        ImageName = "56163b41-6d7d-46c8-a9b5-4eae28e9cef0.webp",
        Enabled = true,
        Created = _properties[2].Created
      },
      new()
      {
        PropertyImageId = new("{4e5a09b6-f580-4864-942d-369920e0574c}"),
        PropertyId = _properties[3].PropertyId,
        Image = SeedImagesData.PropertyImages[3],
        ImageName = "4e5a09b6-f580-4864-942d-369920e0574c.webp",
        Enabled = true,
        Created = _properties[3].Created
      },
      new()
      {
        PropertyImageId = new("{7648a047-13ba-4eea-b9ef-cd4f0c856de2}"),
        PropertyId = _properties[4].PropertyId,
        Image = SeedImagesData.PropertyImages[4],
        ImageName = "7648a047-13ba-4eea-b9ef-cd4f0c856de2.webp",
        Enabled = true,
        Created = _properties[4].Created
      }
    ];
  }
}
