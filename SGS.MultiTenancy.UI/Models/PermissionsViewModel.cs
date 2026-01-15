using SGS.MultiTenancy.Core.Domain.Common;
using System.ComponentModel.DataAnnotations;

namespace SGS.MultiTenancy.UI.Models
{
    public class PermissionsViewModel
    {
        public Guid? Id { get; set; }


        [MaxLength(100, ErrorMessage = Constants.MaxErrorMessage)]
        [Required(ErrorMessage = Constants.RequiredErrorMessage)]
        public string? Code { get; set; } = string.Empty;

        [Required(ErrorMessage = Constants.RequiredErrorMessage)]
        public string? Description { get; set; } = string.Empty;

    }
}
