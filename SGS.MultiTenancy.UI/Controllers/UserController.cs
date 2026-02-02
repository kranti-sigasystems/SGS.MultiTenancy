using Microsoft.AspNetCore.Mvc;

namespace SGS.MultiTenancy.UI.Controllers
{
    public class UserController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
