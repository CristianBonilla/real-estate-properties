using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RealEstateProperties.Infrastructure.Contexts.RealEstateProperties.Migrations
{
  /// <inheritdoc />
  public partial class ChangedPropertiesPrecision : Migration
  {
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
      migrationBuilder.AlterColumn<decimal>(
          name: "Value",
          schema: "dbo",
          table: "PropertyTrace",
          type: "decimal(14,2)",
          precision: 14,
          scale: 2,
          nullable: false,
          oldClrType: typeof(decimal),
          oldType: "decimal(12,2)",
          oldPrecision: 12,
          oldScale: 2);

      migrationBuilder.AlterColumn<decimal>(
          name: "Tax",
          schema: "dbo",
          table: "PropertyTrace",
          type: "decimal(14,2)",
          precision: 14,
          scale: 2,
          nullable: false,
          oldClrType: typeof(decimal),
          oldType: "decimal(12,2)",
          oldPrecision: 12,
          oldScale: 2);

      migrationBuilder.AlterColumn<decimal>(
          name: "Price",
          schema: "dbo",
          table: "Property",
          type: "decimal(14,2)",
          precision: 14,
          scale: 2,
          nullable: false,
          oldClrType: typeof(decimal),
          oldType: "decimal(12,2)",
          oldPrecision: 12,
          oldScale: 2);
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
      migrationBuilder.AlterColumn<decimal>(
          name: "Value",
          schema: "dbo",
          table: "PropertyTrace",
          type: "decimal(12,2)",
          precision: 12,
          scale: 2,
          nullable: false,
          oldClrType: typeof(decimal),
          oldType: "decimal(14,2)",
          oldPrecision: 14,
          oldScale: 2);

      migrationBuilder.AlterColumn<decimal>(
          name: "Tax",
          schema: "dbo",
          table: "PropertyTrace",
          type: "decimal(12,2)",
          precision: 12,
          scale: 2,
          nullable: false,
          oldClrType: typeof(decimal),
          oldType: "decimal(14,2)",
          oldPrecision: 14,
          oldScale: 2);

      migrationBuilder.AlterColumn<decimal>(
          name: "Price",
          schema: "dbo",
          table: "Property",
          type: "decimal(12,2)",
          precision: 12,
          scale: 2,
          nullable: false,
          oldClrType: typeof(decimal),
          oldType: "decimal(14,2)",
          oldPrecision: 14,
          oldScale: 2);
    }
  }
}
