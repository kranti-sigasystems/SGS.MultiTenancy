using SGS.MultiTenancy.Core.Domain.Common;
using SGS.MultiTenancy.Core.Domain.Enums;
using SGS.MultiTenancy.Core.Entities.Common;
using System.ComponentModel.DataAnnotations;

namespace SGS.MultiTenancy.Core.Domain.Entities.Auth
{
    /// <summary>
    /// Represents an application user within a tenant.
    /// </summary>
    public class User : AuditableEntity
    {
        /// <summary>
        /// Gets or sets Primary key (UUID).
        /// </summary>
        [Key]
        public Guid ID { get; set; }

        /// <summary>
        /// Gets or sets User Name.
        /// </summary>
        [Required]
        [MaxLength(255)]
        public string UserName { get; set; }

        /// <summary>
        /// Gets or sets Email address (unique).
        /// </summary>
        [Required]
        [MaxLength(255)]
        public string Email { get; set; }

        /// <summary>
        /// Gets or sets password hash.
        /// </summary>
        [Required]
        [MaxLength(500)]
        public string PasswordHash { get; set; }

        /// <summary>
        /// Gets or sets User avatar URL.
        /// </summary>
        [MaxLength(500)]
        public string? AvatarUrl { get; set; }

        /// <summary>
        /// Gets or sets the unique identifier of the tenant.
        /// </summary>
        [Required]
        public Guid? TenantID { get; set; }

        /// <summary>
        /// Tenant + role mappings.
        /// </summary>
        public ICollection<UserRoles> UserRoles { get; set; } = new List<UserRoles>();

        /// <summary>
        /// Gets or sets the address associated with this user.
        /// </summary>
        public ICollection<UserAddress> UserAddresses { get; set; }


        /// <summary>
        /// Gets or sets status of user.
        /// </summary>
        public EntityStatus Status { get; set; }
    }
}