using SGS.MultiTenancy.Core.Domain.Common;
using SGS.MultiTenancy.Core.Domain.Entities;
using System.ComponentModel;
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
        /// Gets or sets the identifier of the city this address belongs to.
        /// </summary>
        [Required(ErrorMessage = Constants.RequiredErrorMessage)]
        public Guid CityID { get; set; }

        /// <summary>
        /// Gets or sets the address line (e.g., street name and number).
        /// </summary>
        [Required(ErrorMessage = Constants.RequiredErrorMessage)]
        [MaxLength(250, ErrorMessage = Constants.MaxErrorMessage)]
        [DisplayName(Constants.AddressLine)]
        public string AddressLine { get; set; }

        /// <summary>
        /// Gets or sets the postal code of the address.
        /// </summary>
        [Required(ErrorMessage = Constants.RequiredErrorMessage)]
        [MaxLength(10, ErrorMessage = Constants.MaxErrorMessage)]
        [DisplayName(Constants.PostalCode)]
        public string PostalCode { get; set; }

        /// <summary>
        /// Gets or sets the city associated with this address.
        /// </summary>
        public City City { get; set; }
    }
}