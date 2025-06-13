using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CreditManager.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "creditmanager");

            migrationBuilder.CreateTable(
                name: "CreditRequests",
                schema: "creditmanager",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    CustomerId = table.Column<Guid>(type: "uuid", nullable: false),
                    Amount = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: false),
                    CurrencyCode = table.Column<string>(type: "character varying(3)", maxLength: 3, nullable: false),
                    Status = table.Column<int>(type: "integer", nullable: false),
                    Comments = table.Column<string>(type: "character varying(2000)", maxLength: 2000, nullable: true),
                    ApprovalDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    ApprovedBy = table.Column<Guid>(type: "uuid", nullable: true),
                    CreatedById = table.Column<Guid>(type: "uuid", nullable: true),
                    DateCreated = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    LastModifiedBy = table.Column<string>(type: "text", nullable: true),
                    LastModifiedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CreditRequests", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TransAuditHs",
                schema: "creditmanager",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    TableName = table.Column<string>(type: "text", nullable: false),
                    TranType = table.Column<int>(type: "integer", nullable: false),
                    Username = table.Column<string>(type: "text", nullable: false),
                    OsUser = table.Column<string>(type: "text", nullable: true),
                    Machine = table.Column<string>(type: "text", nullable: true),
                    TransDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    PrimKey = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TransAuditHs", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TransAuditEs",
                schema: "creditmanager",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    FieldName = table.Column<string>(type: "text", nullable: false),
                    OldValue = table.Column<string>(type: "text", nullable: true),
                    NewValue = table.Column<string>(type: "text", nullable: true),
                    TransAuditHId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TransAuditEs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TransAuditEs_TransAuditHs_TransAuditHId",
                        column: x => x.TransAuditHId,
                        principalSchema: "creditmanager",
                        principalTable: "TransAuditHs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_TransAuditEs_TransAuditHId",
                schema: "creditmanager",
                table: "TransAuditEs",
                column: "TransAuditHId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CreditRequests",
                schema: "creditmanager");

            migrationBuilder.DropTable(
                name: "TransAuditEs",
                schema: "creditmanager");

            migrationBuilder.DropTable(
                name: "TransAuditHs",
                schema: "creditmanager");
        }
    }
}
