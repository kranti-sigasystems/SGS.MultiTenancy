using Microsoft.EntityFrameworkCore;
using SGS.MultiTenancy.Core.Application.DTOs;
using SGS.MultiTenancy.Core.Application.Interfaces;
using SGS.MultiTenancy.Core.Domain.Entities.Auth;
using SGS.MultiTenancy.Core.Services.ServiceInterface;
namespace SGS.MultiTenancy.Core.Services
{
    /// <summary>
    /// Service implementation for managing permissions using generic repositories.
    /// </summary>
    public class PermissionService : IPermissionService
    {
        private readonly IPermissionRepository _permissionRepository;

        /// <summary>
        /// Initializes a new instance of the <see cref="PermissionService"/> class.
        /// </summary>
        public PermissionService(IPermissionRepository permissionRepository)
        {
            _permissionRepository = permissionRepository;
        }

        /// <summary>
        /// Retrieves a list of permissions.
        /// </summary>
        /// <returns>The list of permission.</returns>
        public async Task<List<Permission>> GetPermissionsAsync()
        {
            return await _permissionRepository.ListAsync(p => p.IsDeleted != true);
        }

        /// <summary>
        /// Retrieves a permission by its unique identifier.
        /// </summary>
        /// <param name="id">The unique identifier of the permission.</param>
        /// <returns>Return the permission by given identifier.</returns>
        public async Task<GenericResponseDto<Permission>?> GetPermissionByIdAsync(Guid id)
        {
            Permission? permission = await _permissionRepository.FirstOrDefaultAsync(p => p.ID == id && p.IsDeleted != true);

            if (permission == null)
            {
                return new GenericResponseDto<Permission>
                {
                    Message = "Permission not found.",
                    Data = null
                };
            }

            return new GenericResponseDto<Permission>
            {
                Message = "Permission retrieved successfully.",
                Data = permission
            };
        }

        /// <summary>
        /// Retrieves a permission by its unique code.
        /// </summary>
        /// <param name="code">The code of permission</param>
        /// <returns>Return the permission given code</returns>
        public async Task<GenericResponseDto<Permission>> GetPermissionByCodeAsync(string code)
        {
            Permission? permission = await _permissionRepository.FirstOrDefaultAsync(p => p.Code == code);

            if (permission == null)
            {
                return new GenericResponseDto<Permission>
                {
                    Message = "Permission not found.",
                    Data = null
                };
            }
            return new GenericResponseDto<Permission>
            {
                Message = "Permission retrieved successfully.",
                Data = permission
            };
        }


        /// <summary>
        /// Retrieves a paged list of permissions.
        /// </summary>
        /// <param name="page">Page number.</param>
        /// <param name="pageSize">Size of Permissions for the page.</param>
        /// <returns>list of permissions for the specified page, the total count, and paging information.</returns>
        public async Task<GenericResponseDto<Permission>> GetPagedPermissionsAsync(int page, int pageSize)
        {
            List<Permission> permissions = await _permissionRepository.GetPaged(p => p.IsDeleted != true, page, pageSize).ToListAsync();
            int totalCount = await _permissionRepository.CountAsync(p => !p.IsDeleted);

            return new GenericResponseDto<Permission>
            {
                Message = "Permissions retrieved successfully.",
                Items = permissions,
                TotalCount = permissions.Count,
                Page = page,
                PageSize = pageSize
            };
        }


        /// <summary>
        ///   Adds a new permission 
        /// </summary>
        /// <param name="request">A request object containing the permission data to add.</param>
        /// <returns>Created permission</returns>
        public async Task<GenericResponseDto<Permission>> AddPermissionAsync(GenericRequestDto<Permission> request)
        {
            string normalizedCode = request.Data.Code.Trim().ToUpper();

            Permission? existingPermission =
                await _permissionRepository.FirstOrDefaultAsync(p => p.Code == normalizedCode);

            if (existingPermission != null)
            {
                return new GenericResponseDto<Permission>
                {
                    Message = "Permission with the same code already exists.",
                    Data = existingPermission
                };
            }

            
            Permission permission = new Permission
            {
                Code = normalizedCode,
                Description = request.Data.Description,
            };

            Permission createdPermission = await _permissionRepository.AddAsync(permission);
            await _permissionRepository.CompleteAsync();

            return new GenericResponseDto<Permission>
            {
                Message = "Permission created successfully.",
                Data = createdPermission
            };
        }

        /// <summary>
        ///  Updates an existing permission.
        /// </summary>
        /// <param name="id">The unique identifier of the permission.</param>
        /// <param name="permissionRequest">A request object containing the updated permission data.</param>
        /// <returns>The task result contains a GenericResponseDto with the updated permission.</returns>
        public async Task<GenericResponseDto<Permission>> UpdatePermissionAsync(Guid id, GenericRequestDto<Permission> permissionRequest)
        {
            Permission? permission = await _permissionRepository.FirstOrDefaultAsync(p => p.ID == id);
            if (permission != null)
            {
                permission.Code = permissionRequest.Data!.Code.ToUpper();
                permission.Description = permissionRequest.Data.Description;
                permission.UpdateOn = permissionRequest.Data.UpdateOn;
                permission.UpdateBy = permissionRequest.Data.UpdateBy;
                Permission updatedPermission = await _permissionRepository.UpdateAsync(permission);
                await _permissionRepository.CompleteAsync();
                return new GenericResponseDto<Permission>
                {
                    Message = "Permission updated successfully.",
                    Data = updatedPermission
                };
            }
            else
            {
                return new GenericResponseDto<Permission>
                {
                    Message = "Permission not found.",
                    Data = null
                };
            }
        }

        /// <summary>
        /// Deletes the permission.
        /// </summary>
        /// <param name="id">The unique identifier of the permission.</param>
        /// <returns>The task result contains a GenericResponseDto.</returns>
        public async Task<GenericResponseDto<Permission>> DeletePermissionAsync(Guid id)
        {
            Permission? permission = await _permissionRepository.FirstOrDefaultAsync(p => p.ID == id);
            if (permission != null)
            {
                await _permissionRepository.DeleteAsync(permission);
                await _permissionRepository.CompleteAsync();
                return new GenericResponseDto<Permission>
                {
                    Message = "Permission deleted successfully.",
                    Data = null
                };
            }
            else
            {
                return new GenericResponseDto<Permission>
                {
                    Message = "Permission not found.",
                    Data = null
                };
            }
        }


    }
}