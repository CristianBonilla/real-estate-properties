using System.Reflection;
using Microsoft.EntityFrameworkCore;
using RealEstateProperties.Contracts.SeedData;

namespace RealEstateProperties.Infrastructure.Extensions
{
  static class EntityTypeConfigExtensions
  {
    static readonly Type _configType = typeof(IEntityTypeConfiguration<>);
    static readonly Type _seedDataType = typeof(ISeedData);

    public static ModelBuilder ApplyEntityTypeConfig(this ModelBuilder builder, ISeedData? seedData, params Type[] entityTypeConfigs)
    {
      foreach (Type entityTypeConfig in entityTypeConfigs.Distinct())
        ApplyEntityTypeConfig(builder, entityTypeConfig, seedData);

      return builder;
    }

    public static ModelBuilder ApplyEntityTypeConfig<TEntityTypeConfig>(this ModelBuilder builder, ISeedData? seedData) where TEntityTypeConfig : class
      => ApplyEntityTypeConfig(builder, typeof(TEntityTypeConfig), seedData);

    public static ModelBuilder ApplyEntityTypeConfig(this ModelBuilder builder, Type entityTypeConfig, ISeedData? seedData)
    {
      if (!IsEntityTypeConfigImplemented(entityTypeConfig))
        throw new InvalidOperationException($"The type {entityTypeConfig.Name} doesn't implement the interface {_configType.Name}");
      if (entityTypeConfig.GetConstructor([_seedDataType]) is ConstructorInfo constructor)
        ApplyEntityTypeConfig(builder, constructor.Invoke([seedData]));
      else
        ApplyEntityTypeConfig(builder, entityTypeConfig.Assembly);

      return builder;
    }

    private static void ApplyEntityTypeConfig(ModelBuilder builder, dynamic entityTypeConfig) => builder.ApplyConfiguration(entityTypeConfig);

    private static void ApplyEntityTypeConfig(ModelBuilder builder, Assembly entityTypeConfigAssembly) => builder.ApplyConfigurationsFromAssembly(entityTypeConfigAssembly);

    private static bool IsEntityTypeConfigImplemented(Type type)
      => type.GetInterfaces().Any(implements => implements.IsGenericType && implements.GetGenericTypeDefinition() == _configType);
  }
}
