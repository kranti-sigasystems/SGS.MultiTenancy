namespace SGS.MultiTenancy.UI.Models
{
    public class PermissionPageViewModel
    {
        /// <summary>
        /// List of permission view model for view
        /// </summary>
        public List<PermissionsViewModel> Permissions { get; set; }

        /// <summary>
        /// Permission view model reference for the form
        /// </summary>
        public PermissionsViewModel Form { get; set; }
    }
}