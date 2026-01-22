using SGS.MultiTenancy.Core.Entities.Common;

namespace SGS.MultiTenancy.Core.Domain.Entities.Auth
{
    /// <summary>
    /// Represents the association between a user and an address.
    /// This entity acts as a mapping table allowing a user to have
    /// one or more addresses, optionally scoped to a tenant.
    /// </summary>
    public class UserAddress
    {
        /// <summary>
        /// Gets or sets the unique identifier of the user.
        /// </summary>
        public Guid UserID { get; set; }

        /// <summary>
        /// Navigation property for the associated user.
        /// </summary>
        public User User { get; set; }

        /// <summary>
        /// Gets or sets the unique identifier of the address.
        /// </summary>
        public Guid AddressId { get; set; }

        /// <summary>
        /// Navigation property for the associated address.
        /// </summary>
        public Address Address { get; set; }

        /// <summary>
        /// Gets or sets the unique identifier of the tenant
        /// under which this address association exists, if applicable.
        /// </summary>
        public Guid? TenantID { get; set; }
    }
}
