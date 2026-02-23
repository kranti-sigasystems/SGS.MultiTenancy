namespace SGS.MultiTenancy.UI.Models
{
    public class CreateRoleViewModel
    {
        public string Name { get; set; }
        public string Description { get; set; }

        public List<Guid> SelectedPermissions { get; set; }
    }
}