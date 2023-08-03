namespace GustoExpress.Web.Controllers.Tests
{
    using System.Security.Claims;

    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.ViewFeatures;

    using GustoExpress.Services.Data.Contracts;
    using GustoExpress.Web.ViewModels;

    using Moq;

    public class OrderControllerTests
    {
        private OrderController controller;

        private Mock<IOrderService> _orderService;
        private Mock<IOrderItemService> _orderItemService;
        private Mock<IRestaurantService> _restaurantService;

        private string userId;

        [SetUp]
        public void SetUp()
        {
            userId = "cfca17a3-c01b-4672-b99b-334727fbef64";

            _orderService = new Mock<IOrderService>();
            _orderItemService = new Mock<IOrderItemService>();
            _restaurantService = new Mock<IRestaurantService>();

            controller = new OrderController(_orderService.Object, _orderItemService.Object, _restaurantService.Object)
            {
                ControllerContext = new ControllerContext()
                {
                    HttpContext = new DefaultHttpContext()
                    {
                        User = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
                        {
                            new Claim(ClaimTypes.NameIdentifier, userId)
                        }))
                    }
                }
            };

            controller.TempData = new TempDataDictionary(
                new DefaultHttpContext(),
                Mock.Of<ITempDataProvider>());
        }

        [Test]
        public async Task Test_AddItemToOrder_ShouldWorkAndShouldRedirectToTheCorrectRoute()
        {
            string orderItemId = "42829594-97d0-40bf-8ce6-7919a872b67e";
            string restaurantId = "0734796c-760b-44ea-8ca4-73bdec2324d3";

            var expectedModel = new OrderViewModel() { RestaurantId = Guid.Parse(restaurantId), TotalCost = 100, UserId = userId, Id = Guid.Parse("dd867152-3cd8-4817-b824-18fa46695be9") };

            _orderItemService
                .Setup(x => x.HasOrderItemWithId(orderItemId))
                .ReturnsAsync(true);

            _orderService
                .Setup(x => x.AddItemToOrder(userId, orderItemId))
                .ReturnsAsync(expectedModel);

            var resultTask = controller.AddItemToOrder(orderItemId);
            var result = await resultTask;

            var redirectResult = (RedirectToActionResult)result;
            Assert.That(redirectResult.ControllerName, Is.EqualTo("Restaurant"));
            Assert.That(redirectResult.ActionName, Is.EqualTo("RestaurantPage"));
            Assert.That(redirectResult.RouteValues["id"].ToString(), Is.EqualTo(restaurantId));
            Assert.That(result, Is.InstanceOf<IActionResult>());

            _orderItemService.Verify(x => x.HasOrderItemWithId(orderItemId), Times.Once);
            _orderService.Verify(x => x.AddItemToOrder(userId, orderItemId), Times.Once);

            Assert.That(controller.TempData["success"], Is.EqualTo("An item was added to your order!"));
        }

        [Test]
        public async Task Test_AddItemToOrder_ShouldReturnGeneralError()
        {
            string orderItemId = "42829594-97d0-40bf-8ce6-7919a872b67e";

            _orderItemService
                .Setup(x => x.HasOrderItemWithId(orderItemId))
                .ReturnsAsync(false);

            var result = await controller.AddItemToOrder(orderItemId);

            var redirectResult = (RedirectToActionResult)result;
            Assert.That(result, Is.InstanceOf<IActionResult>());
            Assert.That(redirectResult.ControllerName, Is.EqualTo("Home"));
            Assert.That(redirectResult.ActionName, Is.EqualTo("Index"));

            Assert.That(controller.TempData["danger"], Is.EqualTo("Unexpected error occurred! Please try again later or contact administrator"));
        }

        [Test]
        public async Task Test_AddItemToOrder_ShouldThrowInvalidOperationException()
        {
            string orderItemId = "42829594-97d0-40bf-8ce6-7919a872b67e";
            string restaurantId = "0734796c-760b-44ea-8ca4-73bdec2324d3";
            string exceptionMessage = "This item was aleady added to your order!";

            _orderItemService
                .Setup(x => x.HasOrderItemWithId(orderItemId))
                .ReturnsAsync(true);

            _orderService
                .Setup(o => o.AddItemToOrder(It.IsAny<string>(), It.IsAny<string>()))
                .ThrowsAsync(new InvalidOperationException(exceptionMessage));

            _orderItemService
                .Setup(r => r.GetRestaurantIdAsync(orderItemId))
                .ReturnsAsync(restaurantId);

            var result = await controller.AddItemToOrder(orderItemId);

            var redirectResult = (RedirectToActionResult)result;
            Assert.That(result, Is.InstanceOf<IActionResult>());
            Assert.That(redirectResult.ControllerName, Is.EqualTo("Restaurant"));
            Assert.That(redirectResult.ActionName, Is.EqualTo("RestaurantPage"));

            Assert.That(controller.TempData["danger"], Is.EqualTo(exceptionMessage));
        }

        [Test]
        public async Task Test_AddItemToOrder_ShouldThrowException()
        {
            string orderItemId = "42829594-97d0-40bf-8ce6-7919a872b67e";
            string restaurantId = "0734796c-760b-44ea-8ca4-73bdec2324d3";

            _orderItemService
                .Setup(x => x.HasOrderItemWithId(orderItemId))
                .ReturnsAsync(true);

            _orderService
                .Setup(o => o.AddItemToOrder(It.IsAny<string>(), It.IsAny<string>()))
                .ThrowsAsync(new Exception());

            _orderItemService
                .Setup(r => r.GetRestaurantIdAsync(orderItemId))
                .ReturnsAsync(restaurantId);

            var result = await controller.AddItemToOrder(orderItemId);

            var redirectResult = (RedirectToActionResult)result;
            Assert.That(result, Is.InstanceOf<IActionResult>());
            Assert.That(redirectResult.ControllerName, Is.EqualTo("Home"));
            Assert.That(redirectResult.ActionName, Is.EqualTo("Index"));
        }

        [Test]
        public async Task Test_CompleteOrder_ShouldReturnGeneralError_WhenRestaurantDontExist()
        {
            string restaurantId = "0734796c-760b-44ea-8ca4-73bdec2324d3";
            string orderId = "6e631b4d-7669-4b07-9ba2-76320b9a7116";

            _restaurantService
                .Setup(r => r.HasRestaurantWithId(restaurantId))
                .ReturnsAsync(false);

            var result = await controller.CompleteOrder(orderId);

            var redirectResult = (RedirectToActionResult)result;
            Assert.That(result, Is.InstanceOf<IActionResult>());
            Assert.That(redirectResult.ControllerName, Is.EqualTo("Home"));
            Assert.That(redirectResult.ActionName, Is.EqualTo("Index"));

            Assert.That(controller.TempData["danger"], Is.EqualTo("Unexpected error occurred! Please try again later or contact administrator"));
        }

        [Test]
        public async Task Test_CompleteOrder_ShouldWork()
        {
            string restaurantId = "0734796c-760b-44ea-8ca4-73bdec2324d3";
            string orderId = "6e631b4d-7669-4b07-9ba2-76320b9a7116";

            var expectedModel = new OrderViewModel()
            {
                Id = Guid.Parse(orderId),
                UserId = userId,
                RestaurantId = Guid.Parse(restaurantId),
                TotalCost = 1000
            };

            _restaurantService
                .Setup(r => r.HasRestaurantWithId(restaurantId))
                .ReturnsAsync(true);

            _orderService
                .Setup(o => o.GetOrderToComplete(userId, restaurantId))
                .ReturnsAsync(expectedModel);

            _orderService
                .Setup(o => o.CompleteOrder(userId, orderId));

            var result = await controller.CompleteOrder(restaurantId);

            var redeirectResult = (RedirectToActionResult)result;
            Assert.That(result, Is.InstanceOf<IActionResult>());
            Assert.That(redeirectResult.ControllerName, Is.EqualTo("Order"));
            Assert.That(redeirectResult.ActionName, Is.EqualTo("GetOrderDetails"));
            Assert.That(redeirectResult.RouteValues["orderId"].ToString(), Is.EqualTo(orderId));

            Assert.That(controller.TempData["success"], Is.EqualTo("Order completed!"));
        }

        [Test]
        public async Task Test_CompleteOrder_ShouldThrowInvalidOperationException()
        {
            string restaurantId = "0734796c-760b-44ea-8ca4-73bdec2324d3";
            string exceptionMessage = "You don't have any item in your order yet!";

            _restaurantService
                .Setup(r => r.HasRestaurantWithId(restaurantId))
                .ReturnsAsync(true);

            _orderService
                .Setup(o => o.GetOrderToComplete(userId, restaurantId))
                .ThrowsAsync(new InvalidOperationException(exceptionMessage));

            var result = await controller.CompleteOrder(restaurantId);

            var redirectResult = (RedirectToActionResult)result;
            Assert.That(result, Is.InstanceOf<IActionResult>());
            Assert.That(redirectResult.ControllerName, Is.EqualTo("Restaurant"));
            Assert.That(redirectResult.ActionName, Is.EqualTo("RestaurantPage"));

            Assert.That(controller.TempData["danger"], Is.EqualTo(exceptionMessage));
        }

        [Test]
        public async Task Test_CompleteOrder_ShouldThrowException()
        {
            string restaurantId = "0734796c-760b-44ea-8ca4-73bdec2324d3";

            _restaurantService
                .Setup(r => r.HasRestaurantWithId(restaurantId))
                .ReturnsAsync(true);

            _orderService
                .Setup(o => o.GetOrderToComplete(userId, restaurantId))
                .ThrowsAsync(new Exception());

            var result = await controller.CompleteOrder(restaurantId);

            var redirectResult = (RedirectToActionResult)result;
            Assert.That(result, Is.InstanceOf<IActionResult>());
            Assert.That(redirectResult.ControllerName, Is.EqualTo("Home"));
            Assert.That(redirectResult.ActionName, Is.EqualTo("Index"));
        }

        [Test]
        public async Task Test_GetOrderDetails_ShouldReturnGeneralErrorForNonExistingOrder()
        {
            string orderId = "6e631b4d-7669-4b07-9ba2-76320b9a7116";

            _orderService
                .Setup(o => o.HasOrderWithId(orderId))
                .ReturnsAsync(false);

            var result = await controller.GetOrderDetails(orderId);

            var redirectResult = (RedirectToActionResult)result;
            Assert.That(result, Is.InstanceOf<IActionResult>());
            Assert.That(redirectResult.ControllerName, Is.EqualTo("Home"));
            Assert.That(redirectResult.ActionName, Is.EqualTo("Index"));
        }

        [Test]
        public async Task Test_GetOrderDetails_ShouldWork()
        {
            string restaurantId = "0734796c-760b-44ea-8ca4-73bdec2324d3";
            string orderId = "6e631b4d-7669-4b07-9ba2-76320b9a7116";

            var expectedModel = new OrderViewModel()
            {
                Id = Guid.Parse(orderId),
                UserId = userId,
                RestaurantId = Guid.Parse(restaurantId),
                TotalCost = 1000
            };

            _orderService
                .Setup(o => o.HasOrderWithId(orderId))
                .ReturnsAsync(true);

            _orderService
                .Setup(o => o.GetOrderDetails(orderId))
                .ReturnsAsync(expectedModel);

            var result = await controller.GetOrderDetails(orderId);

            Assert.That(result, Is.AssignableTo<ViewResult>());
        }

        [Test]
        public async Task Test_GetOrderDetail_ShouldThrowException()
        {
            string orderId = "6e631b4d-7669-4b07-9ba2-76320b9a7116";

            _orderService
                .Setup(o => o.HasOrderWithId(orderId))
                .ReturnsAsync(true);

            _orderService
                .Setup(o => o.GetOrderDetails(orderId))
                .ThrowsAsync(new Exception());

            var result = await controller.GetOrderDetails(orderId);

            var redirectResult = (RedirectToActionResult)result;
            Assert.That(result, Is.InstanceOf<IActionResult>());
            Assert.That(redirectResult.ControllerName, Is.EqualTo("Home"));
            Assert.That(redirectResult.ActionName, Is.EqualTo("Index"));
        }
    }
}
