namespace GustoExpress.Web.Areas.Admin.Controllers
{
    using Microsoft.AspNetCore.Mvc; 

    using GustoExpress.Services.Data.Contracts;
    using GustoExpress.Services.Data.Helpers;
    using GustoExpress.Web.ViewModels;

    using static GustoExpress.Web.Common.GeneralConstraints;

    public class RestaurantController : BaseAdminController
    {
        private readonly IRestaurantService _restaurantService;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public RestaurantController(IRestaurantService restaurantService,
            IWebHostEnvironment webHostEnvironment)
        {
            _restaurantService = restaurantService;
            _webHostEnvironment = webHostEnvironment;
        }

        public async Task<IActionResult> All()
        {
            var allRestaurants = await _restaurantService.AllWithoutCityAsync();

            return View(allRestaurants);
        }

        [HttpGet]
        public IActionResult CreateRestaurant()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateRestaurant(IFormFile? file, CreateRestaurantViewModel obj)
        {
            if (obj.MinTimeToDeliver > obj.MaxTimeToDeliver)
            {
                ModelState.AddModelError("Invalid operation", "Invalid operation - Time to deliver");
            }

            if (ModelState.IsValid)
            {
                RestaurantViewModel restaurant = await _restaurantService.CreateAsync(obj);

                if (file != null)
                    await SaveImage(file, restaurant);

                TempData["success"] = "Successfully created restaurant!";
                return RedirectToAction("RestaurantPage", "Restaurant", new { id = restaurant.Id, Area = "" });
            }

            TempData["danger"] = "Invalid operation!";
            return View(obj);
        }

        [HttpGet]
        public async Task<IActionResult> EditRestaurant(string id)
        {
            if (!await _restaurantService.HasRestaurantWithId(id))
            {
                return GeneralError();
            }

            CreateRestaurantViewModel model = await _restaurantService.ProjectToModel<CreateRestaurantViewModel>(id);

            return View(model);
        }

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
                return RedirectToAction("RestaurantPage", "Restaurant", new { id = restaurant.Id, Area = "" });
            }

            return View(obj);
        }

        public async Task<IActionResult> DeleteRestaurant(string id)
        {
            RestaurantViewModel restaurant = await _restaurantService.DeleteAsync(id);

            TempData["success"] = "Successfully deleted restaurant!";
            return RedirectToAction("All", "Restaurant", new { city = restaurant.City.CityName, Area = ADMIN_AREA_NAME });
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

            FileHelper.DeleteImage(imagePath);
        }
    }
}
