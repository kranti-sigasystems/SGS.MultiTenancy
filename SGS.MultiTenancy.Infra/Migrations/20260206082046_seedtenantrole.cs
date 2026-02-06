using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SGS.MultiTenancy.Infra.Migrations
{
    /// <inheritdoc />
    public partial class seedtenantrole : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            var tenantId = Guid.Empty;
            var adminUserId = Guid.Parse("f63e8ce2-204a-4cee-8979-4f833ce4f70d");

            migrationBuilder.InsertData(
                table: "Permissions",
                columns: new[]
                {
                    "ID","Code","Description","TenantID","CreateBy","LastUpdateBy","LastUpdateOn","CreateOn"
                },
                values: new object[,]
                {
                    {
                        Guid.Parse("a096fd03-ec1f-4a34-be90-29426e07534e"),
                        "USER_CREATE",
                        "Create user",
                        tenantId,
                        adminUserId,
                        adminUserId,
                        DateTime.UtcNow,
                        DateTime.UtcNow
                    },
                    {
                        Guid.Parse("f3563926-7a07-4e30-b514-fdc6ba6c465e"),
                        "USER_VIEW",
                        "View user",
                        tenantId,
                        adminUserId,
                        adminUserId,
                        DateTime.UtcNow,
                        DateTime.UtcNow
                    },
                    {
                        Guid.Parse("4ee04083-2cc8-4e89-bf07-27149bc72dd1"),
                        "USER_UPDATE",
                        "Update user",
                        tenantId,
                        adminUserId,
                        adminUserId,
                        DateTime.UtcNow,
                        DateTime.UtcNow
                    },
                    {
                        Guid.Parse("47d2b5bd-d65a-4aab-90c2-7278db5d5b04"),
                        "USER_DELETE",
                        "Delete user",
                        tenantId, 
                        adminUserId, 
                        adminUserId,
                        DateTime.UtcNow, 
                        DateTime.UtcNow
                    },
                }
            );


            migrationBuilder.InsertData(
                  table: "Roles",
                  columns: new[]
                   {
                    "ID",
                    "Name",
                    "Description",
                    "TenantID",
                    "IsDefault",
                    "CreateBy",
                    "LastUpdateBy",
                    "LastUpdateOn",
                    "CreateOn"
                  },
                  values: new object[,]
                  {

                        {
                          Guid.Parse("30dec8bc-2b22-4b3b-b721-8eb28c5d39c9"),
                          "Tenant_host",
                          "Tenant level access",
                          tenantId,
                          true,
                          adminUserId,
                          adminUserId,
                          DateTime.UtcNow,
                          DateTime.UtcNow
                        }   
                  }
              );
            migrationBuilder.InsertData(
                table: "RolePermissions",
                columns: new[]
                 {
                     "RoleID",
                     "PermissionID",
                     "TenantID",
                  },
                values: new object[,]
                {

                       {
                            Guid.Parse("30dec8bc-2b22-4b3b-b721-8eb28c5d39c9"),
                            Guid.Parse("a096fd03-ec1f-4a34-be90-29426e07534e"),
                            tenantId
                       },
                       {
                            Guid.Parse("30dec8bc-2b22-4b3b-b721-8eb28c5d39c9"),
                            Guid.Parse("f3563926-7a07-4e30-b514-fdc6ba6c465e"),
                            tenantId
                       },
                       {
                             Guid.Parse("30dec8bc-2b22-4b3b-b721-8eb28c5d39c9"),
                             Guid.Parse("4ee04083-2cc8-4e89-bf07-27149bc72dd1"),
                             tenantId
                       },
                      {
                            Guid.Parse("30dec8bc-2b22-4b3b-b721-8eb28c5d39c9"),
                            Guid.Parse("47d2b5bd-d65a-4aab-90c2-7278db5d5b04"),
                            tenantId
                       },

                }
            );

        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // 1. Remove RolePermissions first (child table)

            migrationBuilder.DeleteData(
                table: "RolePermissions",
                keyColumns: new[] { "RoleID", "PermissionID" },
                keyValues: new object[]
                {
            Guid.Parse("30dec8bc-2b22-4b3b-b721-8eb28c5d39c9"),
            Guid.Parse("a096fd03-ec1f-4a34-be90-29426e07534e")
                });

            migrationBuilder.DeleteData(
                table: "RolePermissions",
                keyColumns: new[] { "RoleID", "PermissionID" },
                keyValues: new object[]
                {
            Guid.Parse("30dec8bc-2b22-4b3b-b721-8eb28c5d39c9"),
            Guid.Parse("f3563926-7a07-4e30-b514-fdc6ba6c465e")
                });

            migrationBuilder.DeleteData(
                table: "RolePermissions",
                keyColumns: new[] { "RoleID", "PermissionID" },
                keyValues: new object[]
                {
            Guid.Parse("30dec8bc-2b22-4b3b-b721-8eb28c5d39c9"),
            Guid.Parse("4ee04083-2cc8-4e89-bf07-27149bc72dd1")
                });

            migrationBuilder.DeleteData(
                table: "RolePermissions",
                keyColumns: new[] { "RoleID", "PermissionID" },
                keyValues: new object[]
                {
            Guid.Parse("30dec8bc-2b22-4b3b-b721-8eb28c5d39c9"),
            Guid.Parse("47d2b5bd-d65a-4aab-90c2-7278db5d5b04")
                });

            // 2. Remove the Role

            migrationBuilder.DeleteData(
                table: "Roles",
                keyColumn: "ID",
                keyValue: Guid.Parse("30dec8bc-2b22-4b3b-b721-8eb28c5d39c9")
            );

            // 3. Remove the Permissions

            migrationBuilder.DeleteData(
                table: "Permissions",
                keyColumn: "ID",
                keyValue: Guid.Parse("a096fd03-ec1f-4a34-be90-29426e07534e")
            );

            migrationBuilder.DeleteData(
                table: "Permissions",
                keyColumn: "ID",
                keyValue: Guid.Parse("f3563926-7a07-4e30-b514-fdc6ba6c465e")
            );

            migrationBuilder.DeleteData(
                table: "Permissions",
                keyColumn: "ID",
                keyValue: Guid.Parse("4ee04083-2cc8-4e89-bf07-27149bc72dd1")
            );

            migrationBuilder.DeleteData(
                table: "Permissions",
                keyColumn: "ID",
                keyValue: Guid.Parse("47d2b5bd-d65a-4aab-90c2-7278db5d5b04")
            );
        }

    }
}