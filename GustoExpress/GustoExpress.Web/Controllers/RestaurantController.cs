namespace GustoExpress.Web.Controllers
{
    using GustoExpress.Services.Data.Contracts;
    using GustoExpress.Web.ViewModels;

    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    [Authorize]
    public class RestaurantController : BaseController
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
            List<AllRestaurantViewModel> restaurants = await _restaurantService.AllAsync(city);
            return View(restaurants);
        }

        [HttpGet]
        public async Task<IActionResult> RestaurantPage(string id)
        {
            RestaurantPageViewModel model = await _restaurantService.ProjectToModel<RestaurantPageViewModel>(id);

            return View(model);
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
                RestaurantViewModel restaurant = await _restaurantService.CreateAsync(obj);

                if (file != null)
                    await SaveImage(file, restaurant);

                TempData["success"] = "Successfully created restaurant!";
                return RedirectToAction("RestaurantPage", "Restaurant", new { id = restaurant.Id });
            }

            return View(obj);
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> EditRestaurant(string id)
        {
            CreateRestaurantViewModel model = await _restaurantService.ProjectToModel<CreateRestaurantViewModel>(id);

            return View(model);
        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> EditRestaurant(IFormFile? file, string id, CreateRestaurantViewModel obj)
        {
            if (ModelState.IsValid)
            {
                RestaurantViewModel restaurant = await _restaurantService.EditRestaurantAsync(id, obj);

                if (file != null)
                {
                    if (restaurant.ImageURL != null)
                        DeleteImage(restaurant.ImageURL);

                    await SaveImage(file, restaurant);
                }

                TempData["success"] = "Successfully updated restaurant!";
                return RedirectToAction("RestaurantPage", "Restaurant", new { id = restaurant.Id });
            }

            return View(obj);
        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteRestaurant(string id)
        {
            RestaurantViewModel restaurant = await _restaurantService.DeleteAsync(id);

            TempData["success"] = "Successfully deleted restaurant!";
            return RedirectToAction("All", "Restaurant", new { city = restaurant.City.CityName });
        }

        private async Task SaveImage(IFormFile file, RestaurantViewModel restaurant)
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
        private void DeleteImage(string file)
        {
            string wwwRootPath = _webHostEnvironment.WebRootPath;

            string imagePath = wwwRootPath + file;

            FileInfo fileInfo = new FileInfo(imagePath);
            if (fileInfo != null)
            {
                System.IO.File.Delete(imagePath);
                fileInfo.Delete();
            }
        }
    }
}
