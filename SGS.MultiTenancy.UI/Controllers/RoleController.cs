using Microsoft.AspNetCore.Mvc;

namespace SGS.MultiTenancy.UI.Controllers
{
    public class RoleController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
