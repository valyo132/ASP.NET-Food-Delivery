namespace GustoExpress.Web.Areas.Admin.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Authorization;

    using static GustoExpress.Web.Common.GeneralConstraints;

    [Area(ADMIN_AREA_NAME)]
    [Authorize(Roles = "Admin")]
    public class BaseAdminController : Controller
    {
        public IActionResult GeneralError()
        {
            TempData["danger"] = "Unexpected error occurred! Please try again later or contact administrator";

            return RedirectToAction("Index", "Home");
        }
    }
}
