
namespace SGS.MultiTenancy.Core.Application.DTOs.Permission
{
    public class PermissionRequestDto
    {
        /// <summary>
        /// Permission unique identifier
        /// </summary>
        public Guid? Id { get; set; }

        /// <summary>
        /// Permission unique code
        /// </summary>
        public string? Code { get; set; }

        /// <summary>
        /// Permission description
        /// </summary>
        public string? Description { get; set; }

        /// <summary>
        /// updated by user identifier
        /// </summary>
        public Guid? UpdatedBy { get; set; }

    }
}