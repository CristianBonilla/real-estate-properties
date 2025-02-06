using System.Net;

namespace RealEstateProperties.Contracts.DTO
{
  public class ServiceError(HttpStatusCode status, params string[] errors)
  {
    public HttpStatusCode Status { get; } = status;

    public int StatusCode { get; } = (int)status;

    public ICollection<string> Errors { get; } = new HashSet<string>(errors);
  }
}
