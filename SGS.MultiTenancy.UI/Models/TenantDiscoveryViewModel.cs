using System.ComponentModel.DataAnnotations;

namespace SGS.MultiTenancy.UI.Models
{
    public class TenantDiscoveryViewModel
    {

        /// <summary>
        /// Tenant or bussiness name
        /// </summary>
        [Required]
        [Display(Name = "Tenant / Business Name")]
        public string TenantName { get; set; } = string.Empty;
    }
}
