using Microsoft.AspNetCore.Mvc;
using SGS.MultiTenancy.Core.Domain.Common;
namespace SGS.MultiTenancy.UI.Controllers
{
    public class DashBoardController : Controller
    {
        public IActionResult Index()
        {
            
            if (User.IsInRole(Constants.SuperAdminHost))
            {
                return RedirectToAction(
                        nameof(TenantController.Index),
                        Utility.PrepareControllerName(nameof(TenantController))
                );
            }
            return View();
        }

        public IActionResult SuperTenantDashboard()
        {
            return View();
        }
    }
}
