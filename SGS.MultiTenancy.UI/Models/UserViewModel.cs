using Microsoft.AspNetCore.Mvc.Rendering;
using SGS.MultiTenancy.Core.Application.DTOs.Auth;

namespace SGS.MultiTenancy.UI.Models
{
    public class UserViewModel
    {
        /// <summary>
        /// Gets or sets user dto.
        /// </summary>
        public UserDto User { get; set; } = new UserDto();

        /// <summary>
        /// Gets or sets user list.
        /// </summary>
        public List<UserDto>? UserList { get; set; } = new();

        /// <summary>
        /// Selected Country Id (for dropdown binding)
        /// </summary>
        public Guid? CountryId { get; set; }

        /// <summary>
        /// Selected State Id (for dropdown binding)
        /// </summary>
        public Guid? StateId { get; set; }

        /// <summary>
        /// Gets or sets country list.
        /// </summary>
        public List<SelectListItem> Countries { get; set; } = new();

        /// <summary>
        /// Gets or sets states list.
        /// </summary>
        public List<SelectListItem> States { get; set; } = new();
    }
}
