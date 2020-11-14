﻿using Microsoft.EntityFrameworkCore.Migrations;

namespace DashboardServer.Data.Migrations.AccountsMigrations
{
    public partial class AddStripeCustomerIdColumn : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "StripeCustomerId",
                table: "Accounts",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "StripeCustomerId",
                table: "Accounts");
        }
    }
}