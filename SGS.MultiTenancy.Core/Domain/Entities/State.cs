using SGS.MultiTenancy.Core.Domain.Common;
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
        [Required(ErrorMessage = Constants.RequiredErrorMessage)]
        public Guid CountryID { get; set; }

        /// <summary>
        /// Gets or sets the name of the state.
        /// </summary>
        [Required(ErrorMessage = Constants.RequiredErrorMessage)]
        [MaxLength(50, ErrorMessage = Constants.MaxErrorMessage)]
        [DisplayName("State Name")]
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the optional code of the state (e.g., abbreviation).
        /// </summary>
        [MaxLength(10, ErrorMessage = Constants.MaxErrorMessage)]
        [DisplayName("State Code")]
        public string? Code { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the state is active.
        /// </summary>
        public bool IsActive { get; set; } = true;

        /// <summary>
        /// Gets or sets the country associated with this state.
        /// </summary>
        public Country Country { get; set; }
    }
}
