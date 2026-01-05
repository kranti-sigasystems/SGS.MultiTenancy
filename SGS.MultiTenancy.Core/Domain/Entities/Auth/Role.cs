using SGS.MultiTenancy.Core.Domain.Common;
using System.ComponentModel.DataAnnotations;

namespace SGS.MultiTenancy.Core.Domain.Entities.Auth
{
    /// <summary>
    /// Represents a role used for authorization.
    /// </summary>
    public class Role : AuditableEntity
    {
        [Key]
        /// <summary>
        /// Gets or sets the unique identifier of the role.
        /// </summary>
        public Guid ID { get; set; }

        /// <summary>
        /// Gets or sets the name of the role.
        /// </summary>
        [Required]
        [MaxLength(50)]
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the description of the role.
        /// </summary>
        [Required]
        [MaxLength(200)]
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets the users assigned to this role.
        /// </summary>
        public ICollection<UserRoles> UserRoles { get; set; }

        /// <summary>
        /// Gets or sets the permissions associated with this role.
        /// </summary>
        public ICollection<RolePermission> RolePermissions { get; set; }
    }
}