using Microsoft.EntityFrameworkCore;
using SGS.MultiTenancy.Core.Application.DTOs.Permission;
using SGS.MultiTenancy.Core.Application.Interfaces;
using SGS.MultiTenancy.Core.Domain.Entities.Auth;
using SGS.MultiTenancy.Core.Services.ServiceInterface;

namespace SGS.MultiTenancy.Core.Services
{
    public class PermissionService : IPermissionService
    {
        private readonly IGenericRepository<Permission> _permissionRepository;

        public PermissionService(
            IGenericRepository<Permission> permissionRepository)
        {
            _permissionRepository = permissionRepository;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>

        public async Task<List<PermissionGroupDto>> GetGroupedPermissionsAsync()
        {
            var permissions = await _permissionRepository
                                        .Query()
                                        .AsNoTracking()
                                        .ToListAsync();

            var grouped = permissions
                .Where(p => !string.IsNullOrWhiteSpace(p.Code))
                .GroupBy(p => p.Code.Split('.', 2)[0])
                .Select(g => new PermissionGroupDto
                {
                    GroupName = g.Key.ToUpper(),
                    Permissions = g.Select(p =>
                    {
                        var parts = p.Code.Split('.', 2);

                        return new PermissionItemDto
                        {
                            Id = p.ID,
                            Code = p.Code,
                            Name = parts.Length > 1 ? parts[1] : p.Code
                        };
                    }).ToList()
                })
                .OrderBy(g => g.GroupName)
                .ToList();

            return grouped;
        }
    }
}
