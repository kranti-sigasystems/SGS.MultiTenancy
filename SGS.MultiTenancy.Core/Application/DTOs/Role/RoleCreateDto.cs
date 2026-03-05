namespace SGS.MultiTenancy.Core.Application.DTOs.Role
{
    /// <summary>
    /// Represents the data transfer object for creating a role.
    /// </summary>
    public class RoleCreateDto
    {
        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the description.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets the selected permissions.
        /// </summary>
        public List<Guid> SelectedPermissions { get; set; }
    }
}
