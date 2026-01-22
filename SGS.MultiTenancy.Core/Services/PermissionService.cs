using SGS.MultiTenancy.Core.Application.DTOs.Permission;
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
        public async Task<PermissionResponseDto> GetPermissionsAsync()
        {
            List<Permission> permissions = await _permissionRepository.ListAsync(_ => true);
            PermissionResponseDto response = new PermissionResponseDto
            {
                Message = "Permissions retrieved successfully.",
                Permissions = permissions.Select(p => new PermissionRequestDto
                {
                    Code = p.Code,
                    Description = p.Description,
                    Id = p.ID
                }).ToList()
            };
            return response;
        }

        /// <summary>
        /// Retrieves a permission by its unique identifier.
        /// </summary>
        /// <param name="id">The unique identifier of the permission.</param>
        /// <returns>Return the permission by given identifier.</returns>
        public async Task<PermissionResponseDto?> GetPermissionByIdAsync(Guid id)
        {
            Permission? permission = await _permissionRepository.FirstOrDefaultAsync(p => p.ID == id);

            if (permission == null)
            {
                return new PermissionResponseDto
                {
                    Message = "Permission not found.",
                    Permissionobj = null
                };
            }

            return new PermissionResponseDto
            {
                Message = "Permission retrieved successfully.",
                Permissionobj = new PermissionRequestDto
                {
                    Code = permission.Code,
                    Description = permission.Description,
                    Id = permission.ID
                }
            };
        }

        /// <summary>
        /// Retrieves a permission by its unique code.
        /// </summary>
        /// <param name="code">The code of permission</param>
        /// <returns>Return the permission given code</returns>
        public async Task<PermissionResponseDto> GetPermissionByCodeAsync(string code)
        {
            Permission? permission = await _permissionRepository.FirstOrDefaultAsync(p => p.Code == code);

            if (permission == null)
            {
                return new PermissionResponseDto
                {
                    Message = "Permission not found.",
                    Permissionobj = null
                };
            }
            return new PermissionResponseDto
            {
                Message = "Permission retrieved successfully.",
                Permissionobj = new PermissionRequestDto
                {
                    Code = permission.Code,
                    Description = permission.Description,
                    Id = permission.ID
                }
            };
        }

        /// <summary>
        ///   Adds a new permission 
        /// </summary>
        /// <param name="request">A request object containing the permission data to add.</param>
        /// <returns>Created permission</returns>
        public async Task<PermissionResponseDto> AddPermissionAsync(PermissionRequestDto request)
        {
            string normalizedCode = request.Code!.Trim().ToUpper();

            Permission? existingPermission =
                await _permissionRepository.FirstOrDefaultAsync(p => p.Code == normalizedCode);

            if (existingPermission != null)
            {
                return new PermissionResponseDto
                {
                    Message = "Permission with the same code already exists.",
                    Permissionobj = new PermissionRequestDto
                    {
                        Id = existingPermission.ID,
                        Code = existingPermission.Code,
                        Description = existingPermission.Description
                    }
                };
            }


            Permission permission = new Permission
            {
                Code = normalizedCode,
                Description = request.Description!,
            };

            Permission createdPermission = await _permissionRepository.AddAsync(permission);
            await _permissionRepository.CompleteAsync();

            return new PermissionResponseDto
            {
                Message = "Permission created successfully.",
                Permissionobj = new PermissionRequestDto
                {
                    Id = createdPermission.ID,
                    Code = createdPermission.Code,
                    Description = createdPermission.Description
                }
            };
        }

        /// <summary>
        ///  Updates an existing permission.
        /// </summary>
        /// <param name="id">The unique identifier of the permission.</param>
        /// <param name="permissionRequest">A request object containing the updated permission data.</param>
        /// <returns>The task result contains a PermissionResponseDto with the updated permission.</returns>
        public async Task<PermissionResponseDto> UpdatePermissionAsync(Guid id, PermissionRequestDto permissionRequest)
        {
            Permission? permission = await _permissionRepository.FirstOrDefaultAsync(p => p.ID == id);
            if (permission != null)
            {
                permission.Code = permissionRequest.Code!.ToUpper();
                permission.Description = permissionRequest.Description!;
                permission.UpdateOn = DateTime.Now;
                permission.UpdateBy =permissionRequest.UpdatedBy;
                Permission updatedPermission = await _permissionRepository.UpdateAsync(permission);
                await _permissionRepository.CompleteAsync();
                return new PermissionResponseDto
                {
                    Message = "Permission updated successfully.",
                    Permissionobj = new PermissionRequestDto
                    { 
                        Id = permission.ID, 
                        Code = permission.Code,
                        Description = permission.Description 
                    }
                };
            }
            else
            {
                return new PermissionResponseDto
                {
                    Message = "Permission not found.",
                    Permissionobj = null
                };
            }
        }

        /// <summary>
        /// Deletes the permission.
        /// </summary>
        /// <param name="id">The unique identifier of the permission.</param>
        /// <returns>The task result contains a PermissionResponseDto.</returns>
        public async Task<PermissionResponseDto> DeletePermissionAsync(Guid id)
        {
            Permission? permission = await _permissionRepository.FirstOrDefaultAsync(p => p.ID == id);
            if (permission != null)
            {
                await _permissionRepository.DeleteAsync(permission);
                await _permissionRepository.CompleteAsync();
                return new PermissionResponseDto
                {
                    Message = "Permission deleted successfully.",
                    Permissionobj = null
                };
            }
            else
            {
                return new PermissionResponseDto
                {
                    Message = "Permission not found.",
                    Permissionobj = null
                };
            }
        }

        Task<PermissionResponseDto> IPermissionService.GetPagedPermissionsAsync(int page, int pageSize)
        {
            throw new NotImplementedException();
        }
    }
}