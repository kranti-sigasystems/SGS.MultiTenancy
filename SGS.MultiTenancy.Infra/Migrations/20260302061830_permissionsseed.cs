using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SGS.MultiTenancy.Infra.Migrations
{
    /// <inheritdoc />
    public partial class permissionsseed : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            var now = new DateTime(2026, 03, 02, 0, 0, 0, DateTimeKind.Utc);

            migrationBuilder.InsertData(
                table: "Permissions",
                columns: new[]
                {
                "ID",
                "Code",
                "Description",
                "TenantID",
                "CreateBy",
                "CreateOn",
                "LastUpdateBy",
                "LastUpdateOn"
                },
                values: new object[,]
                {
                {
                    new Guid("da91b773-f7b0-4661-894e-0fab341471f5"),
                    "roles.read",
                    "Read roles",
                    null,
                    "SYSTEM",
                    now,
                    null,
                    null
                },
                {
                    new Guid("d5493506-179e-4db0-b1e7-0d8a5eb0bfce"),
                    "roles.create",
                    "Create roles",
                    null,
                    "SYSTEM",
                    now,
                    null,
                    null
                },
                {
                    new Guid("cff33d5b-6182-4a6d-b7bd-f9ab7632732e"),
                    "roles.update",
                    "Update roles",
                    null,
                    "SYSTEM",
                    now,
                    null,
                    null
                },
                {
                    new Guid("0f7b57a8-56b4-40ed-9d34-259159c7924b"),
                    "roles.delete",
                    "Delete roles",
                    null,
                    "SYSTEM",
                    now,
                    null,
                    null
                },
                {
                    new Guid("563ab9bf-3d2c-40b5-a97d-33db51964f66"),
                    "users.assignRole",
                    "Assign role to user",
                    null,
                    "SYSTEM",
                    now,
                    null,
                    null
                },
                {
                    new Guid("b9ae1aa6-8d1a-4fa4-b03a-8a6482e16894"),
                    "roles.assignPermissions",
                    "Assign permissions to role",
                    null,
                    "SYSTEM",
                    now,
                    null,
                    null
                }
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData("Permissions", "ID",
                new Guid("da91b773-f7b0-4661-894e-0fab341471f5"));

            migrationBuilder.DeleteData("Permissions", "ID",
                new Guid("d5493506-179e-4db0-b1e7-0d8a5eb0bfce"));

            migrationBuilder.DeleteData("Permissions", "ID",
                new Guid("cff33d5b-6182-4a6d-b7bd-f9ab7632732e"));

            migrationBuilder.DeleteData("Permissions", "ID",
                new Guid("0f7b57a8-56b4-40ed-9d34-259159c7924b"));

            migrationBuilder.DeleteData("Permissions", "ID",
                new Guid("563ab9bf-3d2c-40b5-a97d-33db51964f66"));

            migrationBuilder.DeleteData("Permissions", "ID",
                new Guid("b9ae1aa6-8d1a-4fa4-b03a-8a6482e16894"));
        }
    }
}