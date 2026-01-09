using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SGS.MultiTenancy.UI.Models;

namespace SGS.MultiTenancy.UI.Controllers
{
    [Authorize (Roles = "SGS_SuperHost")]
    public class TenantController : Controller
    {
        private readonly ILogger<TenantController> _logger;

        public TenantController(ILogger<TenantController> logger)
        {
            _logger = logger;
        }

        public IActionResult AddTenant()
        {
            return View(new TenantFormViewModel());
        }
        public IActionResult UpdateTenant()
        {
            return View(new TenantFormViewModel());
        }
        public IActionResult Index()
        {
            return View(new TenantFormViewModel());
        }
    }
}
