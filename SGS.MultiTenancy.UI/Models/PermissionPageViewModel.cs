using SGS.MultiTenancy.Core.Domain.Common;

namespace SGS.MultiTenancy.UI.Models
{
    public class PermissionPageViewModel
    {
        public PagedResult<PermissionsViewModel> Permissions { get; set; }
        public PermissionsViewModel Form { get; set; }
    }
}