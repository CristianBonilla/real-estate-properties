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
    public async IAsyncEnumerable<PropertiesResult> Convert(
      IAsyncEnumerable<(
        OwnerEntity Owner,
        PropertyEntity? Property,
        PropertyTraceEntity? PropertyTrace)> source,
      IAsyncEnumerable<PropertiesResult> destination,
      ResolutionContext context)
    {
      IRuntimeMapper mapper = context.Mapper;
      var sources = await source.ToArrayAsync();
      var owners = sources.Select(source => source.Owner);
      var properties = sources.Select(source => source.Property);
      var propertyTraces = sources.Select(source => source.PropertyTrace);
      var propertiesResult = owners
        .Distinct()
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
      foreach (PropertiesResult propertyResult in propertiesResult)
        yield return propertyResult;
    }

    private static PropertyResponse? GetProperty(IRuntimeMapper mapper, PropertyEntity? property, IEnumerable<PropertyTraceEntity?> propertyTraces)
    {
      PropertyResponse? propertyResponse = mapper.Map<PropertyResponse?>(property);
      if (propertyResponse is not null)
        propertyResponse.PropertyTraces = propertyTraces.Select(mapper.Map<PropertyTraceResponse>);

      return propertyResponse;
    }
  }
}
