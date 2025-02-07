using AutoMapper;
using RealEstateProperties.Contracts.DTO.User;
using RealEstateProperties.Domain.Entities.Auth;

namespace RealEstateProperties.API.Mappers
{
  class AuthProfile : Profile
  {
    public AuthProfile()
    {
      CreateMap<UserEntity, UserEntity>();
      CreateMap<UserRegisterRequest, UserEntity>();
      CreateMap<UserEntity, UserResponse>();
    }
  }
}
