namespace GustoExpress.Web.Controllers.Tests
{
    using System.Security.Claims;

    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.ViewFeatures;

    using GustoExpress.Data.Models;
    using GustoExpress.Services.Data.Contracts;
    using GustoExpress.Web.ViewModels;

    using Moq;

    [TestFixture]
    public class RestaurantControllerTests
    {
        private RestaurantController controller;

        private Mock<IRestaurantService> _restaurantService;
        private Mock<IOrderService> _orderService;

        private string generalErrorMessage = "Unexpected error occurred! Please try again later or contact administrator";
        private string userId = "0cb9b955-9152-42fd-a899-84b4d6f89c21";

        [SetUp]
        public void SetUp()
        {
            _restaurantService = new Mock<IRestaurantService>();
            _orderService = new Mock<IOrderService>();

            controller = new RestaurantController(_restaurantService.Object, _orderService.Object)
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
        public async Task Test_All_ShouldWork()
        {
            string city = "Test";

            var model = new AllRestaurantViewModel()
            {
                CityName = city,
                Sort = ViewModels.Enums.Restaurant.RestaurantSorting.DeliveryPriceAscending,
                SearchString = "Test"
            };

            _restaurantService
                .Setup(r => r.AllAsync(city, model))
                .ReturnsAsync(model);

            var result = await controller.All(city, model);

            Assert.That(result, Is.InstanceOf<ViewResult>());
        }

        [Test]
        public async Task Test_RestaurantPage_ShouldReturnGeneralError()
        {
            string restaurantId = "a75df743-dc55-41c4-831d-ec7dae6eb002";

            _restaurantService
                .Setup(r => r.HasRestaurantWithId(restaurantId))
                .ReturnsAsync(false);

            var result = await controller.RestaurantPage(restaurantId);

            var redirectResult = (RedirectToActionResult)result;
            Assert.That(result, Is.InstanceOf<IActionResult>());
            Assert.That(redirectResult.ControllerName, Is.EqualTo("Home"));
            Assert.That(redirectResult.ActionName, Is.EqualTo("Index"));

            Assert.That(controller.TempData["danger"], Is.EqualTo(generalErrorMessage));
        }

        [Test]
        public async Task Test_RestaurantPage_ShouldWork()
        {
            string restaurantId = "a75df743-dc55-41c4-831d-ec7dae6eb002";

            var expectedModel = new RestaurantPageViewModel()
            {
                Id = Guid.Parse("6fe56d67-ec2e-4d0a-98f0-ca84e9061158"),
                Name = "Test",
                Description = "Test",
            };

            var order = new Order()
            {
                RestaurantId = Guid.Parse(restaurantId)
            };

            _restaurantService
                .Setup(r => r.HasRestaurantWithId(restaurantId))
                .ReturnsAsync(true);

            _restaurantService
                .Setup(r => r.ProjectToModel<RestaurantPageViewModel>(restaurantId))
                .ReturnsAsync(expectedModel);

            _orderService
                .Setup(r => r.GetUserOrderAsync(userId, restaurantId))
                .ReturnsAsync(order);

            var result = await controller.RestaurantPage(restaurantId);

            Assert.That(result, Is.InstanceOf<ViewResult>());
        }

        [Test]
        public async Task Test_RestaurantPage_ShouldThrowException()
        {
            string restaurantId = "a75df743-dc55-41c4-831d-ec7dae6eb002";

            _restaurantService
                .Setup(r => r.HasRestaurantWithId(restaurantId))
                .ReturnsAsync(true);

            _restaurantService
               .Setup(r => r.ProjectToModel<RestaurantPageViewModel>(restaurantId))
               .ThrowsAsync(new Exception());

            var result = await controller.RestaurantPage(restaurantId);

            var redirectResult = (RedirectToActionResult)result;
            Assert.That(result, Is.InstanceOf<IActionResult>());
            Assert.That(redirectResult.ControllerName, Is.EqualTo("Home"));
            Assert.That(redirectResult.ActionName, Is.EqualTo("Index"));

            Assert.That(controller.TempData["danger"], Is.EqualTo(generalErrorMessage));
        }
    }
}
