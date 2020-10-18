using Microsoft.EntityFrameworkCore.Migrations;

namespace DashboardServer.Data.Migrations.ConfigurationMigrations
{
    public partial class AddMoreWidgetPrefs : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "HeaderFontColor",
                table: "WidgetPreferences",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ListFontColor",
                table: "WidgetPreferences",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "HeaderFontColor",
                table: "WidgetPreferences");

            migrationBuilder.DropColumn(
                name: "ListFontColor",
                table: "WidgetPreferences");
        }
    }
}
