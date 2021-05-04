using Microsoft.EntityFrameworkCore.Migrations;

namespace Palavyr.Core.Data.Migrations.ConfigurationMigrations
{
    public partial class AddButtonWidgetPrefs : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ButtonColor",
                table: "WidgetPreferences",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ButtonFontColor",
                table: "WidgetPreferences",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ButtonColor",
                table: "WidgetPreferences");

            migrationBuilder.DropColumn(
                name: "ButtonFontColor",
                table: "WidgetPreferences");
        }
    }
}
