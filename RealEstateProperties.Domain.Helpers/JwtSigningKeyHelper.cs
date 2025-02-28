using System.Text;

namespace RealEstateProperties.Domain.Helpers
{
  public record struct JwtSigningKeyHelper
  {
    public static byte[] GetSecretKey(string secretValue)
    {
      string secret = Convert.ToHexStringLower(Encoding.UTF8.GetBytes(secretValue));
      byte[] secretKey = Encoding.UTF8.GetBytes(secret);

      return secretKey;
    }
  }
}
