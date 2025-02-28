using RealEstateProperties.Contracts.SeedData;
using RealEstateProperties.Domain.Entities;

namespace RealEstateProperties.Domain.SeedWork.Collections.RealEstateProperties
{
  class PropertyTraceCollection : SeedDataCollection<PropertyTraceEntity>
  {
    static readonly PropertyCollection _properties = RealEstatePropertiesCollection.Properties;

    protected override PropertyTraceEntity[] Collection => [
      new()
      {
        PropertyTraceId = new("{edc5e784-b24f-441c-a752-e1a44ad83350}"),
        PropertyId = _properties[0].PropertyId,
        Name = "Headland Waters Mount Martha Trace",
        Value = 1058000000M,
        Tax = 5430000M,
        DateSale = new(2024, 2, 11, 21, 5, 19, TimeSpan.FromHours(3)),
        Created = new(2024, 2, 11, 0, 0, 0, TimeSpan.FromHours(3))
      },
      new()
      {
        PropertyTraceId = new("{c3875a32-f307-41eb-97e8-90cb8f611600}"),
        PropertyId = _properties[1].PropertyId,
        Name = "Luyary Jeddo Trace",
        Value = 1117566000M,
        Tax = 6132000M,
        DateSale = new(2024, 5, 20, 8, 20, 0, TimeSpan.FromHours(3)),
        Created = new(2024, 5, 20, 0, 0, 0, TimeSpan.FromHours(3))
      },
      new()
      {
        PropertyTraceId = new("{41f0c4bc-bc02-4ea6-8940-aefdeb64aae1}"),
        PropertyId = _properties[2].PropertyId,
        Name = "Runneymede Trace",
        Value = 1008954000M,
        Tax = 5011000M,
        DateSale = new(2024, 12, 11, 10, 33, 8, TimeSpan.FromHours(3)),
        Created = new(2024, 12, 11, 0, 0, 0, TimeSpan.FromHours(3))
      },
      new()
      {
        PropertyTraceId = new("{af6166a1-09d1-4c71-8aa4-d1bceca2496d}"),
        PropertyId = _properties[3].PropertyId,
        Name = "Zuburnano Up Trace",
        Value = 1417844000M,
        Tax = 6234000M,
        DateSale = new(2024, 7, 25, 12, 7, 45, TimeSpan.FromHours(3)),
        Created = new(2024, 7, 25, 0, 0, 0, TimeSpan.FromHours(3))
      },
      new()
      {
        PropertyTraceId = new("{e72aa5ce-fb69-46a5-9c66-6900e6728f09}"),
        PropertyId = _properties[4].PropertyId,
        Name = "The Kingfisher Trace",
        Value = 1122910000M,
        Tax = 8950000M,
        DateSale = new(2024, 8, 22, 1, 2, 18, TimeSpan.FromHours(3)),
        Created = new(2024, 8, 22, 1, 2, 18, TimeSpan.FromHours(3))
      }
    ];
  }
}
