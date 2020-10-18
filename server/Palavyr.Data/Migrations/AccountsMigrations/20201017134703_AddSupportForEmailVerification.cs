using Microsoft.EntityFrameworkCore.Migrations;

namespace DashboardServer.Data.Migrations.AccountsMigrations
{
    public partial class AddSupportForEmailVerification : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "DefaultEmailIsVerified",
                table: "Accounts",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DefaultEmailIsVerified",
                table: "Accounts");
        }
    }
}
