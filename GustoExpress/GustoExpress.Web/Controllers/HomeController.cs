namespace GustoExpress.Web.Controllers
{
    using Microsoft.AspNetCore.Mvc;

    using static GustoExpress.Web.Common.GeneralConstraints;

    public class HomeController : BaseController
    {
        public HomeController()
        {
        }

        public IActionResult Index(string? city)
        {
            if (this.User.IsInRole("Admin"))
            {
                return RedirectToAction("Index", "Home", new { Area = ADMIN_AREA_NAME });
            }

            if (city != null)
            {
                return RedirectToAction("All", "Restaurant", new { city });
            }

            return View();
        }

        public IActionResult Error(int statusCode)
        {
            if (statusCode == 404 || statusCode == 400)
            {
                return View("Error404");
            }

            if (statusCode == 401)
            {
                return View("Error401");
            }

            return View();
        }
    }
}