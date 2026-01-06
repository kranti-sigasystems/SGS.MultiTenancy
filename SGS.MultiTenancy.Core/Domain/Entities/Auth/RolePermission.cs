using SGS.MultiTenancy.Core.Domain.Common;
using System.ComponentModel.DataAnnotations;

namespace SGS.MultiTenancy.Core.Domain.Entities.Auth
{
    /// <summary>
    /// Represents the association between a role and a permission within a tenant.
    /// </summary>
    public class RolePermission : AuditableEntity
    {
        /// <summary>
        /// Gets or sets the unique identifier of the permission.
        /// </summary>
        [Key]
        public Guid ID { get; set; }

        /// <summary>
        /// Gets or sets the unique identifier of the role.
        /// </summary>
        public Guid RoleID { get; set; }

        /// <summary>
        /// Gets or sets the role associated with this permission.
        /// </summary>
        public Role Role { get; set; }

        /// <summary>
        /// Gets or sets the unique identifier of the permission.
        /// </summary>
        public Guid PermissionID { get; set; }

        /// <summary>
        /// Gets or sets the permission associated with this role.
        /// </summary>
        public Permission Permission { get; set; }

        /// <summary>
        /// Gets or sets the unique identifier of the tenant.
        /// </summary>
        public Guid TenantID { get; set; }
    }
}