using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace Palavyr.Data.Migrations.ConfigurationMigrations
{
    public partial class RenameToTwoNestedCategory : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CategorySelectCounts");

            migrationBuilder.CreateTable(
                name: "TwoNestedCategories",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    AccountId = table.Column<string>(nullable: true),
                    AreaIdentifier = table.Column<string>(nullable: true),
                    TableId = table.Column<string>(nullable: true),
                    ValueMin = table.Column<double>(nullable: false),
                    ValueMax = table.Column<double>(nullable: false),
                    Range = table.Column<bool>(nullable: false),
                    RowId = table.Column<string>(nullable: true),
                    RowOrder = table.Column<int>(nullable: false),
                    ItemId = table.Column<string>(nullable: true),
                    ItemOrder = table.Column<int>(nullable: false),
                    Category = table.Column<string>(nullable: true),
                    SubCategory = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TwoNestedCategories", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TwoNestedCategories");

            migrationBuilder.CreateTable(
                name: "CategorySelectCounts",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    AccountId = table.Column<string>(type: "text", nullable: true),
                    AreaIdentifier = table.Column<string>(type: "text", nullable: true),
                    Count = table.Column<string>(type: "text", nullable: true),
                    ItemId = table.Column<string>(type: "text", nullable: true),
                    ItemName = table.Column<string>(type: "text", nullable: true),
                    ItemOrder = table.Column<int>(type: "integer", nullable: false),
                    Range = table.Column<bool>(type: "boolean", nullable: false),
                    RowId = table.Column<string>(type: "text", nullable: true),
                    RowOrder = table.Column<int>(type: "integer", nullable: false),
                    TableId = table.Column<string>(type: "text", nullable: true),
                    ValueMax = table.Column<double>(type: "double precision", nullable: false),
                    ValueMin = table.Column<double>(type: "double precision", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CategorySelectCounts", x => x.Id);
                });
        }
    }
}
