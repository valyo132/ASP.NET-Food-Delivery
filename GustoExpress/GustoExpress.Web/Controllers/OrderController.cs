namespace GustoExpress.Web.Controllers
{
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    using GustoExpress.Services.Data.Contracts;
    using GustoExpress.Web.ViewModels;

    [Authorize]
    public class OrderController : BaseController
    {
        private readonly IOrderService _orderService;
        private readonly IOrderItemService _orderItemService;
        private readonly IRestaurantService _restaurantService;

        public OrderController(IOrderService orderService,
            IOrderItemService orderItemService,
            IRestaurantService restaurantService)
        {
            _orderService = orderService;
            _orderItemService = orderItemService;
            _restaurantService = restaurantService;

        }

        public async Task<IActionResult> AddItemToOrder(string id)
        {
            if (!await _orderItemService.HasOrderItemWithId(id))
            {
                return GeneralError();
            }

            try
            {
                string userId = GetUserId();
                OrderViewModel model = await _orderService.AddItemToOrder(userId, id);

                TempData["success"] = "An item was added to your order!";
                return RedirectToAction("RestaurantPage", "Restaurant", new { id = model.RestaurantId });
            }
            catch (InvalidOperationException ioe)
            {
                TempData["danger"] = ioe.Message;
                string restaurantId = await _orderItemService.GetRestaurantIdAsync(id);

                return RedirectToAction("RestaurantPage", "Restaurant", new { id = restaurantId });
            }
            catch (Exception)
            {
                return GeneralError();
            }
        }

        [HttpGet]
        public async Task<IActionResult> CompleteOrder(string id)
        {
            if (!await _restaurantService.HasRestaurantWithId(id))
            {
                return GeneralError();
            }

            try
            {
                string userId = GetUserId();
                OrderViewModel model = await _orderService.GetOrderToComplete(userId, id);
                await _orderService.CompleteOrder(userId, id);

                TempData["success"] = "Order completed!";
                return RedirectToAction("GetOrderDetails", "Order", new { orderId = model.Id });
            }
            catch (InvalidOperationException ioe)
            {
                TempData["danger"] = ioe.Message;
                return RedirectToAction("RestaurantPage", "Restaurant", new { id = id });
            }
            catch (Exception)
            {
                return GeneralError();
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetOrderDetails(string orderId)
        {
            if (!await _orderService.HasOrderWithId(orderId))
            {
                return GeneralError();
            }

            try
            {
                OrderViewModel model = await _orderService.GetOrderDetails(orderId);

                return View(model);
            }
            catch (Exception)
            {
                return GeneralError();
            }
        }
    }
}
