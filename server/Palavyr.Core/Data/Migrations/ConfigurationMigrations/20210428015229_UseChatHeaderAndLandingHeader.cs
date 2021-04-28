using Microsoft.EntityFrameworkCore.Migrations;

namespace Palavyr.Core.Data.Migrations.ConfigurationMigrations
{
    public partial class UseChatHeaderAndLandingHeader : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Header",
                table: "WidgetPreferences");

            migrationBuilder.DropColumn(
                name: "Subtitle",
                table: "WidgetPreferences");

            migrationBuilder.DropColumn(
                name: "Title",
                table: "WidgetPreferences");

            migrationBuilder.AddColumn<string>(
                name: "ChatHeader",
                table: "WidgetPreferences",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LandingHeader",
                table: "WidgetPreferences",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ChatHeader",
                table: "WidgetPreferences");

            migrationBuilder.DropColumn(
                name: "LandingHeader",
                table: "WidgetPreferences");

            migrationBuilder.AddColumn<string>(
                name: "Header",
                table: "WidgetPreferences",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Subtitle",
                table: "WidgetPreferences",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Title",
                table: "WidgetPreferences",
                type: "text",
                nullable: true);
        }
    }
}
