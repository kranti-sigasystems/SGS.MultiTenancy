
namespace SGS.MultiTenancy.Core.Application.DTOs.Permission
{
    public class PermissionResponseDto
    {
        /// <summary>
        /// Returned the Updated or Fetched Permission Details.
        /// </summary>
        public PermissionRequestDto? Permissionobj { get; set; }

        /// <summary>
        /// List of permissions
        /// </summary>
        public List<PermissionRequestDto>? Permissions { get; set; }

        /// <summary>
        /// Option Status Message
        /// </summary>
        public string? Message { get; set; }
    }
}