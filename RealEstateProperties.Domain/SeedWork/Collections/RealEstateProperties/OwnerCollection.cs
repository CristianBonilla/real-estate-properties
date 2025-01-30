using RealEstateProperties.Contracts.SeedData;
using RealEstateProperties.Domain.Entities;

namespace RealEstateProperties.Domain.SeedWork.Collections.RealEstateProperties
{
  class OwnerCollection : SeedDataCollection<OwnerEntity>
  {
    protected override OwnerEntity[] Collection => [
      new()
      {
        OwnerId = new("{69917363-3927-497a-9e2f-a2b783cf8222}"),
        Name = "Cristian Camilo Bonilla",
        Address = "Cl. 139 # 94 - 90",
        Photo = new(SeedImagesData.OwnerPhotos[0], "69917363-3927-497a-9e2f-a2b783cf8222.png"),
        Birthday = new(1995, 8, 11, 0, 0, 0, TimeSpan.FromHours(3)),
        Created = new(2023, 1, 27, 9, 32, 22, TimeSpan.FromHours(3))
      },
      new()
      {
        OwnerId = new("{ce860353-677d-480b-81e9-5a640a96851b}"),
        Name = "Mayerlis Cordero",
        Address = "Cl. 169 # 20-57",
        Photo = new(SeedImagesData.OwnerPhotos[1], "ce860353-677d-480b-81e9-5a640a96851b.jpg"),
        Birthday = new(2001, 9, 3, 0, 0, 0, TimeSpan.FromHours(3)),
        Created = new(2023, 1, 28, 13, 5, 18, TimeSpan.FromHours(3))
      }
    ];
  }
}
