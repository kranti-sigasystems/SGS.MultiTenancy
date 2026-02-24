using Microsoft.AspNetCore.Http;
using SGS.MultiTenancy.Core.Domain.Common;
using SGS.MultiTenancy.Core.Domain.Enums;
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

        [Required(ErrorMessage = Constants.RequiredErrorMessage)]
        [Display(Name = "User Name")]
        public string UserName { get; set; }

        /// <summary>
        /// Gets or set user email address.
        /// </summary>

        [Required(ErrorMessage = Constants.RequiredErrorMessage)]
        [EmailAddress(ErrorMessage = Constants.EmailErrorMessage)]
        public string Email { get; set; }

        /// <summary>
        /// Gets or set user password.
        /// </summary>
        [Required(ErrorMessage = Constants.RequiredErrorMessage)]
        [MinLength(Constants.PasswordMinLength, ErrorMessage = Constants.MinErrorMessage)]
        [MaxLength(25, ErrorMessage = Constants.MaxErrorMessage)]
        [RegularExpression(
        Constants.PasswordStrengthRegex,
        ErrorMessage = Constants.PasswordStrengthErrorMessage)]
        public string Password { get; set; }

        /// <summary>
        /// Gets or set user avtar url.
        /// </summary>
        public string? AvtarUrl { get; set; }

        /// <summary>
        /// Get or set user profile image file.
        /// </summary>
        public IFormFile? ProfileImage { get; set; }

        /// <summary>
        /// Gets or set user tenant id.
        /// </summary>
        public Guid? TenantId { get; set; }

        /// <summary>
        /// Gets or set role ids.
        /// </summary>
        public List<Guid>? RoleIds { get; set; }= new List<Guid>();

        /// <summary>
        /// Gets or sets the list of addresses associated with the user.
        /// </summary>
        public List<CreateUserAddressDto>? Addresses { get; set; }

        /// <summary>
        /// Gets or set status.
        /// </summary>
        public EntityStatus Status { get; set; }
    }
}