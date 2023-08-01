namespace GustoExpress.Web.Controllers
{
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    using GustoExpress.Services.Data.Contracts;
    using GustoExpress.Services.Data.Helpers.Restaurant;
    using GustoExpress.Web.ViewModels;

    [Authorize]
    public class RestaurantController : BaseController
    {
        private readonly IRestaurantService _restaurantService;
        private readonly IOrderService _orderService;

        public RestaurantController(IRestaurantService restaurantService,
            IOrderService orderService)
        {
            _restaurantService = restaurantService;
            _orderService = orderService;
        }

        [HttpGet]
        public async Task<IActionResult> All(string city, [FromQuery] AllRestaurantViewModel? obj)
        {
            AllRestaurantViewModel restaurants = await _restaurantService.AllAsync(city, obj);
            restaurants.SortingItems = RestaurantHelper.GetRestaurantSortingValues();

            return View(restaurants);
        }

        [HttpGet]
        public async Task<IActionResult> RestaurantPage(string id)
        {
            if (!await _restaurantService.HasRestaurantWithId(id))
            {
                return GeneralError();
            }

            try
            {
                RestaurantPageViewModel model = await _restaurantService.ProjectToModel<RestaurantPageViewModel>(id);
                model.Order = await _orderService.GetUserOrderAsync(GetUserId(), id);

                return View(model);
            }
            catch (Exception)
            {
                return GeneralError();
            }
        }
    }
}
