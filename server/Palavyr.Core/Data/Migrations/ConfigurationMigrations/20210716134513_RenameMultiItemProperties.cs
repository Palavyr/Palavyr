using Microsoft.EntityFrameworkCore.Migrations;

namespace Palavyr.Core.Data.Migrations.ConfigurationMigrations
{
    public partial class RenameMultiItemProperties : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Category",
                table: "TwoNestedCategories");

            migrationBuilder.DropColumn(
                name: "SubCategory",
                table: "TwoNestedCategories");

            migrationBuilder.DropColumn(
                name: "Category",
                table: "CategoryNestedThresholds");

            migrationBuilder.AddColumn<string>(
                name: "InnerItemName",
                table: "TwoNestedCategories",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ItemName",
                table: "TwoNestedCategories",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ItemOrder",
                table: "PercentOfThresholds",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "ItemName",
                table: "CategoryNestedThresholds",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "InnerItemName",
                table: "TwoNestedCategories");

            migrationBuilder.DropColumn(
                name: "ItemName",
                table: "TwoNestedCategories");

            migrationBuilder.DropColumn(
                name: "ItemOrder",
                table: "PercentOfThresholds");

            migrationBuilder.DropColumn(
                name: "ItemName",
                table: "CategoryNestedThresholds");

            migrationBuilder.AddColumn<string>(
                name: "Category",
                table: "TwoNestedCategories",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SubCategory",
                table: "TwoNestedCategories",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Category",
                table: "CategoryNestedThresholds",
                type: "text",
                nullable: true);
        }
    }
}
