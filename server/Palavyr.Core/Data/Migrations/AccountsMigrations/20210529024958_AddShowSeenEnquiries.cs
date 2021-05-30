using Microsoft.EntityFrameworkCore.Migrations;

namespace Palavyr.Core.Data.Migrations.AccountsMigrations
{
    public partial class AddShowSeenEnquiries : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "ShowSeenEnquiries",
                table: "Accounts",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ShowSeenEnquiries",
                table: "Accounts");
        }
    }
}
