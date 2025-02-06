using System.Linq.Expressions;
using System.Reflection;

namespace RealEstateProperties.Domain.Helpers.Extensions
{
  static class PropertyFromExpressionExtensions
  {
    public static PropertyInfo? GetProperty(this LambdaExpression expression)
    {
      PropertyInfo? property = null;
      if (expression.Body is MemberExpression body)
        property = (PropertyInfo)body.Member;
      else if (expression.Body is UnaryExpression unary && unary.Operand is MemberExpression operand)
        property = (PropertyInfo)operand.Member;

      return property;
    }

    public static bool IsPropertyIncluded(this IEnumerable<LambdaExpression> expressions, PropertyInfo property)
      => expressions.Any(expression => property.Equals(expression.GetProperty()));
  }
}
