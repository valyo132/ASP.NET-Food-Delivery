using GustoExpress.Services.Data.Contracts;
using GustoExpress.Web.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GustoExpress.Web.Controllers
{
    [Authorize]
    public class RestaurantController : Controller
    {
        private readonly IRestaurantService _restaurantService;

        public RestaurantController(IRestaurantService restaurantService)
        {
            _restaurantService = restaurantService;
        }

        public async Task<IActionResult> All(string city)
        {
            var restaurants = await _restaurantService.All(city);
            return View(restaurants);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateRestaurantViewModel obj)
        {
            if (ModelState.IsValid)
            {
                await _restaurantService.Create(obj);

                return RedirectToAction("Index", "Home");
            }

            return View(obj);
        }
    }
}
