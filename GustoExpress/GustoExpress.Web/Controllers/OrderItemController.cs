using GustoExpress.Data.Models;
using GustoExpress.Services.Data.Contracts;
using GustoExpress.Web.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GustoExpress.Web.Controllers
{
    [Authorize]
    public class OrderItemController : BaseController
    {
        private readonly IOrderItemService _orderItemService;

        public OrderItemController(IOrderItemService orderItemService)
        {
            _orderItemService = orderItemService;
        }

        [HttpGet]
        public async Task<IActionResult> CreateItem(string id)
        {
            var @object = await _orderItemService.GetObjectAsync(id);
            CreateOrderItemViewModel model = _orderItemService.GetOrderItemViewModel(@object);

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> CreateItem(CreateOrderItemViewModel obj)
        {
            if (ModelState.IsValid)
            {

            }

            return View(obj);
        }
    }
}
