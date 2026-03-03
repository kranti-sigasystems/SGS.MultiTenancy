namespace SGS.MultiTenancy.UI.Models
{
    public class PermissionItemViewModel
    {
        public Guid Id { get; set; }
        public string Code { get; set; } = default!;
        public string Name { get; set; } = default!;
    }
}
