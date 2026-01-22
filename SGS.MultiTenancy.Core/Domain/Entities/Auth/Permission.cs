using SGS.MultiTenancy.Core.Domain.Common;
using System.ComponentModel.DataAnnotations;


namespace SGS.MultiTenancy.Core.Domain.Entities.Auth
{
    /// <summary>
    ///  Represents a system permission for access control.
    /// </summary>
    public class Permission : AuditableEntity
    { 
        /// <summary>
        /// Gets or sets the unique identifier of the permission.
        /// </summary>
        [Key]
        public Guid ID { get; set; }

        /// <summary>
        /// Gets or sets the unique permission code.
        /// </summary>
        [Required]
        [StringLength(50)]
        public string Code { get; set; }

        /// <summary>
        /// Gets or sets the description of the permission.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets the unique identifier of the tenant.
        /// </summary>
        public Guid? TenantID { get; set; }

        /// <summary>
        /// Gets or sets the permissions associated with this role.
        /// </summary>
        public ICollection<RolePermission> RolePermissions { get; set; }
    }
}