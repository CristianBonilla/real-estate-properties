namespace RealEstateProperties.Domain.Helpers
{
  public class ConvertionsHelper
  {
    public static byte[] GetStringToBytes(string source, char separator = ',') => Array.ConvertAll(source.Split(separator), byte.Parse);
    public static byte[][] GetStringToBytes(string[] sources, char separator = ',') => [.. sources.Select(source => GetStringToBytes(source, separator))];
  }
}
