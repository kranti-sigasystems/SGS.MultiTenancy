namespace SGS.MultiTenancy.Core.Application.DTOs.Permission
{
    public class PermissionGroupDto
    {
        /// <summary>
        /// Gets or set gourp name of the permission.
        /// </summary>
        public string GroupName { get; set; } = default!;

        /// <summary>
        /// Gets or set the list of permissions.
        /// </summary>
        public List<PermissionItemDto> Permissions { get; set; } = new();
    }
}
