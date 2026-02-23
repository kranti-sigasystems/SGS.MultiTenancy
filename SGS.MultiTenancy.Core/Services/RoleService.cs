using Microsoft.EntityFrameworkCore;
using SGS.MultiTenancy.Core.Application.Interfaces;
using SGS.MultiTenancy.Core.Domain.Entities.Auth;
using SGS.MultiTenancy.Core.Services.ServiceInterface;
using SGS.MultiTenancy.UI.Models;

namespace SGS.MultiTenancy.Core.Services
{
    public class RoleService : IRoleService
    {
        private readonly IGenericRepository<Role> _roleRepository;
        private readonly IGenericRepository<Permission> _permissionRepository;
        private readonly IGenericRepository<RolePermission> _rolePermissionRepository;

        public RoleService(
            IGenericRepository<Role> roleRepository,
            IGenericRepository<Permission> permissionRepository,
            IGenericRepository<RolePermission> rolePermissionRepository)
        {
            _roleRepository = roleRepository;
            _permissionRepository = permissionRepository;
            _rolePermissionRepository = rolePermissionRepository;
        }

        public async Task<List<Permission>> GetAllPermissionsAsync()
        {
            return await _permissionRepository
                .Query()
                 .Where(p => p.TenantID == Guid.Empty)
                .ToListAsync();
        }

        public async Task CreateRoleAsync(CreateRoleViewModel model, Guid tenantId)
        {
            Role role = new Role
            {
                ID = Guid.NewGuid(),
                Name = model.Name,
                Description = model.Description,
                TenantID = tenantId,
                IsDefault = false
            };

            await _roleRepository.AddAsync(role);
            await _roleRepository.CompleteAsync();

            if (model.SelectedPermissions != null)
            {
                foreach (var permissionId in model.SelectedPermissions)
                {
                    RolePermission rolePermission = new RolePermission
                    {
                        RoleID = role.ID,
                        PermissionID = permissionId,
                        TenantID = tenantId
                    };

                    await _rolePermissionRepository.AddAsync(rolePermission);
                    await _rolePermissionRepository.CompleteAsync();
                }
            }
        }
        public async Task<List<Role>> GetRolesByTenantAsync(Guid tenantId)
        {
            return await _roleRepository
                .Query()
                .Where(r => r.TenantID == tenantId)
                .ToListAsync();
        }
    }
}