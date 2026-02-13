using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SGS.MultiTenancy.Infra.Migrations
{
    /// <inheritdoc />
    public partial class UserHostRoleSeeded : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Roles",
                columns: new[]
                {"ID","Name","Description","TenantID","IsDefault", "CreateBy","LastUpdateBy","CreateOn","LastUpdateOn" },
                values: new object[]
                {
            new Guid("3b64e297-1cc8-47a0-bd80-b0bb46cf87f6"),
            "USER_HOST",
            "Default user host role",
            new Guid("00000000-0000-0000-0000-000000000000"),
            false,
            new Guid("00000000-0000-0000-0000-000000000000"),
            null,
            new DateTime(2026, 2, 11),
            new DateTime(2026, 2, 11)
                });
        }
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Roles",
                keyColumn: "ID",
                keyValue: new Guid("3b64e297-1cc8-47a0-bd80-b0bb46cf87f6"));
        }
    }
}
