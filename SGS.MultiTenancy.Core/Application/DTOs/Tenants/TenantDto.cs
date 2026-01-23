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
        public string Name { get; set; }

        /// <summary>
        /// Unique slug for subdomains.
        /// </summary>
        [Required]
        [MaxLength(100)]
        [RegularExpression(@"^[a-z0-9\-]+$", ErrorMessage = "Slug must contain only lowercase letters, numbers, and hyphens.")]
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
        [Url(ErrorMessage = "Invalid logo URL format.")]
        public string? LogoUrl { get; set; }
    }
}


