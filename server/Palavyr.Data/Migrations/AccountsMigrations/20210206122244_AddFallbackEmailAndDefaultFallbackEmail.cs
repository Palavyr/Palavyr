using Microsoft.EntityFrameworkCore.Migrations;

namespace DashboardServer.Data.Migrations.AccountsMigrations
{
    public partial class AddFallbackEmailAndDefaultFallbackEmail : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "GeneralFallbackEmailTemplate",
                table: "Accounts",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "GeneralFallbackSubject",
                table: "Accounts",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "GeneralFallbackEmailTemplate",
                table: "Accounts");

            migrationBuilder.DropColumn(
                name: "GeneralFallbackSubject",
                table: "Accounts");
        }
    }
}
