using System.Linq.Expressions;
using System.Reflection;
using RealEstateProperties.Domain.Helpers.Extensions;

namespace RealEstateProperties.Domain.Helpers
{
  public static class MatchesHelper
  {
    public static bool MatchesByText<TObject>(
      TObject obj,
      string text,
      params Expression<Func<TObject, object>>[] includedProperties) where TObject : class => !string.IsNullOrWhiteSpace(text) && HasMatches(obj, text, includedProperties);

    private static bool HasMatches<TObject>(
      TObject obj,
      string text,
      params LambdaExpression[] includedProperties) where TObject : notnull
    {
      Type objType = obj.GetType();
      PropertyInfo[] properties = objType.GetProperties();
      foreach (PropertyInfo property in properties)
      {
        object? propertyValue = property.GetValue(obj, null);
        if (propertyValue == null)
          continue;
        bool isIncluded = includedProperties.IsPropertyIncluded(property);
        Type propertyType = property.PropertyType;
        if (propertyType.IsUserDefinedObject())
          return HasMatches(propertyValue, text, IncludedInternalProperties(propertyValue, includedProperties));
        if (!isIncluded || propertyType != typeof(string) && !propertyType.IsValueType)
          continue;
        string propertyValueText = propertyValue?.ToString() ?? string.Empty;
        bool hasMatches = propertyValueText.Contains(text, StringComparison.OrdinalIgnoreCase);
        if (hasMatches)
          return true;
      }

      return false;
    }

    private static LambdaExpression[] IncludedInternalProperties<TObject>(
      TObject objectValue,
      params LambdaExpression[] includedProperties) where TObject : notnull
    {
      PropertyInfo[] internalProperties = objectValue.GetType().GetProperties();
      var includedInternalProperties = includedProperties.Where(expression => internalProperties
        .Any(internalProperty => expression.GetProperty()?.Equals(internalProperty) ?? false))
        .ToArray();

      return includedInternalProperties;
    }
  }
}
