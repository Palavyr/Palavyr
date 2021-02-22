using Microsoft.EntityFrameworkCore.Migrations;

namespace Palavyr.Data.Migrations.ConfigurationMigrations
{
    public partial class AddWidgetPrefsForFrontend : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "FontFamily",
                table: "WidgetPreferences",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Header",
                table: "WidgetPreferences",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "HeaderColor",
                table: "WidgetPreferences",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SelectListColor",
                table: "WidgetPreferences",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FontFamily",
                table: "WidgetPreferences");

            migrationBuilder.DropColumn(
                name: "Header",
                table: "WidgetPreferences");

            migrationBuilder.DropColumn(
                name: "HeaderColor",
                table: "WidgetPreferences");

            migrationBuilder.DropColumn(
                name: "SelectListColor",
                table: "WidgetPreferences");
        }
    }
}
