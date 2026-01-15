using System.ComponentModel.DataAnnotations;

namespace SGS.MultiTenancy.UI.Models
{
    public class RegisterViewModel
    {
        /// <summary>
        /// Gets or sets the Permission name used for identification within the tenant.
        /// </summary>
        [Required]
        [MaxLength(100)]
        public string PermissionName { get; set; }

        /// <summary>
        /// Gets or sets the email address of the Permission.
        /// This value is used for communication and login purposes.
        /// </summary>
        [Required]
        [EmailAddress]
        [MaxLength(100)]
        public string Email { get; set; }

        /// <summary>
        /// Gets or sets the password for the Permission account.
        /// Must be between 8 and 25 characters.
        /// </summary>
        [Required]
        [RegularExpression(
        @"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&])[A-Za-z\d@$!%*?&]{8,25}$",
        ErrorMessage = "Password must be 8–25 characters and include uppercase, lowercase, number, and special character.")]
        public required string Password { get; set; }

        /// <summary>
        /// Gets or sets the Confirm Password for the Permission account.
        /// Must be between 8 and 25 characters.
        /// </summary>
        [Required]
        [Compare(nameof(Password), ErrorMessage = "Comfirm Password and Password should match.")]
        public string ConfirmPassword { get; set; }

        /// <summary>
        /// Gets or sets the phone number associated with the Permission.
        /// </summary>
        [Required(ErrorMessage = "Phone number is required.")]
        [MaxLength(25)]
        
        public string PhoneNumber { get; set; }


        /// <summary>
        /// Gets or sets the unique identifier of the tenant to which the Permission belongs.
        /// </summary>
        [Required]
        public Guid TenantId { get; set; }

        /// <summary>
        /// Gets or sets the optional address identifier associated with the Permission.
        /// </summary>
        public Guid? AddressId { get; set; }
    }
}
