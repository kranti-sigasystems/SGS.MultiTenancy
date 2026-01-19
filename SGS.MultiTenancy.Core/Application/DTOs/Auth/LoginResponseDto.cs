namespace SGS.MultiTenancy.Core.Application.DTOs.Auth
{
    /// <summary>
    /// Represents the response returned after a successful login, including user information, authentication token,assigned roles, and tenant identifier.
    /// </summary>
    public class LoginResponseDto
    {
        /// <summary>
        /// Gets or sets the user associated with the current context.
        /// </summary>
        public UserDto User { get; set; }
        /// <summary>
        /// Gets or sets the authentication token.
        /// </summary>
        public string Token { get; set; }
        /// <summary>
        /// Gets or sets the list of roles assigned to the user.
        /// </summary>
        public List<string> Roles { get; set; }
        /// <summary>
        /// Gets or sets the unique identifier for the tenant.
        /// </summary>
        public Guid TenantID { get; set; }
    }
}
