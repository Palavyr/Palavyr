using Microsoft.EntityFrameworkCore.Migrations;

namespace Palavyr.Core.Data.Migrations.ConfigurationMigrations
{
    public partial class FixRenderChildrenName : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ShouldRenderChild",
                table: "ConversationNodes");

            migrationBuilder.AddColumn<bool>(
                name: "ShouldRenderChildren",
                table: "ConversationNodes",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ShouldRenderChildren",
                table: "ConversationNodes");

            migrationBuilder.AddColumn<bool>(
                name: "ShouldRenderChild",
                table: "ConversationNodes",
                type: "boolean",
                nullable: false,
                defaultValue: false);
        }
    }
}
