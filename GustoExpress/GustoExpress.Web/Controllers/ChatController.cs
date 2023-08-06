namespace GustoExpress.Web.Controllers
{
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    [Authorize]
    public class ChatController : BaseController
    {
        public IActionResult Chat(string email)
        {
            return View("Chat", email);
        }
    }
}
