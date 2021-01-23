using Microsoft.EntityFrameworkCore.Migrations;

namespace DashboardServer.Data.Migrations.ConfigurationMigrations
{
    public partial class AddbackupTableToRecordLatestBackupS3Keys : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsFromDynamic",
                table: "ConversationNodes");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsFromDynamic",
                table: "ConversationNodes",
                type: "boolean",
                nullable: false,
                defaultValue: false);
        }
    }
}
