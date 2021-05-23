using Microsoft.EntityFrameworkCore.Migrations;

namespace Palavyr.Core.Data.Migrations.ConfigurationMigrations
{
    public partial class updateNullabilityInImage : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "ImageId",
                table: "Images",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "AccountId",
                table: "Images",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "ImageId",
                table: "Images",
                type: "text",
                nullable: true,
                oldClrType: typeof(string));

            migrationBuilder.AlterColumn<string>(
                name: "AccountId",
                table: "Images",
                type: "text",
                nullable: true,
                oldClrType: typeof(string));
        }
    }
}
