using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RealEstateProperties.Contracts.SeedData;
using RealEstateProperties.Domain.Entities;

namespace RealEstateProperties.Infrastructure.Contexts.RealEstateProperties.Config
{
  class OwnerConfig(ISeedData? seedData = null) : IEntityTypeConfiguration<OwnerEntity>
  {
    public void Configure(EntityTypeBuilder<OwnerEntity> builder)
    {
      builder.ToTable("Owner", "dbo")
        .HasKey(key => key.OwnerId);
      builder.Property(property => property.OwnerId)
        .HasDefaultValueSql("NEWID()");
      builder.Property(property => property.Name)
        .HasMaxLength(50)
        .IsUnicode(false)
        .IsRequired();
      builder.Property(property => property.Address)
        .HasMaxLength(150)
        .IsUnicode()
        .IsRequired();
      builder.Property(property => property.Photo);
      builder.Property(property => property.PhotoName)
        .HasMaxLength(100)
        .IsUnicode(false);
      builder.Property(property => property.Birthday)
        .IsRequired();
      builder.Property(property => property.Created)
        .HasDefaultValueSql("GETUTCDATE()");
      builder.Property(property => property.Version)
        .IsRowVersion();
      builder.HasMany(many => many.Properties)
        .WithOne(one => one.Owner)
        .HasForeignKey(key => key.OwnerId)
        .OnDelete(DeleteBehavior.Cascade);
      builder.HasIndex(index => index.Name)
        .IsUnique();
      if (seedData is not null)
        builder.HasData(seedData.RealEstateProperties.Owners.GetAll());
    }
  }

  class PropertyConfig(ISeedData? seedData = null) : IEntityTypeConfiguration<PropertyEntity>
  {
    public void Configure(EntityTypeBuilder<PropertyEntity> builder)
    {
      builder.ToTable("Property", "dbo")
        .HasKey(key => key.PropertyId);
      builder.Property(property => property.PropertyId)
        .HasDefaultValueSql("NEWID()");
      builder.Property(property => property.Name)
        .HasMaxLength(50)
        .IsUnicode(false)
        .IsRequired();
      builder.Property(property => property.Address)
        .HasMaxLength(150)
        .IsUnicode()
        .IsRequired();
      builder.Property(property => property.Price)
        .HasPrecision(14, 2)
        .IsRequired();
      builder.Property(property => property.CodeInternal)
        .IsRequired();
      builder.Property(property => property.Year)
        .IsRequired();
      builder.Property(property => property.Created)
        .HasDefaultValueSql("GETUTCDATE()");
      builder.Property(property => property.Version)
        .IsRowVersion();
      builder.HasOne(one => one.Owner)
        .WithMany(many => many.Properties)
        .HasForeignKey(key => key.OwnerId);
      builder.HasMany(many => many.Images)
        .WithOne(one => one.Property)
        .HasForeignKey(key => key.PropertyId)
        .OnDelete(DeleteBehavior.Cascade);
      builder.HasMany(many => many.Traces)
        .WithOne(one => one.Property)
        .HasForeignKey(key => key.PropertyId)
        .OnDelete(DeleteBehavior.Cascade);
      builder.HasIndex(index => new { index.Name, index.CodeInternal })
        .IsUnique();
      if (seedData is not null)
        builder.HasData(seedData.RealEstateProperties.Properties.GetAll());
    }
  }

  class PropertyImageConfig(ISeedData? seedData = null) : IEntityTypeConfiguration<PropertyImageEntity>
  {
    public void Configure(EntityTypeBuilder<PropertyImageEntity> builder)
    {
      builder.ToTable("PropertyImage", "dbo")
        .HasKey(key => key.PropertyImageId);
      builder.Property(property => property.PropertyImageId)
        .HasDefaultValueSql("NEWID()");
      builder.Property(property => property.Image)
        .IsRequired();
      builder.Property(property => property.ImageName)
        .HasMaxLength(100)
        .IsRequired();
      builder.Property(property => property.Enabled)
        .IsRequired();
      builder.Property(property => property.Created)
        .HasDefaultValueSql("GETUTCDATE()");
      builder.Property(property => property.Version)
        .IsRowVersion();
      builder.HasOne(one => one.Property)
        .WithMany(many => many.Images)
        .HasForeignKey(key => key.PropertyId);
      builder.HasIndex(index => index.ImageName)
        .IsUnique();
      if (seedData is not null)
        builder.HasData(seedData.RealEstateProperties.PropertyImages.GetAll());
    }
  }

  class PropertyTraceConfig(ISeedData? seedData = null) : IEntityTypeConfiguration<PropertyTraceEntity>
  {
    public void Configure(EntityTypeBuilder<PropertyTraceEntity> builder)
    {
      builder.ToTable("PropertyTrace", "dbo")
        .HasKey(key => key.PropertyTraceId);
      builder.Property(property => property.PropertyTraceId)
        .HasDefaultValueSql("NEWID()");
      builder.Property(property => property.Name)
        .HasMaxLength(50)
        .IsUnicode(false)
        .IsRequired();
      builder.Property(property => property.Value)
        .HasPrecision(14, 2)
        .IsRequired();
      builder.Property(property => property.Tax)
        .HasPrecision(14, 2)
        .IsRequired();
      builder.Property(property => property.DateSale)
        .IsRequired();
      builder.Property(property => property.Created)
        .HasDefaultValueSql("GETUTCDATE()");
      builder.Property(property => property.Version)
        .IsRowVersion();
      builder.HasOne(one => one.Property)
        .WithMany(many => many.Traces)
        .HasForeignKey(key => key.PropertyId);
      builder.HasIndex(index => index.Name)
        .IsUnique();
      if (seedData is not null)
        builder.HasData(seedData.RealEstateProperties.PropertyTraces.GetAll());
    }
  }
}
