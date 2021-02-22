using Microsoft.EntityFrameworkCore.Migrations;

namespace Palavyr.Data.Migrations.AccountsMigrations
{
    public partial class AddSubscriptionDetailsToAccountsTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "HasUpgraded",
                table: "Accounts",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "PaymentInterval",
                table: "Accounts",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "PlanType",
                table: "Accounts",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "HasUpgraded",
                table: "Accounts");

            migrationBuilder.DropColumn(
                name: "PaymentInterval",
                table: "Accounts");

            migrationBuilder.DropColumn(
                name: "PlanType",
                table: "Accounts");
        }
    }
}
