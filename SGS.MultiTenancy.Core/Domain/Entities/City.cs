using SGS.MultiTenancy.Core.Domain.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SGS.MultiTenancy.Core.Domain.Entities
{
    /// <summary>
    /// Represents a city entity.
    /// </summary>
    public class City : AuditableEntity
    {
        /// <summary>
        /// Gets or sets the unique identifier of the city.
        /// </summary>
        [Key]
        public Guid ID { get; set; }

        /// <summary>
        /// Gets or sets the identifier of the state this city belongs to.
        /// </summary>
        [Required(ErrorMessage = Constants.RequiredErrorMessage)]
        public Guid StateID { get; set; }

        /// <summary>
        /// Gets or sets the name of the city.
        /// </summary>
        [Required(ErrorMessage = Constants.RequiredErrorMessage)]
        [MaxLength(50, ErrorMessage = Constants.MaxErrorMessage)]
        [DisplayName("City Name")]
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the city is active.
        /// </summary>
        public bool IsActive { get; set; } = true;

        /// <summary>
        /// Gets or sets the state associated with this city.
        /// </summary>
        public State State { get; set; }
    }
}
