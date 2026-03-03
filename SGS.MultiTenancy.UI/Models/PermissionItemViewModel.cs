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
    }
}
