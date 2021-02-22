using Microsoft.EntityFrameworkCore.Migrations;

namespace Palavyr.Data.Migrations.AccountsMigrations
{
    public partial class AddWebhookPayloadTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "StripeWebHookRecords",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    PayloadSignature = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StripeWebHookRecords", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "StripeWebHookRecords");
        }
    }
}
