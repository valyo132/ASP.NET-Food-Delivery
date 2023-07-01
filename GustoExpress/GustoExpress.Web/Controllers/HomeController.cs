using GustoExpress.Web.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace GustoExpress.Web.Controllers
{
    public class HomeController : BaseController
    {
        public HomeController()
        {
        }

        public IActionResult Index(string? city)
        {
            if (city != null)
            {
                return RedirectToAction("All", "Restaurant", new { city });
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