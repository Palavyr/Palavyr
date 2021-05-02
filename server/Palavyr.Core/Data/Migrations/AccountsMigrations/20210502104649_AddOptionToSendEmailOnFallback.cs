using Microsoft.EntityFrameworkCore.Migrations;

namespace Palavyr.Core.Data.Migrations.AccountsMigrations
{
    public partial class AddOptionToSendEmailOnFallback : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "LatestUserDataBackup",
                table: "Backups");

            migrationBuilder.AlterColumn<string>(
                name: "EmailAddress",
                table: "Accounts",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "ApiKey",
                table: "Accounts",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "AccountId",
                table: "Accounts",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "LatestUserDataBackup",
                table: "Backups",
                type: "text",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "EmailAddress",
                table: "Accounts",
                type: "text",
                nullable: true,
                oldClrType: typeof(string));

            migrationBuilder.AlterColumn<string>(
                name: "ApiKey",
                table: "Accounts",
                type: "text",
                nullable: true,
                oldClrType: typeof(string));

            migrationBuilder.AlterColumn<string>(
                name: "AccountId",
                table: "Accounts",
                type: "text",
                nullable: true,
                oldClrType: typeof(string));
        }
    }
}
