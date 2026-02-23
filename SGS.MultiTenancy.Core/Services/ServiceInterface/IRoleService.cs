using SGS.MultiTenancy.Core.Domain.Entities.Auth;
using SGS.MultiTenancy.UI.Models;

namespace SGS.MultiTenancy.Core.Services.ServiceInterface
{
    public interface IRoleService
    {
        Task<List<Permission>> GetAllPermissionsAsync();
        Task CreateRoleAsync(CreateRoleViewModel model, Guid tenantId);
        Task<List<Role>> GetRolesByTenantAsync(Guid tenantId);
    }
}
