using Microsoft.EntityFrameworkCore.Migrations;

namespace Palavyr.Data.Migrations.AccountsMigrations
{
    public partial class RemoveAndRenameUpdates : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UserName",
                table: "Accounts");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "UserName",
                table: "Accounts",
                type: "text",
                nullable: true);
        }
    }
}
