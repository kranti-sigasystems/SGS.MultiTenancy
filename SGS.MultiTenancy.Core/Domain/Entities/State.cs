using SGS.MultiTenancy.Core.Domain.Common;
using SGS.MultiTenancy.Core.Domain.Enums;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace SGS.MultiTenancy.Core.Domain.Entities
{
    /// <summary>
    /// Represents a state entity.
    /// </summary>
    public class State : AuditableEntity
    {
        /// <summary>
        /// Gets or sets the unique identifier of the state.
        /// </summary>
        [Key]
        public Guid ID { get; set; }

        /// <summary>
        /// Gets or sets the identifier of the country this state belongs to.
        /// </summary>
        [Required]
        public Guid CountryID { get; set; }

        /// <summary>
        /// Gets or sets the name of the state.
        /// </summary>
        [Required]
        [MaxLength(50)]
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the optional code of the state (e.g., abbreviation).
        /// </summary>
        [MaxLength(10)]
        public string? Code { get; set; }

        /// <summary>
        /// Gets or sets status of tenant.
        /// </summary>
        public EntityStatus Status { get; set; }

        /// <summary>
        /// Gets or sets the country associated with this state.
        /// </summary>
        public Country Country { get; set; }
    }
}
