namespace GustoExpress.Web.Controllers
{
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    using GustoExpress.Services.Data.Contracts;
    using GustoExpress.Web.ViewModels;

    [Authorize]
    public class OrderItemController : BaseController
    {
        private readonly IOrderItemService _orderItemService;
        private readonly IProductService _productService;
        private readonly IOfferService _offerService;

        public OrderItemController(IOrderItemService orderItemService, 
            IProductService productService,
            IOfferService offerService)
        {
            _orderItemService = orderItemService;
            _productService = productService;
            _offerService = offerService;

        }

        [HttpGet]
        public async Task<IActionResult> CreateItem(string id)
        {
            try
            {
                var @object = await _orderItemService.GetObjectAsync(id);
                CreateOrderItemViewModel model = _orderItemService.GetOrderItemViewModel(@object);

                return View(model);
            }
            catch (Exception)
            {
                return GeneralError();
            }
        }

        [HttpPost]
        public async Task<IActionResult> CreateItem(CreateOrderItemViewModel obj)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    OrderItemViewModel model = await _orderItemService.CreateOrderItemAsync(obj);

                    return RedirectToAction("AddItemToOrder", "Order", new { id = model.Id.ToString() });
                }

                return View(obj);
            }
            catch (Exception)
            {
                return GeneralError();
            }
        }
    }
}
