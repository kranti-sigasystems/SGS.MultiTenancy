using SGS.MultiTenancy.Core.Domain.Common;
using System.ComponentModel.DataAnnotations;

namespace SGS.MultiTenancy.UI.Models
{
    public class ChangePasswordViewModel
    {
        /// <summary>
        /// Gets or sets the user's current password.
        /// </summary>
        [Display(Name = Constants.CurrentPasswordDisplay)]
        [Required(ErrorMessage = Constants.RequiredErrorMessage)]
        [DataType(DataType.Password)]
        public required string CurrentPassword { get; set; }

        /// <summary>
        /// Gets or sets the new password to be set for the user.
        /// </summary>
        [Display(Name =Constants.NewPasswordDisplay)]
        [Required(ErrorMessage = Constants.RequiredErrorMessage)]
        [DataType(DataType.Password)]
        [MinLength(Constants.PasswordMinLength, ErrorMessage = Constants.MinErrorMessage)]
        [RegularExpression(Constants.PasswordStrengthRegex,ErrorMessage = Constants.PasswordStrengthErrorMessage)]
        public required string NewPassword { get; set; }

        /// <summary>
        /// Gets or sets the confirmation entry for the new password used to verify that the user has entered the new password correctly.
        /// </summary>
        /// <remarks>The value must match the value of the NewPassword property for the operation to
        /// succeed. This property is typically used in password reset or change workflows to prevent typographical
        /// errors.</remarks>
        [Display(Name = Constants.ConfirmPasswordDisplay)]
        [Required(ErrorMessage = Constants.RequiredErrorMessage)]
        [Compare(nameof(NewPassword), ErrorMessage = Constants.PasswordsDoNotMatch)]
        [DataType(DataType.Password)]
        public required string ConfirmNewPassword { get; set; }
    }
}
