using Microsoft.EntityFrameworkCore.Migrations;

namespace Palavyr.Data.Migrations.ConfigurationMigrations
{
    public partial class AddIsSplitMergeTypeToConveNode : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsSplitMergeType",
                table: "ConversationNodes",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsSplitMergeType",
                table: "ConversationNodes");
        }
    }
}
