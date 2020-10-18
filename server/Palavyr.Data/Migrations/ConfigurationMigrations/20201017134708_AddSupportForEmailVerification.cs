using Microsoft.EntityFrameworkCore.Migrations;

namespace DashboardServer.Data.Migrations.ConfigurationMigrations
{
    public partial class AddSupportForEmailVerification : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "AreaSpecificEmail",
                table: "Areas",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "EmailIsVerified",
                table: "Areas",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AreaSpecificEmail",
                table: "Areas");

            migrationBuilder.DropColumn(
                name: "EmailIsVerified",
                table: "Areas");
        }
    }
}
