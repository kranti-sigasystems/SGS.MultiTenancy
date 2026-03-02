using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SGS.MultiTenancy.Infra.Migrations
{
    /// <inheritdoc />
    public partial class permissionupdated : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // USERS
            migrationBuilder.Sql(@"
            UPDATE Permissions SET Code = 'users.create' WHERE Code = 'USER_CREATE';
            UPDATE Permissions SET Code = 'users.update' WHERE Code = 'USER_UPDATE';
            UPDATE Permissions SET Code = 'users.delete' WHERE Code = 'USER_DELETE';
            UPDATE Permissions SET Code = 'users.view'   WHERE Code = 'USER_VIEW';
            UPDATE Permissions SET Code = 'users.assign-role' WHERE Code = 'users.assignRole';
        ");

            // ROLES
            migrationBuilder.Sql(@"
            UPDATE Permissions SET Code = 'roles.read' WHERE Code = 'roles.read';
            UPDATE Permissions SET Code = 'roles.create' WHERE Code = 'roles.create';
            UPDATE Permissions SET Code = 'roles.update' WHERE Code = 'roles.update';
            UPDATE Permissions SET Code = 'roles.delete' WHERE Code = 'roles.delete';
            UPDATE Permissions SET Code = 'roles.assign-permissions' WHERE Code = 'roles.assignPermissions';
        ");

            // TENANTS
            migrationBuilder.Sql(@"
            UPDATE Permissions SET Code = 'tenants.create' WHERE Code = 'TENANT_CREATE';
            UPDATE Permissions SET Code = 'tenants.update' WHERE Code = 'TENANT_EDIT';
            UPDATE Permissions SET Code = 'tenants.delete' WHERE Code = 'TENANT_DELETE';
            UPDATE Permissions SET Code = 'tenants.view'   WHERE Code = 'TENANT_VIEW';
        ");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // Reverse back if needed
            migrationBuilder.Sql(@"
            UPDATE Permissions SET Code = 'USER_CREATE' WHERE Code = 'users.create';
            UPDATE Permissions SET Code = 'USER_UPDATE' WHERE Code = 'users.update';
            UPDATE Permissions SET Code = 'USER_DELETE' WHERE Code = 'users.delete';
            UPDATE Permissions SET Code = 'USER_VIEW'   WHERE Code = 'users.view';
            UPDATE Permissions SET Code = 'users.assignRole' WHERE Code = 'users.assign-role';

            UPDATE Permissions SET Code = 'roles.assignPermissions' WHERE Code = 'roles.assign-permissions';

            UPDATE Permissions SET Code = 'TENANT_CREATE' WHERE Code = 'tenants.create';
            UPDATE Permissions SET Code = 'TENANT_EDIT'   WHERE Code = 'tenants.update';
            UPDATE Permissions SET Code = 'TENANT_DELETE' WHERE Code = 'tenants.delete';
            UPDATE Permissions SET Code = 'TENANT_VIEW'   WHERE Code = 'tenants.view';
        ");
        }
    }
}
