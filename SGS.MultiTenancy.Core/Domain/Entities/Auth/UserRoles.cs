namespace SGS.MultiTenancy.Core.Domain.Entities.Auth
{
    public class UserRoles
    {
        /// <summary>
        /// Gets or sets user identifier.
        /// </summary>
        public Guid UserID { get; set; }

        /// <summary>
        /// Gets or sets user.
        /// </summary>
        public User User { get; set; }

        /// <summary>
        /// Gets or sets role identifier.
        /// </summary>
        public Guid RoleID { get; set; }

        /// <summary>
        /// Gets or sets role.
        /// </summary>
        public Role Role { get; set; }

        /// <summary>
        /// Gets or sets tenant identifier.
        /// </summary>
        public Guid? TenantID { get; set; }
    }
}