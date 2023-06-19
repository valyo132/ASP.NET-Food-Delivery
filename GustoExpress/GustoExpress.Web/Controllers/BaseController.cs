using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace GustoExpress.Web.Controllers
{
    public class BaseController : Controller
    {
        public string GetUserIdAsync()
        {
            return this.User.FindFirstValue(ClaimTypes.NameIdentifier);
        }
    }
}
