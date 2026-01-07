using SGS.MultiTenancy.Core.Domain.Enums;
using System.ComponentModel.DataAnnotations;

namespace SGS.MultiTenancy.UI.Models
{
    public class TenantFormViewModel
    {
        /// <summary>
        /// Gets or sets the unique identifier of the tenant.
        /// </summary>
        public Guid? Id { get; set; }

        /// <summary>
        /// Gets or sets the name of the tenant.
        /// </summary>
        [Required(ErrorMessage = "Tenant name is required")]
        [StringLength(200)]
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the business name of the tenant.
        /// </summary>
        [Required(ErrorMessage = "Business name is required")]
        [StringLength(200)]
        [Display(Name = "Business Name")]
        public string BussinessName { get; set; }

        /// <summary>
        /// Gets or sets the email address of the tenant.
        /// </summary>
        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Invalid email address")]
        [StringLength(100)]
        public string Email { get; set; }

        /// <summary>
        /// Gets or sets the phone number of the tenant.
        /// </summary>
        [Required(ErrorMessage = "Phone number is required")]
        [StringLength(25)]
        [Display(Name = "Phone Number")]
        public string PhoneNumber { get; set; }

        /// <summary>
        /// Gets or sets the address identifier of the tenant.
        /// </summary>
        [Required(ErrorMessage = "Address is required")]
        [Display(Name = "Address")]
        public Guid? AddressId { get; set; }

        /// <summary>
        /// Gets or sets the status of the tenant.
        /// </summary>
        [Display(Name = "Status")]
        public EntityStatus? Status { get; set; }
    }
}
