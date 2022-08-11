using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using TVShop.Models;

namespace TVShop.Controllers
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
            if (HttpContext.Session.GetString("LoggedIn") == "yes")
            {
                ViewData["LoggedIn"] = "yes";
                ViewData["CustomerId"] = HttpContext.Session.GetInt32("CustomerId");
                ViewData["CustomerName"] = HttpContext.Session.GetString("CustomerName");
            }
            else
            {
                ViewData["LoggedIn"] = "no";
            }
            return View();
        }

        public IActionResult About()
        {
            if (HttpContext.Session.GetString("LoggedIn") == "yes")
            {
                ViewData["LoggedIn"] = "yes";
            }
            else
            {
                ViewData["LoggedIn"] = "no";
            }
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}