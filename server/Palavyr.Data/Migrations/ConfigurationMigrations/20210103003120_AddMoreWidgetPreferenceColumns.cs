using Microsoft.EntityFrameworkCore.Migrations;

namespace DashboardServer.Data.Migrations.ConfigurationMigrations
{
    public partial class AddMoreWidgetPreferenceColumns : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ChatBubbleColor",
                table: "WidgetPreferences",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ChatFontColor",
                table: "WidgetPreferences",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "OptionsHeaderColor",
                table: "WidgetPreferences",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "OptionsHeaderFontColor",
                table: "WidgetPreferences",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ChatBubbleColor",
                table: "WidgetPreferences");

            migrationBuilder.DropColumn(
                name: "ChatFontColor",
                table: "WidgetPreferences");

            migrationBuilder.DropColumn(
                name: "OptionsHeaderColor",
                table: "WidgetPreferences");

            migrationBuilder.DropColumn(
                name: "OptionsHeaderFontColor",
                table: "WidgetPreferences");
        }
    }
}
