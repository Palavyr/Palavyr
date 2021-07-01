using Microsoft.EntityFrameworkCore.Migrations;

namespace Palavyr.Core.Data.Migrations.ConfigurationMigrations
{
    public partial class RemoveDeprecatedFieldsFromConversationNode : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Fallback",
                table: "ConversationNodes");

            migrationBuilder.DropColumn(
                name: "IsSplitMergeMergePoint",
                table: "ConversationNodes");

            migrationBuilder.DropColumn(
                name: "IsSplitMergeType",
                table: "ConversationNodes");

            migrationBuilder.AddColumn<bool>(
                name: "IsLoopbackAnchorType",
                table: "ConversationNodes",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsLoopbackAnchorType",
                table: "ConversationNodes");

            migrationBuilder.AddColumn<bool>(
                name: "Fallback",
                table: "ConversationNodes",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsSplitMergeMergePoint",
                table: "ConversationNodes",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsSplitMergeType",
                table: "ConversationNodes",
                type: "boolean",
                nullable: false,
                defaultValue: false);
        }
    }
}
