using SGS.MultiTenancy.Core.Domain.Entities.Auth;

namespace SGS.MultiTenancy.Core.Application.Interfaces
{
    /// <summary>
    /// Generates a JWT token for an authenticated user with assigned roles and permissions.
    /// </summary>
    public interface IJwtTokenGenerator
    {
        /// <summary>
        /// Creates a JWT token containing user, role, permission, and tenant claims.
        /// </summary>
        /// <param name="applicationUser">The authenticated application user.</param>
        /// <param name="roles">The roles assigned to the user.</param>
        /// <param name="permissions">The permissions granted to the user.</param>
        /// <returns>JWT access token.</returns>
        string GenerateToken(
            User applicationUser,
            IEnumerable<string> roles,
            IEnumerable<string> permissions);
    }
}
