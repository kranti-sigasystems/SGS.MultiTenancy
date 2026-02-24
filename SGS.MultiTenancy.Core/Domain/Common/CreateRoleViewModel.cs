using SGS.MultiTenancy.Core.Application.DTOs;
namespace SGS.MultiTenancy.UI.Models
{
    /// <summary>
    ///  View model used to capture data required for creating a new role.
    /// </summary>
    public class CreateRoleViewModel
    {
        /// <summary>
        /// Gets or sets the name of the role.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets a brief description of the role.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets the list of permission identifiers selected.
        /// </summary>
        public List<Guid> SelectedPermissions { get; set; }

        /// <summary>
        /// Gets or set a list of roles.
        /// </summary>
        public List<RoleDto> RolesList { get; set; }

        /// <summary>
        /// Gets or set a list of Permissions.
        /// </summary>
        public List<PermissionDto> PermissionList { get; set; }
    }
}