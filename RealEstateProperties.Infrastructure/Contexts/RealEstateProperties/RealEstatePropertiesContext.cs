using Microsoft.EntityFrameworkCore;
using RealEstateProperties.Contracts.SeedData;
using RealEstateProperties.Infrastructure.Contexts.RealEstateProperties.Config;
using RealEstateProperties.Infrastructure.Extensions;

namespace RealEstateProperties.Infrastructure.Contexts.RealEstateProperties
{
  public class RealEstatePropertiesContext(DbContextOptions<RealEstatePropertiesContext> options, ISeedData? seedData) : DbContext(options)
  {
    protected override void OnModelCreating(ModelBuilder builder)
    {
      builder.ApplyEntityTypeConfig(seedData, typeof(UserConfig));
      builder.ApplyEntityTypeConfig(seedData,
        typeof(OwnerConfig),
        typeof(PropertyConfig),
        typeof(PropertyImageConfig),
        typeof(PropertyTraceConfig));
    }
  }
}
