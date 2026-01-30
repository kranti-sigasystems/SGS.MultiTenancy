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
        Task<bool> ForgotPasswordAsync(string email);

        Task<bool> UserHasPermissionAsync(Guid userId, Guid tenantId, Guid permissionId);
    }
}