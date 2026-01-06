using SGS.MultiTenancy.Core.Domain.Common;
using SGS.MultiTenancy.Core.Domain.Enums;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace SGS.MultiTenancy.Core.Domain.Entities
{
    /// <summary>
    /// Represents a country entity.
    /// </summary>
    public class Country : AuditableEntity
    {
        /// <summary>
        /// Gets or sets the unique identifier of the country.
        /// </summary>
        [Key]
        public Guid ID { get; set; }

        /// <summary>
        /// Gets or sets the name of the country.
        /// </summary>
        [Required]
        [MaxLength(50)]
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the code of the country (e.g., ISO code).
        /// </summary>
        [Required]
        [MaxLength(5)]
        public string Code { get; set; }

        /// <summary>
        /// Gets or sets status of tenant.
        /// </summary>
        public EntityStatus Status { get; set; }
    }
}
