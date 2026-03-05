namespace SGS.MultiTenancy.Core.Application.DTOs.Permission
{
    public class PermissionItemDto
    {

        /// <summary>
        /// Gets or set identifier.
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Gets or set code.
        /// </summary>
        public string Code { get; set; } = default!;

        /// <summary>
        /// Gets or set name.
        /// </summary>
        public string Name { get; set; } = default!;

        /// <summary>
        /// Gets or set Description.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Gets or set tenant id.
        /// </summary>
        public Guid? TenantId { get; set; }
    }
}
