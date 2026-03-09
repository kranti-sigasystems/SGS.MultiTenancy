using SGS.MultiTenancy.Core.Application.DTOs.Permission;
namespace SGS.MultiTenancy.Core.Services.ServiceInterface
{
    public interface IPermissionService
    {
        /// <summary>
        /// Gets the list of permissions grouped by their categories.
        /// </summary>
        /// <returns></returns>
        Task<List<PermissionGroupDto>> GetGroupedPermissionsAsync(Guid tenantId);

        /// <summary>
        /// Create a permission.
        /// </summary>
        /// <returns></returns>
        Task<PermissionCreateDto> CreatePermissionAsync(PermissionCreateDto model);

        /// <summary>
        /// Delete a permission based on th identifier.
        /// </summary>
        /// <returns></returns>
        Task DeletePermissionAsync(Guid id);


        /// <summary>
        /// Update a permission.
        /// </summary>
        /// <returns></returns>
        Task UpdatePermissionAsync(PermissionUpdateDto model);

        /// <summary>
        /// Gets a permission by identifier.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<PermissionCreateDto> GetPermissionByAsync(Guid id);
    }
}
