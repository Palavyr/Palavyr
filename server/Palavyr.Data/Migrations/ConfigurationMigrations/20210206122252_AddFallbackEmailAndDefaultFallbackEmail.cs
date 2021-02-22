using Microsoft.EntityFrameworkCore.Migrations;

namespace Palavyr.Data.Migrations.ConfigurationMigrations
{
    public partial class AddFallbackEmailAndDefaultFallbackEmail : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "FallbackEmailTemplate",
                table: "Areas",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "FallbackSubject",
                table: "Areas",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "UseAreaFallbackEmail",
                table: "Areas",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FallbackEmailTemplate",
                table: "Areas");

            migrationBuilder.DropColumn(
                name: "FallbackSubject",
                table: "Areas");

            migrationBuilder.DropColumn(
                name: "UseAreaFallbackEmail",
                table: "Areas");
        }
    }
}
