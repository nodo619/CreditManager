using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CreditManager.Identity.Migrations
{
    /// <inheritdoc />
    public partial class Updatedviewsaddedpasswords : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                schema: "identity",
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("157e730f-df20-4c5d-be84-22dd80c46ef4"),
                columns: new[] { "ConcurrencyStamp", "Email", "EmailConfirmed", "NormalizedEmail", "PasswordHash", "SecurityStamp" },
                values: new object[] { "2eca2b23-210c-4e5e-b308-b266300213f2", "n.chapsas@example.com", true, "N.CHAPSAS@EXAMPLE.COM", "AQAAAAIAAYagAAAAEEAeE6qye0g1afpQA3NVq+hyj1qIQeG7AaM7nAkCYzU+YGRAd7Cc8bMct0ZzgHrxvg==", "9ed96c02-af10-4af2-a14a-d61d124807fa" });

            migrationBuilder.UpdateData(
                schema: "identity",
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("157e730f-df20-4c5d-be84-22dd80c46efe"),
                columns: new[] { "ConcurrencyStamp", "Email", "EmailConfirmed", "NormalizedEmail", "PasswordHash", "SecurityStamp" },
                values: new object[] { "728fd00e-5663-43dc-a67e-1ad8b5dd9d86", "m.schwarzmuller@example.com", true, "M.SCHWARZMULLER@EXAMPLE.COM", "AQAAAAIAAYagAAAAEP6eWqmsz+IvoiSzBfeDljZXaKoVCsj8Au+8ir9A5/2mPhiPr7Dgs25GnQj2EEwZCQ==", "f9a324fe-810b-4cd8-adef-bb2d63999e2f" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                schema: "identity",
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("157e730f-df20-4c5d-be84-22dd80c46ef4"),
                columns: new[] { "ConcurrencyStamp", "Email", "EmailConfirmed", "NormalizedEmail", "PasswordHash", "SecurityStamp" },
                values: new object[] { "49112324-f5b8-4d23-8d4b-f978ff25e996", null, false, null, null, null });

            migrationBuilder.UpdateData(
                schema: "identity",
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("157e730f-df20-4c5d-be84-22dd80c46efe"),
                columns: new[] { "ConcurrencyStamp", "Email", "EmailConfirmed", "NormalizedEmail", "PasswordHash", "SecurityStamp" },
                values: new object[] { "b480cebf-a4d6-4d31-8e3d-85a2582c5825", null, false, null, null, null });
        }
    }
}
