using Microsoft.EntityFrameworkCore.Migrations;

namespace Palavyr.Core.Data.Migrations.ConfigurationMigrations
{
    public partial class RemoveLowTriggerAndRenameHighThresholdTriggerProp : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
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

            migrationBuilder.AddColumn<bool>(
                name: "TriggerFallback",
                table: "PercentOfThresholds",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "TriggerFallback",
                table: "CategoryNestedThresholds",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "TriggerFallback",
                table: "BasicThresholds",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TriggerFallback",
                table: "PercentOfThresholds");

            migrationBuilder.DropColumn(
                name: "TriggerFallback",
                table: "CategoryNestedThresholds");

            migrationBuilder.DropColumn(
                name: "TriggerFallback",
                table: "BasicThresholds");

            migrationBuilder.AddColumn<bool>(
                name: "MaxThreshold",
                table: "PercentOfThresholds",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "MinThreshold",
                table: "PercentOfThresholds",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "MaxThreshold",
                table: "CategoryNestedThresholds",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "MinThreshold",
                table: "CategoryNestedThresholds",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "MaxThreshold",
                table: "BasicThresholds",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "MinThreshold",
                table: "BasicThresholds",
                type: "boolean",
                nullable: false,
                defaultValue: false);
        }
    }
}
