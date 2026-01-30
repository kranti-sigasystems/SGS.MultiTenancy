using System.ComponentModel.DataAnnotations;
namespace SGS.MultiTenancy.Core.Application.DTOs.Auth
{
    public class UserDto
    {
        /// <summary>
        /// Gets or set user unique identifier.
        /// </summary>
        public Guid? ID { get; set; }

        /// <summary>
        /// Gets or set user name.
        /// </summary>

        [Required(ErrorMessage = "User name is required")]
        [Display(Name = "User Name")]
        public string UserName { get; set; }

        /// <summary>
        /// Gets or set user email address.
        /// </summary>

        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Invalid Email Address")]
        public string Email { get; set; }

        /// <summary>
        /// Gets or set user password.
        /// </summary>
        [Required(ErrorMessage = "Password is required")]
        public string Password { get; set; }

        /// <summary>
        /// Gets or set user avtar url.
        /// </summary>
        public string? AvtarUrl { get; set; }

        /// <summary>
        /// Gets or set user tenant id.
        /// </summary>
        public Guid? TenantId { get; set; }

    }
}