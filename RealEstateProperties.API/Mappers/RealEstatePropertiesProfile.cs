using AutoMapper;
using RealEstateProperties.API.Mappers.Converters;
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
        .ForMember(member => member.CodeInternal, options => options.Ignore())
        .ForMember(member => member.Version, options => options.Ignore())
        .ForMember(member => member.Owner, options => options.Ignore())
        .ForMember(member => member.Images, options => options.Ignore())
        .ForMember(member => member.Traces, options => options.Ignore());
      CreateMap<PropertyImageEntity, PropertyImageResponse>()
        .ReverseMap()
        .ForMember(member => member.Image, options => options.Ignore())
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
      CreateMap<IAsyncEnumerable<(
        OwnerEntity Owner,
        PropertyEntity? Property,
        PropertyTraceEntity? PropertyTrace)>,
        IAsyncEnumerable<PropertiesResult>>()
        .ConvertUsing<PropertiesFilterConverter>();
    }
  }
}
