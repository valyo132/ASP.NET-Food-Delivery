namespace GustoExpress.Web.Controllers.Tests
{
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.ViewFeatures;

    using GustoExpress.Data.Models;
    using GustoExpress.Services.Data.Contracts;
    using GustoExpress.Web.ViewModels;

    using Moq;

    [TestFixture]
    public class OrderItemControllerTests
    {
        private OrderItemController controller;

        private Mock<IOrderItemService> _orderItemService;
        private Mock<IProductService> _productService;
        private Mock<IOfferService> _offerService;

        [SetUp]
        public void Setup()
        {
            _orderItemService = new Mock<IOrderItemService>();
            _productService = new Mock<IProductService>();
            _offerService = new Mock<IOfferService>();

            controller = new OrderItemController(_orderItemService.Object, _productService.Object, _offerService.Object);

            controller.TempData = new TempDataDictionary(
                new DefaultHttpContext(),
                Mock.Of<ITempDataProvider>());
        }

        [Test]
        public async Task Test_CreateItemOnGet_ShouldWork()
        {
            string itemId = "3fdf619b-88a1-4e75-aa62-975ebfd746ac";

            var expectedModel = new CreateOrderItemViewModel()
            {
                TotalCost = 1000,
            };

            _orderItemService
                .Setup(oi => oi.GetObjectAsync(itemId))
                .ReturnsAsync(new Product());

            _orderItemService
                .Setup(oi => oi.GetOrderItemViewModel(new Product()))
                .Returns(expectedModel);

            var resut = await controller.CreateItem(itemId);
            Assert.IsNotNull(resut);
            Assert.That(resut, Is.InstanceOf<ViewResult>());
        }

        [Test]
        public async Task Test_CreateItemOnGet_ShouldThrowException()
        {
            _orderItemService
                .Setup(x => x.GetObjectAsync(It.IsAny<string>()))
                .ThrowsAsync(new Exception());

            var result = await controller.CreateItem("someid");

            var redirectResult = (RedirectToActionResult)result;
            Assert.That(result, Is.InstanceOf<IActionResult>());
            Assert.That(redirectResult.ControllerName, Is.EqualTo("Home"));
            Assert.That(redirectResult.ActionName, Is.EqualTo("Index"));
        }

        [Test]
        public async Task Test_CreateItemOnPost_ShouldWork()
        {
            var model = new CreateOrderItemViewModel()
            {
                TotalCost = 1000,
            };

            var expectedModel = new OrderItemViewModel()
            {
                Id = Guid.Parse("78ecea93-d576-45d3-aa1c-d654834a69e2"),
                TotalCost = 1,
                Quantity = 3
            };

            _orderItemService
                .Setup(oi => oi.CreateOrderItemAsync(model))
                .ReturnsAsync(expectedModel);

            var result = await controller.CreateItem(model);

            var redirectResult = (RedirectToActionResult)result;
            Assert.That(result, Is.InstanceOf<IActionResult>());
            Assert.That(redirectResult.ControllerName, Is.EqualTo("Order"));
            Assert.That(redirectResult.ActionName, Is.EqualTo("AddItemToOrder"));
        }

        [Test]
        public async Task Test_CreateItemOnPost_ShouldReturnItsViewWhenModelStateIsInvalid()
        {
            var model = new CreateOrderItemViewModel()
            {
                TotalCost = 1000,
            };

            controller.ModelState.AddModelError("test", "invalid");

            var result = await controller.CreateItem(model);

            Assert.That(result, Is.InstanceOf<ViewResult>());
        }

        [Test]
        public async Task Test_CreateItemOnPost_ShouldThrowException()
        {
            var model = new CreateOrderItemViewModel()
            {
                TotalCost = 1000,
            };

            _orderItemService
                .Setup(oi => oi.CreateOrderItemAsync(model))
                .ThrowsAsync(new Exception());

            var result = await controller.CreateItem(model);

            var redirectResult = (RedirectToActionResult)result;
            Assert.That(result, Is.InstanceOf<IActionResult>());
            Assert.That(redirectResult.ControllerName, Is.EqualTo("Home"));
            Assert.That(redirectResult.ActionName, Is.EqualTo("Index"));
        }
    }
}
