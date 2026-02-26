using Microsoft.AspNetCore.Http;
using SGS.MultiTenancy.Core.Application.DTOs.Auth;
using SGS.MultiTenancy.Core.Domain.Common;
using SGS.MultiTenancy.Core.Domain.Enums;
using System.ComponentModel.DataAnnotations;

namespace SGS.MultiTenancy.Core.Application.DTOs.Tenants
{
    public class TenantDto
    {
        /// <summary>
        /// Gets or sets the unique identifier of the tenant.
        /// </summary>
        public Guid? ID { get; set; }

        /// <summary>
        /// Customer / business name (e.g., "Acme Corp").
        /// </summary>
        [Required]
        [MaxLength(200)]
        [Display(Name = Constants.BussinessName)]
        public string Name { get; set; }

        /// <summary>
        /// Unique slug for subdomains.
        /// </summary>
        [Display(Name = Constants.SubDomainDisplay)]
        [Required(ErrorMessage = Constants.SubDomainError)]
        [MaxLength(100, ErrorMessage = Constants.MaxErrorMessage)]
        [RegularExpression(@"^[a-z0-9\-]+$", ErrorMessage = Constants.SlugInvalid)]
        public string Slug { get; set; }
        /// <summary>
        /// Custom domain for tenant.
        /// </summary>
        [MaxLength(255)]
        public string? Domain { get; set; }

        /// <summary>
        /// Status of the tenant.
        /// </summary>
        [Required]
        public EntityStatus Status { get; set; }

        /// <summary>
        /// Logo URL of the tenant.
        /// </summary>
        [MaxLength(500)]
        public string? LogoUrl { get; set; }

        /// <summary>
        /// Gets or set business logo.
        /// </summary>
        [Display(Name = Constants.BussinessLogoDisplay)]
        public IFormFile? BusinessLogo { get; set; }

        /// <summary>
        /// Official business registration number issued by government authority.
        /// </summary>
        [Required(ErrorMessage = Constants.RegistrationNumberError)]
        [Display(Name = Constants.RegistrationDisplay)]
        public string RegistrationNumber { get; set; } = string.Empty;

        /// <summary>
        /// Get or set user information related to the tenant. 
        /// </summary>
        public UserDto? UserDto { get; set; }
    }
}