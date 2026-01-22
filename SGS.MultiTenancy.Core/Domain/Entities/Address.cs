using SGS.MultiTenancy.Core.Domain.Common;
using SGS.MultiTenancy.Core.Domain.Entities.Auth;
using System.ComponentModel.DataAnnotations;

namespace SGS.MultiTenancy.Core.Entities.Common
{
    /// <summary>
    /// Represents an address entity.
    /// </summary>
    public class Address : AuditableEntity
    {
        /// <summary>
        /// Gets or sets the unique identifier of the address.
        /// </summary>
        [Key]
        public Guid ID { get; set; }

        /// <summary>
        /// Gets or sets phone number.
        /// </summary>
        [Required]
        [MaxLength(25)]
        public string PhoneNumber { get; set; }

        /// <summary>
        /// Gets or sets the address line (e.g., street name and number).
        /// </summary>
        [Required]
        [MaxLength(250)]
        public string AddressLine { get; set; }

        /// <summary>
        /// Gets or sets the postal code of the address.
        /// </summary>
        [Required]
        [MaxLength(10)]
        public string PostalCode { get; set; }

        /// <summary>
        /// Gets or sets the city associated with this address.
        /// </summary>
        public string City { get; set; }

        /// <summary>
        /// Gets or sets the state associated with this address.
        /// </summary>
        public string State { get; set; }

        /// <summary>
        /// Gets or sets the country associated with this address.
        /// </summary>
        public string Country { get; set; }

        /// <summary>
        /// Gets or sets the unique identifier of the tenant.
        /// </summary>
        public Guid? TenantID { get; set; }

        /// <summary>
        /// Indicates whether this is the default address for the user
        /// </summary>
        public bool IsDefault { get; set; }

        /// <summary>
        /// Gets or sets the address associated with this user.
        /// </summary>
        public ICollection<UserAddress> UserAddresses { get; set; }

    }
}