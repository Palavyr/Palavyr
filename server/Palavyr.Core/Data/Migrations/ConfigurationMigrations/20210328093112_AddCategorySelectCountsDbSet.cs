using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace Palavyr.Core.Data.Migrations.ConfigurationMigrations
{
    public partial class AddCategorySelectCountsDbSet : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CategorySelectCounts",
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
                    ItemName = table.Column<string>(nullable: true),
                    ItemId = table.Column<string>(nullable: true),
                    ItemOrder = table.Column<int>(nullable: false),
                    Count = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CategorySelectCounts", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CategorySelectCounts");
        }
    }
}
