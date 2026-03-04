namespace SGS.MultiTenancy.UI.Models
{
    public class PermissionGroupViewModel
    {
        /// <summary>
        /// Gets or sets the name of the permission group.
        /// </summary>
        public string GroupName { get; set; } = default!;

        /// <summary>
        /// Gets or sets the list of permissions that belong to this group.
        /// </summary>
        public List<PermissionItemViewModel> Permissions { get; set; } = new();
    }
}
