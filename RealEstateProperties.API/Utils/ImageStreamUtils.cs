using System.IO.Compression;
using RealEstateProperties.Domain.Entities;

namespace RealEstateProperties.API.Utils
{
  using ImageInfo = (byte[] imageBytes, string contentType, string imageName);

  class ImageStreamUtils
  {
    public static async Task<byte[]> GetImageBytes(IFormFile image)
    {
      using MemoryStream memoryStream = new();
      await image.CopyToAsync(memoryStream);
      byte[] imageBytes = memoryStream.ToArray();

      return imageBytes;
    }

    public static async Task<ImageInfo?> GetImagesBytes(string propertyName, IEnumerable<PropertyImageEntity> propertyImages)
      => propertyImages.Count() switch
      {
        0 => null,
        1 => GetBytesFromSingleImage(propertyName, propertyImages),
        _ => await GetZipBytesFromImages(propertyName, propertyImages)
      };

    private static ImageInfo GetBytesFromSingleImage(string propertyName, IEnumerable<PropertyImageEntity> propertyImages)
    {
      PropertyImageEntity propertyImage = propertyImages.Single();

      return (propertyImage.Image, "application/octet-stream", $"{propertyName} {propertyImage.ImageName}");
    }

    private static async Task<ImageInfo> GetZipBytesFromImages(string propertyName, IEnumerable<PropertyImageEntity> propertyImages)
    {
      using MemoryStream memoryStream = new();
      using (ZipArchive zip = new(memoryStream, ZipArchiveMode.Create, true))
      {
        foreach (PropertyImageEntity propertyImage in propertyImages)
        {
          ZipArchiveEntry entry = zip.CreateEntry(propertyImage.ImageName, CompressionLevel.Fastest);
          using Stream stream = entry.Open();
          await stream.WriteAsync(propertyImage.Image.AsMemory(0, propertyImage.Image.Length));
        }
      }
      memoryStream.Seek(0, SeekOrigin.Begin);
      byte[] zipBytes = memoryStream.ToArray();
      string date = DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss tt");
      string zipName = $"{propertyName} {date}.zip";

      return (zipBytes, "application/zip", zipName);
    }
  }
}
