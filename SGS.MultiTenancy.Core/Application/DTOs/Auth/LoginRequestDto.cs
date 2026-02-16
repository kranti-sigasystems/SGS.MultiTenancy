namespace SGS.MultiTenancy.Core.Application.DTOs.Auth
{
    public class LoginRequestDto
    {
        /// <summary>
        /// Gets or set user name.
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// Gets or set password.
        /// </summary>
        public string Password { get; set; }

        /// <summary>
        /// Gets or set tenantid.
        /// </summary>
        public Guid? TenantId { get; set; }
    }
}
