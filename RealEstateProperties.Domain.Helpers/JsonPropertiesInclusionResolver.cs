using System.Linq.Expressions;
using System.Reflection;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using RealEstateProperties.Domain.Helpers.Extensions;

namespace RealEstateProperties.Domain.Helpers
{
  public class JsonPropertiesInclusionResolver<TObject>(params Expression<Func<TObject, object>>[] includedProperties) : CamelCasePropertyNamesContractResolver where TObject : class
  {
    readonly Expression<Func<TObject, object>>[] _includedProperties = includedProperties;

    public IContractResolver Instance => new JsonPropertiesInclusionResolver<TObject>(_includedProperties);

    protected override JsonProperty CreateProperty(MemberInfo member, MemberSerialization memberSerialization)
    {
      JsonProperty jsonProperty = base.CreateProperty(member, memberSerialization);
      PropertyInfo property = (PropertyInfo)member;
      if (!_includedProperties.IsIncludedProperty(property))
      {
        jsonProperty.ShouldSerialize = _ => false;
        jsonProperty.ShouldDeserialize = _ => false;
      }

      return jsonProperty;
    }
  }
}
