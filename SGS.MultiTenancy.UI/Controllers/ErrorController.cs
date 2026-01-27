using Microsoft.AspNetCore.Mvc;
using SGS.MultiTenancy.UI.Models;

namespace SGS.MultiTenancy.UI.Controllers
{
    public class ErrorController : Controller
    {
        [Route("ErrorP")]
        public IActionResult Error(Guid logId)
        {
            var model = new ErrorViewModel
            {
                LogId = logId,
                ErrorMessage = "An unexpected error occurred."
            };

            return View("ErrorPage", model);
        }
    }
}