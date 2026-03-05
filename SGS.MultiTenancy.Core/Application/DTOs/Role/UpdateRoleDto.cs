namespace SGS.MultiTenancy.Core.Application.DTOs.Role
{
    public class UpdateRoleDto
    {

        /// <summary>
        /// Gets or set id.
        /// </summary>
        public Guid Id { get; set; }
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