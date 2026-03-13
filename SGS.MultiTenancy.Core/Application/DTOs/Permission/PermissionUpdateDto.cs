
using System.ComponentModel.DataAnnotations;

namespace SGS.MultiTenancy.Core.Application.DTOs.Permission
{
    public class PermissionUpdateDto
    {
        public Guid ID { get; set; }
        /// <summary>
        /// Gets or set group name.
        /// </summary>

        [StringLength(100)]
        public string Group { get; set; } = string.Empty;

        /// <summary>
        /// Gets or set action.
        /// </summary>

        [StringLength(100)]
        public string Action { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets code.
        /// </summary>

        [StringLength(201)]
        public string Code { get; set; } = string.Empty;


        /// <summary>
        /// Gets or set Description.
        /// </summary>
        [StringLength(500)]
        public string? Description { get; set; }

        /// <summary>
        /// Gets or set Tenant id.
        /// </summary>
        public Guid? TenantId { get; set; }
    }
}
