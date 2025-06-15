using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CreditManager.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class UpdatedModel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CreditType",
                schema: "creditmanager",
                table: "CreditRequests",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<TimeSpan>(
                name: "Period",
                schema: "creditmanager",
                table: "CreditRequests",
                type: "interval",
                nullable: false,
                defaultValue: new TimeSpan(0, 0, 0, 0, 0));

            migrationBuilder.AddColumn<DateTime>(
                name: "RequestDate",
                schema: "creditmanager",
                table: "CreditRequests",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreditType",
                schema: "creditmanager",
                table: "CreditRequests");

            migrationBuilder.DropColumn(
                name: "Period",
                schema: "creditmanager",
                table: "CreditRequests");

            migrationBuilder.DropColumn(
                name: "RequestDate",
                schema: "creditmanager",
                table: "CreditRequests");
        }
    }
}
