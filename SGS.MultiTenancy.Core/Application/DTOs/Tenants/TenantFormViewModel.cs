using Microsoft.AspNetCore.Mvc.Rendering;
using SGS.MultiTenancy.Core.Domain.Enums;
using System.ComponentModel.DataAnnotations;

namespace SGS.MultiTenancy.Core.Application.DTOs.Tenants
{
    public class TenantFormViewModel
    {
        /// <summary>
        /// Gets or sets the unique identifier for the tenant.
        /// </summary>
        public Guid ID { get; set; }
        
        /// <summary>
        /// Gets or sets the tenant's display name.
        /// </summary>
        [Required]
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the tenant's registered business name.
        /// </summary>
        [Required]
        public string BussinessName { get; set; }

        /// <summary>
        /// Gets or sets the tenant's email address.
        /// Must be a valid email format.
        /// </summary>
        [Required, EmailAddress]
        public string Email { get; set; }

        /// <summary>
        /// Gets or sets the tenant's primary phone number.
        /// </summary>
        [Required]
        public string PhoneNumber { get; set; }

        /// <summary>
        /// Gets or sets the tenant's address line (street, building, etc.).
        /// </summary>
        [Required]
        public string AddressLine { get; set; }

        /// <summary>
        /// Gets or sets the postal code for the tenant's address.
        /// </summary>
        [Required]
        public string PostalCode { get; set; }

        /// <summary>
        /// Gets or sets the city associated with this address.
        /// </summary>
        public string City { get; set; }

        /// <summary>
        /// Gets or sets the identifier of the state associated with the tenant's address.
        /// </summary>
        [Required]
        public Guid StateID { get; set; }

        /// <summary>
        /// Gets or sets the list of available countries for dropdown selection.
        /// </summary>
        public IEnumerable<SelectListItem>? Countries { get; set; }

        /// <summary>
        /// Gets or sets the list of available states for dropdown selection.
        /// </summary>
        public IEnumerable<SelectListItem>? States { get; set; }

        /// <summary>
        /// Gets or sets status of tenant.
        /// </summary>
        public EntityStatus Status { get; set; }

        public Guid AddressID { get; set; }
        public Guid CountryID { get; set; }
    }
}


