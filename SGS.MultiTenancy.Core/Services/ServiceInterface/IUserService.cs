using SGS.MultiTenancy.Core.Application.DTOs.Auth;
namespace SGS.MultiTenancy.Core.Services.ServiceInterface
{
    public interface IUserService
    {
        /// <summary>
        /// Authenticates a user using the provided login credentials.
        /// </summary>
        /// <param name="loginRequestDto">The login request data containing user credentials.</param>
        /// <returns>Returns login response dto </returns>
        Task<LoginResponseDto> Login(LoginRequestDto loginRequestDto);

        /// <summary>
        /// Validates current password and updates to new one.
        /// </summary>
        /// <param name="userId">User ID.</param>
        /// <param name="currentPassword">Current password.</param>
        /// <param name="newPassword">New password.</param>
        /// <returns>Success flag and error message if failed.</returns>
        Task<(bool Success, string ErrorMessage)> ChangePasswordAsync(Guid userId, string currentPassword, string newPassword);

        /// <summary>
        /// Checks if a user has a specific permission within a tenant.
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="tenantId"></param>
        /// <param name="permissionId"></param>
        /// <returns>True or false</returns>
        Task<bool> UserHasPermissionAsync(Guid userId, Guid tenantId, Guid permissionId);

        /// <summary>
        /// Add user.
        /// </summary>
        /// <param name="userDto"></param>
        /// <returns></returns>
         Task<UserDto> AddUserAsync(UserDto userDto);
        /// <summary>
        /// Fetch users associated with tenants.
        /// </summary>
        /// <param name="tenantId"></param>
        /// <returns></returns>
        Task<List<UserDto>> GetUsersByTenantAsync(Guid tenantId);

        /// <summary>
        /// Updates user.
        /// </summary>
        /// <param name="userDto"></param>
        /// <returns>UserDto</returns>
         Task<UserDto> UpdateUserAsync(UserDto userDto);
        
        /// <summary>
        /// Deletes the user that is associated with the specified unique identifier.
        /// </summary>
        Task<bool> DeleteUserAsync(Guid userId, Guid tenantId);
    }
}