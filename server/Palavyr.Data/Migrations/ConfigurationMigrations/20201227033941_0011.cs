using Microsoft.EntityFrameworkCore.Migrations;

namespace Palavyr.Data.Migrations.ConfigurationMigrations
{
    public partial class _0011 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "Range",
                table: "PercentOfThresholds",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Range",
                table: "PercentOfThresholds");
        }
    }
}
