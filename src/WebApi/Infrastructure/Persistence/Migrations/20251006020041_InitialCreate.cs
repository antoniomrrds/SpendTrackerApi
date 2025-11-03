using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterDatabase().Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder
                .CreateTable(
                    name: "tbl_category",
                    columns: table => new
                    {
                        ID = table.Column<Guid>(
                            type: "char(36)",
                            nullable: false,
                            collation: "ascii_general_ci"
                        ),
                        NAME = table
                            .Column<string>(type: "varchar(150)", maxLength: 150, nullable: false)
                            .Annotation("MySql:CharSet", "utf8mb4"),
                        DESCRIPTION = table
                            .Column<string>(type: "VARCHAR(200)", maxLength: 200, nullable: true)
                            .Annotation("MySql:CharSet", "utf8mb4"),
                    },
                    constraints: table =>
                    {
                        table.PrimaryKey("pk_ID_CATEGORY", x => x.ID);
                    }
                )
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder
                .CreateTable(
                    name: "tbl_expense",
                    columns: table => new
                    {
                        Id = table.Column<Guid>(
                            type: "char(36)",
                            nullable: false,
                            collation: "ascii_general_ci"
                        ),
                        DESCRIPTION = table
                            .Column<string>(type: "VARCHAR(500)", maxLength: 500, nullable: true)
                            .Annotation("MySql:CharSet", "utf8mb4"),
                        AMOUNT = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                        DATE = table.Column<DateTime>(type: "DATETIME", nullable: false),
                        ID_CATEGORY = table.Column<Guid>(
                            type: "char(36)",
                            nullable: false,
                            collation: "ascii_general_ci"
                        ),
                    },
                    constraints: table =>
                    {
                        table.PrimaryKey("pk_ID_EXPENSE", x => x.Id);
                        table.ForeignKey(
                            name: "fk_tbl_expense_tbl_category",
                            column: x => x.ID_CATEGORY,
                            principalTable: "tbl_category",
                            principalColumn: "ID",
                            onDelete: ReferentialAction.Restrict
                        );
                    }
                )
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "uq_tbl_category_NAME",
                table: "tbl_category",
                column: "NAME",
                unique: true
            );

            migrationBuilder.CreateIndex(
                name: "idx_tbl_expense_EXPENSE_DATE",
                table: "tbl_expense",
                column: "DATE"
            );

            migrationBuilder.CreateIndex(
                name: "idx_tbl_expense_ID_CATEGORY",
                table: "tbl_expense",
                column: "ID_CATEGORY"
            );
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(name: "tbl_expense");

            migrationBuilder.DropTable(name: "tbl_category");
        }
    }
}
