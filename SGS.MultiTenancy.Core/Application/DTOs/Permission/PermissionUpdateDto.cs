
namespace SGS.MultiTenancy.Core.Application.DTOs.Permission
{
    public class PermissionUpdateDto
    {
        /// <summary>
        /// Gets or set identifier.
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Gets or set code.
        /// </summary>
        public string? Code { get; set; }

        /// <summary>
        /// Gets or set description.
        /// </summary>
        public string? Descrition { get; set; }

        /// <summary>
        /// Gets or set tenant id.
        /// </summary>
        public Guid? TenantId { get; set; }
    }
}
