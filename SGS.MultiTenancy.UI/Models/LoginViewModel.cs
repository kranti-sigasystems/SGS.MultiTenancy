using SGS.MultiTenancy.Core.Domain.Common;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace SGS.MultiTenancy.UI.Models
{
    public class LoginViewModel
    {
        [MaxLength(100,ErrorMessage = Constants.MaxErrorMessage)]
        [Required(ErrorMessage = Constants.RequiredErrorMessage)]
        public string UserName { get; set; }

        [Required(ErrorMessage = Constants.RequiredErrorMessage)]
        [MinLength(8, ErrorMessage = Constants.MinErrorMessage)]
        [MaxLength(25, ErrorMessage = Constants.MaxErrorMessage)]
        public string Password { get; set; }
    }
}
