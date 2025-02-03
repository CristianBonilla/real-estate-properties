namespace RealEstateProperties.Domain.Helpers
{
  public class StringCommonHelper
  {
    public static bool IsStringEquivalent(string sourceA, string sourceB, StringComparison comparison)
      => string.Compare(sourceA, sourceB, comparison) == 0;

    public static bool IsStringEquivalent(string sourceA, string sourceB)
      => IsStringEquivalent(sourceA, sourceB, StringComparison.OrdinalIgnoreCase);
  }
}
