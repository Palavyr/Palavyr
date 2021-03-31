using Microsoft.EntityFrameworkCore.Migrations;

namespace Palavyr.Core.Data.Migrations.ConfigurationMigrations
{
    public partial class AddPropertiesRequiredForComplexNodeResolution : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsCurrency",
                table: "ConversationNodes",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsDynamicTableNode",
                table: "ConversationNodes",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsMultiOptionEditable",
                table: "ConversationNodes",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "NodeComponentType",
                table: "ConversationNodes",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ResolveOrder",
                table: "ConversationNodes",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsCurrency",
                table: "ConversationNodes");

            migrationBuilder.DropColumn(
                name: "IsDynamicTableNode",
                table: "ConversationNodes");

            migrationBuilder.DropColumn(
                name: "IsMultiOptionEditable",
                table: "ConversationNodes");

            migrationBuilder.DropColumn(
                name: "NodeComponentType",
                table: "ConversationNodes");

            migrationBuilder.DropColumn(
                name: "ResolveOrder",
                table: "ConversationNodes");
        }
    }
}
