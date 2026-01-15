using SGS.MultiTenancy.Core.Application.DTOs;
using SGS.MultiTenancy.Core.Domain.Entities.Auth;
namespace SGS.MultiTenancy.Core.Services.ServiceInterface
{
    /// <summary>
    /// Provides operations for managing permissions within the system.
    /// </summary>
    public interface IPermissionService
    {
        /// <summary>
        ///  Retrieves the list of permissions .
        /// </summary>
        /// <returns>List of permissions.</returns>
        public Task<List<Permission>> GetPermissionsAsync();

        /// <summary>
        /// Retrieves a permission by its unique identifier.
        /// </summary>
        /// <param name="id">The unique identifier of the permission.</param>
        /// <returns>
        /// A response containing the permission if found.
        /// </returns>
        Task<GenericResponseDto<Permission>?> GetPermissionByIdAsync(Guid id);

        /// <summary>
        /// Retrieves a permission by its unique code.
        /// </summary>
        /// <param name="code">The unique code of the permission.</param>
        /// <returns>
        /// A response containing the permission if found.
        /// </returns>
        Task<GenericResponseDto<Permission>> GetPermissionByCodeAsync(string code);

        /// <summary>
        /// Retrieves a paginated list of permissions.
        /// </summary>
        /// <param name="page">
        /// Zero-based index of the page to retrieve.
        /// </param>
        /// <param name="pageSize">
        /// Number of permissions to include per page.
        /// </param>
        /// <returns>
        /// A response containing a paginated list of permissions.
        /// </returns>
        Task<GenericResponseDto<Permission>> GetPagedPermissionsAsync(int page, int pageSize);

        /// <summary>
        /// Creates a new permission.
        /// </summary>
        /// <param name="request">
        /// Request object containing permission details.
        /// </param>
        /// <returns>
        /// A response containing the created permission.
        /// </returns>
        Task<GenericResponseDto<Permission>> AddPermissionAsync(
            GenericRequestDto<Permission> request);

        /// <summary>
        /// Updates an existing permission.
        /// </summary>
        /// <param name="id">
        /// The unique identifier of the permission to update.
        /// </param>
        /// <param name="request">
        /// Request object containing updated permission details.
        /// </param>
        /// <returns>
        /// A response containing the updated permission.
        /// </returns>
        Task<GenericResponseDto<Permission>> UpdatePermissionAsync(
            Guid id,
            GenericRequestDto<Permission> request);

        /// <summary>
        /// Deletes a permission by its identifier.
        /// </summary>
        /// <param name="id">
        /// The unique identifier of the permission to delete.
        /// </param>
        /// <returns>
        /// A response indicating the result of the delete operation.
        /// </returns>
        Task<GenericResponseDto<Permission>> DeletePermissionAsync(Guid id);
    }
}
