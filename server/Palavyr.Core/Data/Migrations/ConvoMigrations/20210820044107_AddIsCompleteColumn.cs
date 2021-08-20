using Microsoft.EntityFrameworkCore.Migrations;

namespace Palavyr.Core.Data.Migrations.ConvoMigrations
{
    public partial class AddIsCompleteColumn : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsComplete",
                table: "ConversationRecords",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "Locale",
                table: "ConversationRecords",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsComplete",
                table: "ConversationRecords");

            migrationBuilder.DropColumn(
                name: "Locale",
                table: "ConversationRecords");
        }
    }
}
