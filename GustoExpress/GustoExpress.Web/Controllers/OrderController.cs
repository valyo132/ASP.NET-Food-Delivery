namespace GustoExpress.Web.Controllers
{
    using GustoExpress.Services.Data.Contracts;
    using GustoExpress.Web.ViewModels;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    [Authorize]
    public class OrderController : BaseController
    {
        private readonly IOrderService _orderService;

        public OrderController(IOrderService orderService)
        {
            _orderService = orderService;
        }

        public async Task<IActionResult> AddItemToOrder(string id)
        {
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
                return RedirectToAction("Index", "Home");
            }
        }

        [HttpGet]
        public async Task<IActionResult> CompleteOrder(string id)
        {
            try
            {
                string userId = GetUserId();
                OrderViewModel model = await _orderService.GetOrderToComplete(userId, id);
                await _orderService.CompleteOrder(userId, id);

                TempData["success"] = "Order completed!";
                return RedirectToAction("RestaurantPage", "Restaurant", new { id = id });
            }
            catch (InvalidOperationException ioe)
            {
                TempData["danger"] = ioe.Message;
                return RedirectToAction("RestaurantPage", "Restaurant", new { id = id });
            }
        }
    }
}
