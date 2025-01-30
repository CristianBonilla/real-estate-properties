using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RealEstateProperties.Contracts.SeedData;
using RealEstateProperties.Domain.Entities.Auth;

namespace RealEstateProperties.Infrastructure.Contexts.RealEstateProperties.Config
{
  class UserConfig(ISeedData? seedData = null) : IEntityTypeConfiguration<UserEntity>
  {
    public void Configure(EntityTypeBuilder<UserEntity> builder)
    {
      builder.ToTable("User", "dbo")
        .HasKey(key => key.UserId);
      builder.Property(property => property.UserId)
        .HasDefaultValueSql("NEWID()");
      builder.Property(property => property.DocumentNumber)
        .HasMaxLength(20)
        .IsUnicode(false)
        .IsRequired();
      builder.Property(property => property.Mobile)
        .HasMaxLength(20)
        .IsUnicode(false)
        .IsRequired();
      builder.Property(property => property.Username)
        .HasMaxLength(100)
        .IsUnicode(false)
        .IsRequired();
      builder.Property(property => property.Password)
        .HasColumnType("varchar(max)")
        .IsRequired();
      builder.Property(property => property.Email)
        .HasMaxLength(100)
        .IsUnicode(false)
        .IsRequired();
      builder.Property(property => property.Firstname)
        .HasMaxLength(50)
        .IsUnicode(false)
        .IsRequired();
      builder.Property(property => property.Lastname)
        .HasMaxLength(50)
        .IsUnicode(false)
        .IsRequired();
      builder.Property(property => property.IsActive)
        .IsRequired();
      builder.Property(property => property.Salt)
        .IsRequired();
      builder.Property(property => property.Created)
        .HasDefaultValueSql("GETUTCDATE()");
      builder.Property(property => property.Version)
        .IsRowVersion();
      builder.HasIndex(index => new { index.DocumentNumber, index.Username, index.Email, index.Mobile })
        .IsUnique();
      if (seedData is not null)
        builder.HasData(seedData.Auth.Users.GetAll());
    }
  }
}
