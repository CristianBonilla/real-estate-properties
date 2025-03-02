namespace RealEstateProperties.Domain.Helpers.Extensions
{
  static class EvaluateTypesExtensions
  {
    const string RealEstateProperties = nameof(RealEstateProperties);

    public static bool IsUserDefinedObject(this Type propertyType)
    {
      string propertyAssemblyName = propertyType.Assembly.FullName!;
      bool isClass = propertyType.IsClass;
      bool isStruct = propertyType.IsValueType && !propertyType.IsPrimitive && !propertyType.IsEnum;

      return (isClass || isStruct) && propertyAssemblyName.StartsWith(RealEstateProperties);
    }
  }
}
