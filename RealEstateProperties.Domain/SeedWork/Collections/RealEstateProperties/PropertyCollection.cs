using RealEstateProperties.Contracts.SeedData;
using RealEstateProperties.Domain.Entities;

namespace RealEstateProperties.Domain.SeedWork.Collections.RealEstateProperties
{
  class PropertyCollection : SeedDataCollection<PropertyEntity>
  {
    static readonly OwnerCollection _owners = RealEstatePropertiesCollection.Owners;

    protected override PropertyEntity[] Collection => [
      new()
      {
        PropertyId = new("{3e85514e-b3d5-4b3f-acb2-84f4a7865386}"),
        OwnerId = _owners[0].OwnerId,
        Name = "Headland Waters Mount Martha",
        Address = "6677 Schroeder Avenue",
        Price = 1358000000,
        CodeInternal = 34432111,
        Year = 2018,
        Created = new(2023, 1, 27, 11, 1, 26, TimeSpan.FromHours(3))
      },
      new()
      {
        PropertyId = new("{00242191-a02d-452b-b149-4c56147ade6b}"),
        OwnerId = _owners[0].OwnerId,
        Name = "Luyary Jeddo",
        Address = "Moussaouidreef 8",
        Price = 1297566000,
        CodeInternal = 98801123,
        Year = 2021,
        Created = new(2023, 1, 27, 12, 5, 0, TimeSpan.FromHours(3))
      },
      new()
      {
        PropertyId = new("{ea10680e-44f3-44d1-84ee-f031f4746ac6}"),
        OwnerId = _owners[0].OwnerId,
        Name = "Runneymede",
        Address = "8098 Yundt Mission",
        Price = 1188954000,
        CodeInternal = 11983367,
        Year = 2020,
        Created = new(2023, 1, 27, 18, 50, 0, TimeSpan.FromHours(3))
      },
      new()
      {
        PropertyId = new("{5a42e202-5641-4abc-a39e-2e9bbc6ec299}"),
        OwnerId = _owners[1].OwnerId,
        Name = "Zuburnano Up",
        Address = "701, avenue de Guilbert",
        Price = 1877544000,
        CodeInternal = 87711809,
        Year = 2021,
        Created = new(2023, 1, 28, 20, 0, 27, TimeSpan.FromHours(3))
      },
      new()
      {
        PropertyId = new("{319ab07a-8fbd-4686-8ca3-76b5d4acd2a8}"),
        OwnerId = _owners[1].OwnerId,
        Name = "The Kingfisher",
        Address = "193 Kshlerin Spring",
        Price = 1988411000,
        CodeInternal = 43309922,
        Year = 2020,
        Created = new(2023, 1, 28, 21, 16, 0, TimeSpan.FromHours(3))
      }
    ];
  }
}
