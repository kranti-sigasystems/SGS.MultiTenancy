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
        /// Registers a new user and.
        /// </summary>
        /// <param name="user"></param>
        /// <returns>returns a confirmation message</returns>
        Task<string> Register(UserDto user);

        /// <summary>
        /// Gets all users by tenant id.
        /// </summary>
        /// <param name="tenantid"></param>
        /// <returns>Return list</returns>
        Task<List<UserDto>> GetUsersByTenantId(Guid tenantid);

        /// <summary>
        /// Gets user by id.
        /// </summary>
        /// <param name="tenantId"></param>
        Task<UserDto?> GetUsersById(Guid tenantId);

        /// <summary>
        /// Updates user information.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="usermodel"></param>
        Task<UserDto?> UpdateUser(Guid id, UserDto usermodel);

        /// <summary>
        /// Deletes a user by id.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<UserDto?> DeleteUser(Guid id);
    }
}