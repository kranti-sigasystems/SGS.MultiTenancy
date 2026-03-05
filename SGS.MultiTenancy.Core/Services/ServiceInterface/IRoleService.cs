using SGS.MultiTenancy.Core.Application.DTOs;
using SGS.MultiTenancy.Core.Application.DTOs.Role;

namespace SGS.MultiTenancy.Core.Services.ServiceInterface
{
    /// <summary>
    /// Defines role-related operations for managing roles and permissions
    /// within a multi-tenant environment.
    /// </summary>
    public interface IRoleService
    {
        /// <summary>
        ///  Retrieves all available permissions that can be assigned to roles.
        /// </summary>
        /// <returns>Permission dto.</returns>
        Task<List<PermissionDto>> GetAllPermissionsAsync();

        /// <summary>
        /// Creates a new role for the specified tenant and assigns
        /// the selected permissions to that role.
        /// </summary>
        Task CreateRoleAsync(RoleCreateDto dto, Guid tenantId);

        /// <summary>
        /// Retrieves all roles associated with a specific tenant.
        /// </summary>
        /// <param name="tenantId"></param>
        /// <returns>List of roles.</returns>
        Task<List<RoleDto>> GetRolesByTenantAsync(Guid tenantId);

        /// <summary>
        /// Retrieves a role by its unique identifier and tenant ID, including its assigned permissions.
        /// </summary>
        /// <param name="roleid"></param>
        /// <param name="tenantId"></param>
        /// <returns></returns>
        Task<RoleDto> GetRolesByIdandTenantIdAsync(Guid roleid, Guid tenantId);

        /// <summary>
        /// Update the existing role with the provided details and permissions for the specufied tenant.
        /// </summary>
        /// <param name="dto"></param>
        /// <param name="tenantId"></param>
        /// <returns></returns>
        Task UpdateRoleAsync(UpdateRoleDto dto, Guid tenantId);
    }
}