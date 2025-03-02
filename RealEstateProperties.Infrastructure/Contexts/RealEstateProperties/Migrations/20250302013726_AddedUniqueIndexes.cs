using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RealEstateProperties.Infrastructure.Contexts.RealEstateProperties.Migrations
{
  /// <inheritdoc />
  public partial class AddedUniqueIndexes : Migration
  {
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
      migrationBuilder.CreateIndex(
          name: "IX_PropertyTrace_Name",
          schema: "dbo",
          table: "PropertyTrace",
          column: "Name",
          unique: true);

      migrationBuilder.CreateIndex(
          name: "IX_PropertyImage_ImageName",
          schema: "dbo",
          table: "PropertyImage",
          column: "ImageName",
          unique: true);

      migrationBuilder.CreateIndex(
          name: "IX_Property_Name_CodeInternal",
          schema: "dbo",
          table: "Property",
          columns: new[] { "Name", "CodeInternal" },
          unique: true);

      migrationBuilder.CreateIndex(
          name: "IX_Owner_Name",
          schema: "dbo",
          table: "Owner",
          column: "Name",
          unique: true);
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
      migrationBuilder.DropIndex(
          name: "IX_PropertyTrace_Name",
          schema: "dbo",
          table: "PropertyTrace");

      migrationBuilder.DropIndex(
          name: "IX_PropertyImage_ImageName",
          schema: "dbo",
          table: "PropertyImage");

      migrationBuilder.DropIndex(
          name: "IX_Property_Name_CodeInternal",
          schema: "dbo",
          table: "Property");

      migrationBuilder.DropIndex(
          name: "IX_Owner_Name",
          schema: "dbo",
          table: "Owner");
    }
  }
}
