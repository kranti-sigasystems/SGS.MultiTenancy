namespace SGS.MultiTenancy.UI.Models
{
    public class PermissionItemViewModel
    {
        /// <summary>
        /// Gets or sets the unique identifier of the permission item.
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Gets or sets the code of the permission.
        /// </summary>
        public string Code { get; set; } = default!;

        /// <summary>
        /// Gets or sets the display name of the permission.
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
