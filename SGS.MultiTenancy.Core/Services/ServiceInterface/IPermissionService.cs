using SGS.MultiTenancy.Core.Application.DTOs.Permission;
namespace SGS.MultiTenancy.Core.Services.ServiceInterface
{
    public interface IPermissionService
    {
        /// <summary>
        /// Gets the list of permissions grouped by their categories.
        /// </summary>
        /// <returns></returns>
        Task<List<PermissionGroupDto>> GetGroupedPermissionsAsync();
    }
}
