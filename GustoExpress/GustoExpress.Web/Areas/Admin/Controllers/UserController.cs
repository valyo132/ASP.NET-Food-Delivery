namespace GustoExpress.Web.Areas.Admin.Controllers
{
    using Microsoft.AspNetCore.Mvc;

    using GustoExpress.Services.Data.Contracts;
    using GustoExpress.Web.ViewModels;

    public class UserController : BaseAdminController
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpGet]
        public async Task<IActionResult> All()
        {
            List<UserViewModel> users = await _userService.AllUsersAsync();

            return View(users);
        }
    }
}
