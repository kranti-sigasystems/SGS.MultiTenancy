using SGS.MultiTenancy.Core.Domain.Common;
using System.ComponentModel.DataAnnotations;

namespace SGS.MultiTenancy.UI.Models
{
    public class PermissionsViewModel
    {

        /// <summary>
        /// Permission id for the update and delete
        /// </summary>
        public Guid? Id { get; set; }

        /// <summary>
        /// Permission Code
        /// </summary>

        [MaxLength(100, ErrorMessage = Constants.MaxErrorMessage)]
        [Required(ErrorMessage = Constants.RequiredErrorMessage)]
        public string? Code { get; set; } = string.Empty;


       /// <summary>
       /// Permission Description 
       /// </summary>
        [Required(ErrorMessage = Constants.RequiredErrorMessage)]
        public string? Description { get; set; } = string.Empty;

    }
}