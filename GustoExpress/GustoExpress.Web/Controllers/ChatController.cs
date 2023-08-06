namespace GustoExpress.Web.Controllers
{
    using GustoExpress.Services.Data.Contracts;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    [Authorize]
    public class ChatController : BaseController
    {
        private readonly IUserService _userService;

        public ChatController(IUserService userService)
        {
            _userService = userService;
        }

        public async Task<IActionResult> Chat(string? user)
        {
            if (user != null)
            {
                user = await _userService.GetUserEmailByUsername(user);
            }

            return View("Chat", user);
        }
    }
}
