using Microsoft.EntityFrameworkCore.Migrations;

namespace Palavyr.Core.Data.Migrations.ConfigurationMigrations
{
    public partial class addimageKeyToConvoNode : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ImageS3Key",
                table: "ConversationNodes",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ImageS3Key",
                table: "ConversationNodes");
        }
    }
}
