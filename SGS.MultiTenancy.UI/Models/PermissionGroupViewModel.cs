namespace SGS.MultiTenancy.UI.Models
{
    public class PermissionGroupViewModel
    {
        public string GroupName { get; set; } = default!;
        public List<PermissionItemViewModel> Permissions { get; set; } = new();
    }
}
