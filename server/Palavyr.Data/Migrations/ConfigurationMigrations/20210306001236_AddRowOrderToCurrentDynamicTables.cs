using Microsoft.EntityFrameworkCore.Migrations;

namespace Palavyr.Data.Migrations.ConfigurationMigrations
{
    public partial class AddRowOrderToCurrentDynamicTables : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "RowOrder",
                table: "SelectOneFlats",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "RowOrder",
                table: "PercentOfThresholds",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "RowOrder",
                table: "BasicThresholds",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "RowOrder",
                table: "SelectOneFlats");

            migrationBuilder.DropColumn(
                name: "RowOrder",
                table: "PercentOfThresholds");

            migrationBuilder.DropColumn(
                name: "RowOrder",
                table: "BasicThresholds");
        }
    }
}
