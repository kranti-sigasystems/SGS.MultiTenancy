using System.ComponentModel.DataAnnotations;

namespace SGS.MultiTenancy.UI.Models
{
    public class ForgotPasswordViewModel
    {
        /// <summary>
        /// Gets or sets email.
        /// </summary>
        [Required]
        [EmailAddress]
        [Display(Name = "Email Address")]
        public string Email { get; set; } = string.Empty;
    }
}
