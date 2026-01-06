using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SGS.MultiTenancy.Infra.Migrations
{
    /// <inheritdoc />
    public partial class SeedSupperAdmin : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {

            DateTime now = DateTime.Now;

            migrationBuilder.InsertData(
                table: "Roles",
                columns: new[]
                {
                "ID", "Name", "Description",
                "CreateBy", "CreateOn", "UpdateBy", "UpdateOn"
                },
                values: new object[]
                {
               Guid.Parse("AE52E52B-0451-4229-AF95-98B332386AB4"),
                "SGS_SuperHost",
                "System level administrator",
                Guid.Empty,
                now,
                null,
                null
                });


            migrationBuilder.InsertData(
                table: "Permissions",
                columns: new[]
                {
                "ID", "Code", "Description",
                "CreateBy", "CreateOn", "UpdateBy", "UpdateOn"
                },
                values: new object[,]
                {
                { Guid.Parse("58F4A77F-1FC7-44EC-8DA4-90ECA1CF90C4"), "TENANT_CREATE", "Create tenant", Guid.Empty, now, null, null },
                { Guid.Parse("1E6E0767-513A-4A9A-A4C1-6E2F11473B5C"),   "TENANT_VIEW",   "View tenant",   Guid.Empty, now, null, null },
                { Guid.Parse("0782CE83-9579-4D68-BB43-DCB19BEEBBC7"),   "TENANT_EDIT",   "Edit tenant",   Guid.Empty, now, null, null },
                { Guid.Parse("FB171982-7B8C-4B21-8F24-3BD3704A47A7"), "TENANT_DELETE", "Delete tenant", Guid.Empty, now, null, null }
                });


            migrationBuilder.InsertData(
                table: "RolePermissions",
                columns: new[]
                {
                "ID", "RoleID", "PermissionID", "TenantID",
                "CreateBy", "CreateOn", "UpdateBy", "UpdateOn"
                },
                values: new object[,]
                {
                { Guid.NewGuid(), Guid.Parse("AE52E52B-0451-4229-AF95-98B332386AB4"), Guid.Parse("58F4A77F-1FC7-44EC-8DA4-90ECA1CF90C4"), Guid.Empty, Guid.Empty, now, null, null },
                { Guid.NewGuid(), Guid.Parse("AE52E52B-0451-4229-AF95-98B332386AB4"), Guid.Parse("1E6E0767-513A-4A9A-A4C1-6E2F11473B5C"),   Guid.Empty, Guid.Empty, now, null, null },
                { Guid.NewGuid(), Guid.Parse("AE52E52B-0451-4229-AF95-98B332386AB4"), Guid.Parse("0782CE83-9579-4D68-BB43-DCB19BEEBBC7"),   Guid.Empty, Guid.Empty, now, null, null },
                { Guid.NewGuid(), Guid.Parse("AE52E52B-0451-4229-AF95-98B332386AB4"), Guid.Parse("FB171982-7B8C-4B21-8F24-3BD3704A47A7"), Guid.Empty, Guid.Empty, now, null, null }
                });


            migrationBuilder.InsertData(
                table: "Users",
                columns: new[]
                {
                "ID", "TenantID", "Name", "Email", "PhoneNumber",
                "Password", "IsEmailConfirmed", "IsMobileConfirmed",
                "CreateBy", "CreateOn", "UpdateBy", "UpdateOn"
                },
                values: new object[]
                {
                Guid.Parse("F63E8CE2-204A-4CEE-8979-4F833CE4F70D"),
                Guid.Empty,
                "SGS_SupperAdmin",
                "superadmin@sgs.com",
                "9999999999",
                "SGS!@#$",
                true,
                true,
                Guid.Empty,
                now,
                null,
                null
                });


            migrationBuilder.InsertData(
                table: "UserRoles",
                columns: new[]
                {
                "ID", "UserID", "RoleID", "TenantID",
                "CreateBy", "CreateOn", "UpdateBy", "UpdateOn"
                },
                values: new object[]
                {
                Guid.NewGuid(),
                Guid.Parse("F63E8CE2-204A-4CEE-8979-4F833CE4F70D"),
                Guid.Parse("AE52E52B-0451-4229-AF95-98B332386AB4"),
                Guid.Empty,
                Guid.Empty,
                now,
                null,
                null
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "UserRoles",
                keyColumn: "UserID",
                keyValue: Guid.Parse("F63E8CE2-204A-4CEE-8979-4F833CE4F70D"));

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "ID",
                keyValue: Guid.Parse("F63E8CE2-204A-4CEE-8979-4F833CE4F70D"));

            migrationBuilder.DeleteData(
                table: "RolePermissions",
                keyColumn: "RoleID",
                keyValue: Guid.Parse("AE52E52B-0451-4229-AF95-98B332386AB4"));

            migrationBuilder.DeleteData(
                table: "Permissions",
                keyColumn: "Code",
                keyValues: new object[]
                {
                "TENANT_CREATE",
                "TENANT_VIEW",
                "TENANT_EDIT",
                "TENANT_DELETE"
                });

            migrationBuilder.DeleteData(
                table: "Roles",
                keyColumn: "ID",
                keyValue: Guid.Parse("AE52E52B-0451-4229-AF95-98B332386AB4"));
        }
    }
}
