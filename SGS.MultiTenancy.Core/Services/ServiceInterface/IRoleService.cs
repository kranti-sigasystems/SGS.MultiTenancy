using SGS.MultiTenancy.Core.Application.DTOs;
using SGS.MultiTenancy.UI.Models;

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
        Task CreateRoleAsync(CreateRoleViewModel model, Guid tenantId);

        /// <summary>
        /// Retrieves all roles associated with a specific tenant.
        /// </summary>
        /// <param name="tenantId"></param>
        /// <returns>List of roles.</returns>
        Task<List<RoleDto>> GetRolesByTenantAsync(Guid tenantId);
    }
}