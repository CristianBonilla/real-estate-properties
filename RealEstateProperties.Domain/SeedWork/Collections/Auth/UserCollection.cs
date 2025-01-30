using RealEstateProperties.Contracts.SeedData;
using RealEstateProperties.Domain.Entities.Auth;
using RealEstateProperties.Domain.Helpers;

namespace RealEstateProperties.Domain.SeedWork.Collections.Auth
{
  class UserCollection : SeedDataCollection<UserEntity>
  {
    readonly (string Password, byte[] SaltBytes) _hash = HashPasswordHelper.Create("C.C1023944678.1995");

    protected override UserEntity[] Collection => [
      new()
      {
        UserId = new("{c880a1fd-2c32-46cb-b744-a6fad6175a53}"),
        DocumentNumber = "1023944678",
        Mobile = "+573163534451",
        Username = "chris__boni",
        Password = _hash.Password,
        Email = "cristian10camilo95@gmail.com",
        Firstname = "Cristian Camilo",
        Lastname = "Bonilla",
        IsActive = true,
        Salt = _hash.SaltBytes
      }
    ];
  }
}
