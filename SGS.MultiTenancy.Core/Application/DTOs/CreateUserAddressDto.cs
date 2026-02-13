using System.ComponentModel.DataAnnotations;

namespace SGS.MultiTenancy.Core.Application.DTOs
{
    public class CreateUserAddressDto
    {
        /// <summary>
        /// Gets or sets the phone number associated with the entity.
        /// </summary>
        [Required]
        [MaxLength(25)]
        public string PhoneNumber { get; set; }

        /// <summary>
        /// Gets or sets the address line.
        /// </summary>
        [Required]
        [MaxLength(250)]
        public string AddressLine { get; set; }

        /// <summary>
        /// Gets or sets the postal code.
        /// </summary>
        [Required]
        [MaxLength(10)]
        public string PostalCode { get; set; }

        /// <summary>
        /// Gets or sets the name of the city.
        /// </summary>
        public string? City { get; set; }

        /// <summary>
        /// Gets or sets the current state name.
        /// </summary>
        public string? State { get; set; }

        /// <summary>
        /// Gets or sets the country.
        /// </summary>
        public string? Country { get; set; }

        /// <summary>
        /// Gets or sets a is default.
        /// </summary>
        public bool IsDefault { get; set; }
    }
}
