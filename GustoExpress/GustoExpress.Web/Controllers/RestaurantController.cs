using GustoExpress.Data.Models;
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
        private readonly IWebHostEnvironment _webHostEnvironment;

        public RestaurantController(IRestaurantService restaurantService,
            IWebHostEnvironment webHostEnvironment)
        {
            _restaurantService = restaurantService;
            _webHostEnvironment = webHostEnvironment;
        }

        [HttpGet]
        public async Task<IActionResult> All(string city)
        {
            var restaurants = await _restaurantService.All(city);
            return View(restaurants);
        }

        [HttpGet]
        public async Task<IActionResult> RestaurantPage(string id)
        {
            var restaurant = await _restaurantService.GetByIdAsync(id);
            var restaurantViewModel = _restaurantService.ProjectTo<RestaurantPageViewModel>(restaurant);

            return View(restaurantViewModel);
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public IActionResult CreateRestaurant()
        {
            return View();
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> CreateRestaurant(IFormFile? file, CreateRestaurantViewModel obj)
        {
            if (ModelState.IsValid)
            {
                Restaurant restaurant = await _restaurantService.CreateAsync(obj);

                if (file != null)
                    await SaveImage(file, restaurant);

                return RedirectToAction("Index", "Home");
            }

            return View(obj);
        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteRestaurant(string id)
        {
            await _restaurantService.DeleteAsync(id);

            return RedirectToAction("Index", "Home");
        }

        private async Task SaveImage(IFormFile file, Restaurant restaurant)
        {
            string wwwRootPath = _webHostEnvironment.WebRootPath;

            string fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
            string imagePath = Path.Combine(wwwRootPath, @"images/Restaurants");

            using (var fileStream = new FileStream(Path.Combine(imagePath, fileName), FileMode.Create))
            {
                await file.CopyToAsync(fileStream);
            }

            string imageURL = @"/images/Restaurants/" + fileName;
            await _restaurantService.SaveImageURL(imageURL, restaurant);
        }
    }
}
