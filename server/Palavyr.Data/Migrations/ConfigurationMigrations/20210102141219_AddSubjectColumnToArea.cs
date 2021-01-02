using Microsoft.EntityFrameworkCore.Migrations;

namespace DashboardServer.Data.Migrations.ConfigurationMigrations
{
    public partial class AddSubjectColumnToArea : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Subject",
                table: "Areas",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Subject",
                table: "Areas");
        }
    }
}
