namespace RealEstateProperties.API.Utils
{
  class PropertyImageStreamUtil
  {
    public static async Task<byte[]> GetImageBytes(IFormFile image)
    {
      using MemoryStream memoryStream = new();
      await image.CopyToAsync(memoryStream);
      byte[] imageBytes = memoryStream.ToArray();

      return imageBytes;
    }
  }
}
