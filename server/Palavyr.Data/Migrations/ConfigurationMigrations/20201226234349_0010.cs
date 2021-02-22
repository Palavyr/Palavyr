using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace Palavyr.Data.Migrations.ConfigurationMigrations
{
    public partial class _0010 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TableTag",
                table: "SelectOneFlats");

            migrationBuilder.CreateTable(
                name: "PercentOfThresholds",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    AccountId = table.Column<string>(nullable: true),
                    AreaIdentifier = table.Column<string>(nullable: true),
                    TableId = table.Column<string>(nullable: true),
                    ItemId = table.Column<string>(nullable: true),
                    ItemName = table.Column<string>(nullable: true),
                    RowId = table.Column<string>(nullable: true),
                    Threshold = table.Column<double>(nullable: false),
                    ValueMin = table.Column<double>(nullable: false),
                    ValueMax = table.Column<double>(nullable: false),
                    Modifier = table.Column<double>(nullable: false),
                    PosNeg = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PercentOfThresholds", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PercentOfThresholds");

            migrationBuilder.AddColumn<string>(
                name: "TableTag",
                table: "SelectOneFlats",
                type: "text",
                nullable: true);
        }
    }
}
