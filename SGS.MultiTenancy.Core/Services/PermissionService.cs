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
        private readonly IGenericRepository<RolePermission> _rolePermissionRepository;
        public PermissionService(
            IGenericRepository<Permission> permissionRepository, IGenericRepository<RolePermission> rolePermissionRepository)
        {
            _permissionRepository = permissionRepository;
            _rolePermissionRepository = rolePermissionRepository;
        }

        /// <summary>
        /// Retrieves all permissions from the repository, groups them by their top-level code segment,
        /// and returns a list of grouped permissions.
        /// </summary>
        public async Task<List<PermissionGroupDto>> GetGroupedPermissionsAsync(Guid tenantId)
        {
            List<Permission> permissions = await _permissionRepository
                                        .Query(permission => permission.TenantID == tenantId || permission.TenantID == Guid.Empty)
                                        .AsNoTracking()
                                        .ToListAsync();

            List<PermissionGroupDto> grouped = permissions
                .Where(p => !string.IsNullOrWhiteSpace(p.Code))
                .GroupBy(p => p.Code.Split('.', 2)[0])
                .Select(g => new PermissionGroupDto
                {
                    GroupName = g.Key.ToUpper(),
                    Permissions = g.Select(p =>
                    {
                        string[]? parts = p.Code.Split('.', 2);

                        return new PermissionItemDto
                        {
                            Id = p.ID,
                            Code = p.Code,
                            Description = p.Description,
                            TenantId = p.TenantID,
                            Name = parts.Length > 1 ? parts[1] : p.Code
                        };
                    }).ToList()
                })
                .OrderBy(g => g.GroupName)
                .ToList();

            return grouped;
        }

        /// <inheritdoc/>
        public async Task<PermissionCreateDto> CreatePermissionAsync(PermissionCreateDto model)
        {

            Permission permission = new Permission
            {
                Code = model.Code,
                Description = model.Description!,
                TenantID = model.TenantId,
                CreateOn = DateTime.Now
            };

            await _permissionRepository.AddAsync(permission);
            await _permissionRepository.CompleteAsync();
            return model;
        }

        /// <inheritdoc/>
        public async Task DeletePermissionAsync(Guid id)
        {

            Permission? per = await _permissionRepository.Query(pemission => pemission.ID == id).FirstOrDefaultAsync();

            if (per == null) return;

            List<RolePermission> rolepermissionlist = await _rolePermissionRepository.Query(rp => rp.PermissionID == id).ToListAsync();
            await _rolePermissionRepository.DeleteRangeAsync(rolepermissionlist);
            await _rolePermissionRepository.CompleteAsync();
            await _permissionRepository.DeleteAsync(per);
            await _permissionRepository.CompleteAsync();
        }

        /// <inheritdoc/>
        public async Task<PermissionCreateDto> GetPermissionByAsync(Guid id)
        {

            Permission? permission = await _permissionRepository.Query(pemission => pemission.ID == id).FirstOrDefaultAsync();
            if (permission == null)
            {
                return null!;
            }
            else
            {
                return new PermissionCreateDto
                {
                    Code = permission.Code,
                    Description = permission.Description,
                    TenantId = permission.TenantID
                };
            }
        }

        /// <inheritdoc/>
        public async Task UpdatePermissionAsync(PermissionUpdateDto model)
        {
            Permission? permission = await _permissionRepository.Query(permission => permission.ID == model.Id).FirstOrDefaultAsync();
            if (permission == null) return;
            permission.Code = model.Code ?? permission.Code;
            permission.Description = model.Descrition ?? permission.Description;
            await _permissionRepository.UpdateAsync(permission);
            await _permissionRepository.CompleteAsync();
        }
    }
}