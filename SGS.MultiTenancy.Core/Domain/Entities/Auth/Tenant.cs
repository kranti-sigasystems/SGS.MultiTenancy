using SGS.MultiTenancy.Core.Domain.Common;
using SGS.MultiTenancy.Core.Domain.Enums;
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
        /// Gets or sets customer / business name.
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
        /// Gets or sets the registration number.
        /// </summary>
        public string RegistrationNumber { get; set; }

        /// <summary>
        /// Gets or sets logo URL.
        /// </summary>
        [MaxLength(500)]
        public string? LogoUrl { get; set; }
    }
}