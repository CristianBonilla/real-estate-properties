using RealEstateProperties.Contracts.SeedData;
using RealEstateProperties.Domain.Entities.Auth;

namespace RealEstateProperties.Domain.SeedWork.Collections.Auth
{
  class UserCollection : SeedDataCollection<UserEntity>
  {
    const string PASSWORD = "oO63zcP14ylquh+FDz/NdI3v2Zltfk2p4gmLcZ6bmmwcwCJlEMjIH95egAt/BGZiWjKVTkblXoQOuxv/OAFegw==";
    readonly byte[] _saltBytes = [160, 238, 183, 205, 195, 245, 227, 41, 106, 186, 31, 133, 15, 63, 205, 116, 141, 239, 217, 153, 109, 126, 77, 169, 226, 9, 139, 113, 158, 155, 154, 108];

    protected override UserEntity[] Collection => [
      new()
      {
        UserId = new("{c880a1fd-2c32-46cb-b744-a6fad6175a53}"),
        DocumentNumber = "1023944678",
        Mobile = "+573163534451",
        Username = "chris__boni",
        Password = PASSWORD,
        Email = "cristian10camilo95@gmail.com",
        Firstname = "Cristian Camilo",
        Lastname = "Bonilla",
        IsActive = true,
        Salt = _saltBytes
      }
    ];
  }
}
