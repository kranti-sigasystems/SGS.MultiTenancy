using SGS.MultiTenancy.Core.Domain.Common;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace SGS.MultiTenancy.UI.Models
{
    public class LoginViewModel
    {
        /// <summary>
        /// Gets or sets the user name associated with the account.
        /// </summary>
        [Display(Name = Constants.UserNameDisplay)]
        [MaxLength(100,ErrorMessage = Constants.MaxErrorMessage)]
        [Required(ErrorMessage = Constants.RequiredErrorMessage)]
        public required string UserName { get; set; }

        /// <summary>
        /// Gets or sets the password for the user.
        /// </summary>
        [Required(ErrorMessage = Constants.RequiredErrorMessage)]
        [MinLength(8, ErrorMessage = Constants.MinErrorMessage)]
        [MaxLength(25, ErrorMessage = Constants.MaxErrorMessage)]
        public required string Password { get; set; }
    }
}
