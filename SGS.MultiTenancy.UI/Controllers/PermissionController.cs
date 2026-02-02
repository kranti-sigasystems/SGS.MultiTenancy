using Microsoft.AspNetCore.Mvc;

namespace SGS.MultiTenancy.UI.Controllers
{
    public class PermissionController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
