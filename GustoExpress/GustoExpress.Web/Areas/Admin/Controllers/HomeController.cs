using Microsoft.AspNetCore.Mvc;

namespace GustoExpress.Web.Areas.Admin.Controllers
{
    public class HomeController : BaseAdminController
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
