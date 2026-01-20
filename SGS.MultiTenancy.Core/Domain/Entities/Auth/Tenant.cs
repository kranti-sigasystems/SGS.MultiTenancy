using SGS.MultiTenancy.Core.Application.Interfaces;
using SGS.MultiTenancy.Core.Domain.Common;
using SGS.MultiTenancy.Core.Domain.Enums;
using SGS.MultiTenancy.Core.Entities.Common;
using System.ComponentModel.DataAnnotations;

namespace SGS.MultiTenancy.Core.Domain.Entities.Auth
{
    public class Tenant : AuditableEntity
    {
        /// <summary>
        /// Primary key (UUID).
        /// </summary>
        [Key]
        public Guid ID { get; set; }

        /// <summary>
        /// Customer / business name (e.g., "Acme Corp").
        /// </summary>
        [Required]
        [MaxLength(200)]
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets Unique slug for subdomains
        /// </summary>
        [Required]
        [MaxLength(100)]
        public string Slug { get; set; }

        /// <summary>
        /// Gets or sets Domain
        /// </summary>
        [MaxLength(255)]
        public string? Domain { get; set; }

        /// <summary>
        /// Gets or sets status of tenant.
        /// </summary>
        public EntityStatus Status { get; set; }

        /// <summary>
        /// Logo URL.
        /// </summary>
        [MaxLength(500)]
        public string? LogoUrl { get; set; }
    }
}