using AutoMapper;
using RealEstateProperties.Contracts.DTO.Owner;
using RealEstateProperties.Contracts.DTO.Properties;
using RealEstateProperties.Domain.Entities;

namespace RealEstateProperties.API.Mappers.Converters
{
  class PropertiesFilterConverter : ITypeConverter<IAsyncEnumerable<(
    OwnerEntity Owner,
    PropertyEntity? Property,
    PropertyTraceEntity? PropertyTrace)>,
    IAsyncEnumerable<PropertiesResult>>
  {
    public IAsyncEnumerable<PropertiesResult> Convert(
      IAsyncEnumerable<(
        OwnerEntity Owner,
        PropertyEntity? Property,
        PropertyTraceEntity? PropertyTrace)> source,
      IAsyncEnumerable<PropertiesResult> destination,
      ResolutionContext context)
    {
      IRuntimeMapper mapper = context.Mapper;
      var owners = source.Select(source => source.Owner);
      var properties = source.Select(source => source.Property);
      var propertyTraces = source.Select(source => source.PropertyTrace);
      
      return owners
        .GroupJoin(
          properties,
          owner => owner.OwnerId,
          property => property?.OwnerId,
          (owner, properties) => new PropertiesResult
          {
            Owner = mapper.Map<OwnerResponse>(owner),
            Properties = properties
              .GroupJoin(
                propertyTraces,
                property => property?.PropertyId,
                propertyTrace => propertyTrace?.PropertyId,
                (property, propertyTraces) => GetProperty(mapper, property, propertyTraces))
          }
        );
    }

    private static PropertyResponse? GetProperty(IRuntimeMapper mapper, PropertyEntity? property, IAsyncEnumerable<PropertyTraceEntity?> propertyTraces)
    {
      PropertyResponse? propertyResponse = mapper.Map<PropertyResponse?>(property);
      if (propertyResponse is not null)
        propertyResponse.PropertyTraces = propertyTraces.Select(mapper.Map<PropertyTraceResponse>);

      return propertyResponse;
    }
  }
}
