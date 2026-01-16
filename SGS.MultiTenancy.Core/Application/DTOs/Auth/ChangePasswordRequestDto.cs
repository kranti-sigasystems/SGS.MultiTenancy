namespace SGS.MultiTenancy.Core.Application.DTOs.Auth
{
    /// <summary>
    /// Sends data required to change a user password.
    /// </summary>
    public class ChangePasswordRequestDto
    {
        /// <summary>
        /// Gets or sets the user identifier.
        /// </summary>
        public Guid UserId { get; set; }
        /// <summary>
        /// Gets or sets the old password.
        /// </summary>
        public string OldPassword { get; set; }
        /// <summary>
        /// Gets or sets the new password.
        /// </summary>
        public string NewPassword { get; set; }
    }
}