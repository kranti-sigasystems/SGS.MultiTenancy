using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SGS.MultiTenancy.Infra.Migrations
{
    /// <inheritdoc />
    public partial class StackTraceIntroduced : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreateBy",
                table: "AuditLogs");

            migrationBuilder.DropColumn(
                name: "CreateOn",
                table: "AuditLogs");

            migrationBuilder.DropColumn(
                name: "LastUpdateBy",
                table: "AuditLogs");

            migrationBuilder.DropColumn(
                name: "LastUpdateOn",
                table: "AuditLogs");

            migrationBuilder.AddColumn<string>(
                name: "StackTrace",
                table: "AuditLogs",
                type: "longtext",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "StackTrace",
                table: "AuditLogs");

            migrationBuilder.AddColumn<Guid>(
                name: "CreateBy",
                table: "AuditLogs",
                type: "char(36)",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                collation: "ascii_general_ci");

            migrationBuilder.AddColumn<DateTime>(
                name: "CreateOn",
                table: "AuditLogs",
                type: "datetime(6)",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<Guid>(
                name: "LastUpdateBy",
                table: "AuditLogs",
                type: "char(36)",
                nullable: true,
                collation: "ascii_general_ci");

            migrationBuilder.AddColumn<DateTime>(
                name: "LastUpdateOn",
                table: "AuditLogs",
                type: "datetime(6)",
                nullable: true);
        }
    }
}
