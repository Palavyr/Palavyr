using Microsoft.EntityFrameworkCore.Migrations;

namespace Palavyr.Core.Data.Migrations.ConfigurationMigrations
{
    public partial class RemoveGroupedPropFromWidgetPrefs : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ShouldGroup",
                table: "WidgetPreferences");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "ShouldGroup",
                table: "WidgetPreferences",
                type: "boolean",
                nullable: false,
                defaultValue: false);
        }
    }
}
