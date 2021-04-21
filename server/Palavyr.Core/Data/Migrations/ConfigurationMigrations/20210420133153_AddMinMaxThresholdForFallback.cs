using Microsoft.EntityFrameworkCore.Migrations;

namespace Palavyr.Core.Data.Migrations.ConfigurationMigrations
{
    public partial class AddMinMaxThresholdForFallback : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "MaxThreshold",
                table: "PercentOfThresholds",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "MinThreshold",
                table: "PercentOfThresholds",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "MaxThreshold",
                table: "CategoryNestedThresholds",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "MinThreshold",
                table: "CategoryNestedThresholds",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "MaxThreshold",
                table: "BasicThresholds",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "MinThreshold",
                table: "BasicThresholds",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "MaxThreshold",
                table: "PercentOfThresholds");

            migrationBuilder.DropColumn(
                name: "MinThreshold",
                table: "PercentOfThresholds");

            migrationBuilder.DropColumn(
                name: "MaxThreshold",
                table: "CategoryNestedThresholds");

            migrationBuilder.DropColumn(
                name: "MinThreshold",
                table: "CategoryNestedThresholds");

            migrationBuilder.DropColumn(
                name: "MaxThreshold",
                table: "BasicThresholds");

            migrationBuilder.DropColumn(
                name: "MinThreshold",
                table: "BasicThresholds");
        }
    }
}
