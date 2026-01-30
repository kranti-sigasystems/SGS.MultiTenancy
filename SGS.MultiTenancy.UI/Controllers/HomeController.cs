using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using SGS.MultiTenancy.UI.Models;

namespace SGS.MultiTenancy.UI.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        [Route("Home/TestDivideByZero")]
        public IActionResult TestDivideByZero(string returnUrl)
        {
            // Store returnUrl so middleware can read it
            HttpContext.Items["ReturnUrl"] = returnUrl;

            _logger.LogInformation("Attempting to divide by zero...");

            int numerator = 10;
            int denominator = 0;

            int result = numerator / denominator; // 💥 DivideByZeroException

            return Content($"Result: {result}");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

    }
}
