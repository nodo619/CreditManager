using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CreditManager.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class UpdatedCreditRequestModel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Period",
                schema: "creditmanager",
                table: "CreditRequests");

            migrationBuilder.AddColumn<int>(
                name: "PeriodDays",
                schema: "creditmanager",
                table: "CreditRequests",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "PeriodMonths",
                schema: "creditmanager",
                table: "CreditRequests",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "PeriodYears",
                schema: "creditmanager",
                table: "CreditRequests",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PeriodDays",
                schema: "creditmanager",
                table: "CreditRequests");

            migrationBuilder.DropColumn(
                name: "PeriodMonths",
                schema: "creditmanager",
                table: "CreditRequests");

            migrationBuilder.DropColumn(
                name: "PeriodYears",
                schema: "creditmanager",
                table: "CreditRequests");

            migrationBuilder.AddColumn<TimeSpan>(
                name: "Period",
                schema: "creditmanager",
                table: "CreditRequests",
                type: "interval",
                nullable: false,
                defaultValue: new TimeSpan(0, 0, 0, 0, 0));
        }
    }
}
