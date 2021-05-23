using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace Palavyr.Core.Data.Migrations.ConfigurationMigrations
{
    public partial class addImagesTableAndUpdateConvoNodeProps : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ImageS3Key",
                table: "ConversationNodes");

            migrationBuilder.AddColumn<string>(
                name: "ImageId",
                table: "ConversationNodes",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Images",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ImageId = table.Column<string>(nullable: true),
                    SafeName = table.Column<string>(nullable: true),
                    RiskyName = table.Column<string>(nullable: true),
                    AccountId = table.Column<string>(nullable: true),
                    S3Key = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Images", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Images");

            migrationBuilder.DropColumn(
                name: "ImageId",
                table: "ConversationNodes");

            migrationBuilder.AddColumn<string>(
                name: "ImageS3Key",
                table: "ConversationNodes",
                type: "text",
                nullable: false,
                defaultValue: "");
        }
    }
}
