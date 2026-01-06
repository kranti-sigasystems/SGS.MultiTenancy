using SGS.MultiTenancy.Core.Domain.Common;
using SGS.MultiTenancy.Core.Domain.Enums;
using SGS.MultiTenancy.Core.Entities.Common;
using System.ComponentModel.DataAnnotations;

namespace SGS.MultiTenancy.Core.Domain.Entities.Auth
{
    public class Tenant : AuditableEntity
    {
        /// <summary>
        /// Gets or sets the unique identifier of the tenant.
        /// </summary>
        [Key]
        public Guid ID { get; set; }

        /// <summary>
        /// Gets or sets tenant name.
        /// </summary>
        [Required(ErrorMessage = Constants.RequiredErrorMessage)]
        [MaxLength(200, ErrorMessage = Constants.MaxErrorMessage)]
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets bussiness name.
        /// </summary>
        [Required(ErrorMessage = Constants.RequiredErrorMessage)]
        [MaxLength(200, ErrorMessage = Constants.MaxErrorMessage)]
        public string BussinessName { get; set; }

        /// <summary>
        /// Gets or sets email address.
        /// </summary>
        [Required(ErrorMessage = Constants.RequiredErrorMessage)]
        [MaxLength(100, ErrorMessage = Constants.MaxErrorMessage)]
        public string Email { get; set; }

        /// <summary>
        /// Gets or sets phone number.
        /// </summary>
        [Required(ErrorMessage = Constants.RequiredErrorMessage)]
        [MaxLength(25, ErrorMessage = Constants.MaxErrorMessage)]
        public string PhoneNumber { get; set; }

        /// <summary>
        /// Gets or sets status of tenant.
        /// </summary>
        public EntityStatus Status { get; set; }

        /// <summary>
        /// Gets or sets address identifier.
        /// </summary>
        public Guid AddressID { get; set; }

        /// <summary>
        /// Gets or sets address.
        /// </summary>
        public Address Address { get; set; }
    }
}