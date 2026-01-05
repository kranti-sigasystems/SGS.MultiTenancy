using SGS.MultiTenancy.Core.Domain.Common;
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
        /// Gets or sets user identifier.
        /// </summary>
        [Key]
        public Guid ID { get; set; }

        /// <summary>
        /// Gets or sets the unique identifier of the tenant.
        /// </summary>
        [Required]
        public Guid TenantID { get; set; }

        /// <summary>
        /// Gets or sets name.
        /// </summary>
        [Required]
        [MaxLength(100)]
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets email.
        /// </summary>
        [Required]
        [MaxLength(100)]
        public string Email { get; set; }

        /// <summary>
        /// Gets or sets phone number.
        /// </summary>
        [Required]
        [MaxLength(25)]
        public string PhoneNumber { get; set; }

        /// <summary>
        /// Gets or sets password.
        /// </summary>
        [Required]
        [MaxLength(25)]
        public string Password { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the user mobile number is verified.
        /// </summary>
        public bool IsMobileConfirmed { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the user email is verified.
        /// </summary>
        public bool IsEmailConfirmed { get; set; }

        // <summary>
        /// Gets or sets address identifier.
        /// </summary>
        public Guid AddressID { get; set; }

        /// <summary>
        /// Gets or sets address.
        /// </summary>
        public Address Address { get; set; }

        /// <summary>
        /// Gets or sets the roles assigned to the user.
        /// </summary>
        public ICollection<UserRoles> UserRoles { get; set; }
    }
}