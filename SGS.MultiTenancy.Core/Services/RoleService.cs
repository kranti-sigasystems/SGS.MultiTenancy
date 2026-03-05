using Microsoft.EntityFrameworkCore;
using SGS.MultiTenancy.Core.Application.DTOs;
using SGS.MultiTenancy.Core.Application.DTOs.Role;
using SGS.MultiTenancy.Core.Application.Interfaces;
using SGS.MultiTenancy.Core.Domain.Entities.Auth;
using SGS.MultiTenancy.Core.Services.ServiceInterface;

namespace SGS.MultiTenancy.Core.Services
{
    /// <summary>
    /// Provides role and permission management operations for a multi-tenant system.
    /// </summary>
    public class RoleService : IRoleService
    {
        private readonly IGenericRepository<Role> _roleRepository;
        private readonly IGenericRepository<Permission> _permissionRepository;
        private readonly IGenericRepository<RolePermission> _rolePermissionRepository;

        /// <summary>
        /// Initializes a new instance of the <see cref="RoleService"/> class.
        /// </summary>
        public RoleService(
            IGenericRepository<Role> roleRepository,
            IGenericRepository<Permission> permissionRepository,
            IGenericRepository<RolePermission> rolePermissionRepository)
        {
            _roleRepository = roleRepository;
            _permissionRepository = permissionRepository;
            _rolePermissionRepository = rolePermissionRepository;
        }

        /// <summary>
        /// Retrieves all system-level permissions that can be assigned to roles.
        /// </summary>
        public async Task<List<PermissionDto>> GetAllPermissionsAsync()
        {
            return await _permissionRepository
                .Query(p => p.TenantID == Guid.Empty || p.TenantID == Guid.Parse("00000000-0000-0000-0000-000000000000"))
                .Select(p => new PermissionDto
                {
                    ID = p.ID,
                    Code = p.Code,
                    Description = p.Description,
                    TenantID = p.TenantID
                }).ToListAsync();
        }

        /// <summary>
        /// Creates a new role for the specified tenant and assigns
        /// the selected permissions to the role.
        /// </summary>
        public async Task CreateRoleAsync(RoleCreateDto dto, Guid tenantId)
        {
            Role role = new Role
            {
                ID = Guid.NewGuid(),
                Name = dto.Name,
                Description = dto.Description,
                TenantID = tenantId,
                IsDefault = false
            };
            await _roleRepository.AddAsync(role);

            if (dto.SelectedPermissions != null)
            {
                List<RolePermission>? rolePermissions = dto.SelectedPermissions
                                         .Distinct()
                                         .Select(permissionId => new RolePermission
                                         {
                                             RoleID = role.ID,
                                             PermissionID = permissionId,
                                             TenantID = tenantId
                                         })
                                         .ToList();
                await _rolePermissionRepository.AddRangeAsync(rolePermissions);
            }
            await _roleRepository.CompleteAsync();
        }
        /// <summary>
        /// Retrieves all roles associated with a specific tenant.
        /// </summary>
        public async Task<List<RoleDto>> GetRolesByTenantAsync(Guid tenantId)
        {
            return await _roleRepository
                .Query(r => r.TenantID == tenantId)
                .Select(r => new RoleDto
                {
                    ID = r.ID,
                    Name = r.Name,
                    Description = r.Description,
                    TenantID = r.TenantID,
                    IsDefault = r.IsDefault
                }).ToListAsync();
        }

        /// <inheritdoc/>
        public async Task<RoleDto> GetRolesByIdandTenantIdAsync(Guid roleid, Guid tenantId)
        {

            Role? role = await _roleRepository.Query(role => role.ID == roleid && role.TenantID == tenantId).FirstOrDefaultAsync();

            List<RolePermission> rolepermissions = await _rolePermissionRepository.Query(rp => rp.RoleID == roleid)
                     .Include(rp => rp.Permission)
                     .ToListAsync();
            return new RoleDto
            {
                ID = role.ID,
                Name = role.Name,
                Description = role.Description,
                TenantID = role.TenantID,
                IsDefault = role.IsDefault,
                RolePermissions = rolepermissions
            };
        }


        /// <inheritdoc/>
        public async Task UpdateRoleAsync(UpdateRoleDto dto, Guid tenantId)
        {
            Role? role = await _roleRepository.Query(r => r.ID == dto.Id && r.TenantID == tenantId).FirstOrDefaultAsync();
            if (role != null)
            {
                role.Name = dto.Name;
                role.Description = dto.Description;
                await _roleRepository.UpdateAsync(role);
                List<Guid>? existingPermissions = await _rolePermissionRepository
                    .Query(rp => rp.RoleID == role.ID && rp.TenantID == tenantId)
                    .Select(rp => rp.PermissionID).ToListAsync();

                List<Guid>? incomingPermissionIds = dto.SelectedPermissions?
                                               .Distinct()
                                               .ToList() ?? new List<Guid>();


                List<Guid>? permissionsToAdd = incomingPermissionIds
                    .Except(existingPermissions)
                    .ToList();

                List<Guid>? permissionsToRemove = existingPermissions
                    .Except(incomingPermissionIds)
                    .ToList();


                if (permissionsToAdd.Count > 0)
                {
                    IEnumerable<RolePermission>? newEntities = permissionsToAdd
                        .Select(pid => new RolePermission
                        {
                            RoleID = role.ID,
                            PermissionID = pid,
                            TenantID = tenantId
                        });

                    await _rolePermissionRepository.AddRangeAsync(newEntities);
                }


                if (permissionsToRemove.Count > 0)
                {
                    List<RolePermission>? removeEntities = await _rolePermissionRepository
                        .Query(rp =>
                            rp.RoleID == role.ID &&
                            rp.TenantID == tenantId &&
                            permissionsToRemove.Contains(rp.PermissionID))
                        .ToListAsync();

                    await _rolePermissionRepository.DeleteRangeAsync(removeEntities);
                }

                await _roleRepository.CompleteAsync();
            }
        }

        /// <inheritdoc/>
        public async Task DeleteAsync(Guid id, Guid tenantid)
        {

            Role? role = await _roleRepository.Query(role => role.ID == id && role.TenantID == tenantid).FirstOrDefaultAsync();

            if (role == null)
            {
                return;
            }
            else
            {
                List<RolePermission>? rolePermissions = await _rolePermissionRepository.Query(rp => rp.RoleID == id && tenantid == role.TenantID).ToListAsync();
                await _rolePermissionRepository.DeleteRangeAsync(rolePermissions);
                await _rolePermissionRepository.CompleteAsync();
                await _roleRepository.DeleteAsync(role.ID);
                await _roleRepository.CompleteAsync();
            }
        }
    }
}