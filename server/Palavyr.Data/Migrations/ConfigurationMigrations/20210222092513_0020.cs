using Microsoft.EntityFrameworkCore.Migrations;

namespace Palavyr.Data.Migrations.ConfigurationMigrations
{
    public partial class _0020 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PerPersonInputRequired",
                table: "StaticTablesRows");

            migrationBuilder.DropColumn(
                name: "GroupId",
                table: "Areas");

            migrationBuilder.DropColumn(
                name: "IsComplete",
                table: "Areas");

            migrationBuilder.AddColumn<bool>(
                name: "PerPersonInputRequired",
                table: "StaticTablesMetas",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsEnabled",
                table: "Areas",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PerPersonInputRequired",
                table: "StaticTablesMetas");

            migrationBuilder.DropColumn(
                name: "IsEnabled",
                table: "Areas");

            migrationBuilder.AddColumn<bool>(
                name: "PerPersonInputRequired",
                table: "StaticTablesRows",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "GroupId",
                table: "Areas",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsComplete",
                table: "Areas",
                type: "boolean",
                nullable: false,
                defaultValue: false);
        }
    }
}
