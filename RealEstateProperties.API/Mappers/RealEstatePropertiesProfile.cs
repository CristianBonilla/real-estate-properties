using AutoMapper;
using RealEstateProperties.Contracts.DTO.Owner;
using RealEstateProperties.Contracts.DTO.Properties;
using RealEstateProperties.Domain.Entities;

namespace RealEstateProperties.API.Mappers
{
  class RealEstatePropertiesProfile : Profile
  {
    public RealEstatePropertiesProfile()
    {
      CreateMap<OwnerRequest, OwnerEntity>()
        .ForMember(member => member.OwnerId, options => options.Ignore())
        .ForMember(member => member.Photo, options => options.Ignore())
        .ForMember(member => member.PhotoName, options => options.Ignore())
        .ForMember(member => member.Created, options => options.Ignore())
        .ForMember(member => member.Version, options => options.Ignore())
        .ForMember(member => member.Properties, options => options.Ignore());
      CreateMap<OwnerEntity, OwnerResponse>()
        .ReverseMap()
        .ForMember(member => member.Version, options => options.Ignore())
        .ForMember(member => member.Properties, options => options.Ignore());
      CreateMap<PropertyRequest, PropertyEntity>()
        .ForMember(member => member.PropertyId, options => options.Ignore())
        .ForMember(member => member.Created, options => options.Ignore())
        .ForMember(member => member.Version, options => options.Ignore())
        .ForMember(member => member.Owner, options => options.Ignore())
        .ForMember(member => member.Images, options => options.Ignore())
        .ForMember(member => member.Traces, options => options.Ignore());
      CreateMap<PropertyEntity, PropertyResponse>()
        .ForMember(member => member.PropertyTraces, options => options.Ignore())
        .ReverseMap()
        .ForMember(member => member.Version, options => options.Ignore())
        .ForMember(member => member.Owner, options => options.Ignore())
        .ForMember(member => member.Images, options => options.Ignore())
        .ForMember(member => member.Traces, options => options.Ignore());
      CreateMap<PropertyImageRequest, PropertyImageEntity>()
        .ForMember(member => member.PropertyImageId, options => options.Ignore())
        .ForMember(member => member.Created, options => options.Ignore())
        .ForMember(member => member.Version, options => options.Ignore())
        .ForMember(member => member.Property, options => options.Ignore());
      CreateMap<PropertyTraceRequest, PropertyTraceEntity>()
        .ForMember(member => member.PropertyTraceId, options => options.Ignore())
        .ForMember(member => member.Created, options => options.Ignore())
        .ForMember(member => member.Version, options => options.Ignore())
        .ForMember(member => member.Property, options => options.Ignore());
      CreateMap<PropertyTraceEntity, PropertyTraceResponse>()
        .ReverseMap()
        .ForMember(member => member.Version, options => options.Ignore())
        .ForMember(member => member.Property, options => options.Ignore());
    }
  }
}
