using System.ComponentModel.DataAnnotations;

namespace SGS.MultiTenancy.Core.Application.DTOs
{
    public class RoleDto
    {
        /// <summary>
        /// Gets or sets the unique identifier of the role.
        /// </summary>
        [Key]
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
        /// Gets or sets the unique identifier of the tenant.
        /// </summary>
        public Guid? TenantID { get; set; }

        /// <summary>
        /// Assign this role to new members automatically.
        /// </summary>
        public bool IsDefault { get; set; }

        /// <summary>
        /// Gets or sets the collection of role permissions assigned to the user.
        /// </summary>
        public List<RoleDto> RolePermissions { get; set; }
    }
}
