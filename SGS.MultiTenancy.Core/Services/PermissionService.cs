using Microsoft.EntityFrameworkCore;
using SGS.MultiTenancy.Core.Application.DTOs.Permission;
using SGS.MultiTenancy.Core.Application.Interfaces;
using SGS.MultiTenancy.Core.Application.Pagination;
using SGS.MultiTenancy.Core.Domain.Entities.Auth;
using SGS.MultiTenancy.Core.Extensions;
using SGS.MultiTenancy.Core.Services.ServiceInterface;
using System.Diagnostics.Contracts;

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
            await _permissionRepository.DeleteAsync(per.ID);
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
            Permission? permission = await _permissionRepository.Query(permission => permission.ID == model.ID).FirstOrDefaultAsync();
            if (permission == null) return;
            permission.Code = model.Code ?? permission.Code;
            permission.Description = model.Description ?? permission.Description;
            await _permissionRepository.UpdateAsync(permission);
            await _permissionRepository.CompleteAsync();
        }

        /// <inheritdoc/>
        public async Task<PagedResult<PermissionListDto>> GetPermissionsPagedAsync(
                           Guid tenantId,
                           PaginationParams paginationParams,
                           string? searchTerm,
                           string? groupName)
        {
            IQueryable<Permission> query = _permissionRepository
                .Query(p => p.TenantID == tenantId || p.TenantID == Guid.Empty)
                .AsNoTracking();

            if (!string.IsNullOrWhiteSpace(searchTerm))
            {
                string term = searchTerm.ToLower();

                query = query.Where(p =>
                    p.Code!.ToLower().Contains(term) ||
                    p.Description!.ToLower().Contains(term));
            }

            if (!string.IsNullOrWhiteSpace(groupName))
            {
                query = query.Where(p => p.Code!.StartsWith(groupName + "."));
            }

            query = query.OrderBy(p => p.Code);

            PagedResult<Permission> pagedPermissions =
                await query.ToPagedResultAsync(paginationParams);

            List<PermissionListDto> items = pagedPermissions.Items
                .Select(p =>
                {
                    string[] parts = p.Code!.Split('.', 2);

                    return new PermissionListDto
                    {
                        Id = p.ID,
                        Code = p.Code,
                        Name = parts.Length > 1 ? parts[1] : p.Code,
                        GroupName = parts[0].ToUpper(),
                        Description = p.Description,
                        TenantId = Guid.Parse(p.TenantID.ToString())
                    };
                })
                .ToList();

            return new PagedResult<PermissionListDto>(
                items,
                pagedPermissions.TotalCount,
                pagedPermissions.PageNumber,
                pagedPermissions.PageSize);
        }

        public async Task<PermissionItemDto> GetPermissionById(Guid id)
        {
            Permission? permission = await _permissionRepository.Query(permission => permission.ID == id).FirstOrDefaultAsync();

            return new PermissionItemDto
            {
                Code = permission!.Code,
                Description = permission.Description,
                TenantId = permission.TenantID,
                Id = permission.ID
            };
        }
    }
}