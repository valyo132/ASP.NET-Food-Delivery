namespace GustoExpress.Web.Controllers.Tests.Admin_area
{
    using System.Security.Claims;

    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.ViewFeatures;

    using GustoExpress.Data.Models;
    using GustoExpress.Services.Data.Contracts;
    using GustoExpress.Web.ViewModels;

    using Moq;

    [TestFixture]
    public class AdminRestaurantControllerTests
    {
        private Areas.Admin.Controllers.RestaurantController controller;

        private Mock<IRestaurantService> _restaurantService;
        private Mock<IWebHostEnvironment> _iWebHostEnviroment;

        private string generalErrorMessage = "Unexpected error occurred! Please try again later or contact administrator";
        private string userId = "0cb9b955-9152-42fd-a899-84b4d6f89c21";

        [SetUp]
        public async Task SetUp()
        {
            _restaurantService = new Mock<IRestaurantService>();
            _iWebHostEnviroment = new Mock<IWebHostEnvironment>();

            controller = new Areas.Admin.Controllers.RestaurantController(_restaurantService.Object, _iWebHostEnviroment.Object)
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
            var expectedModel = new AllRestaurantViewModel()
            {
                SearchString = "Test",
                Sort = ViewModels.Enums.Restaurant.RestaurantSorting.DeliveryPriceDescending
            };

            _restaurantService
                .Setup(r => r.AllWithoutCityAsync())
                .ReturnsAsync(expectedModel);

            var result = await controller.All();

            Assert.That(result, Is.InstanceOf<ViewResult>());
        }

        [Test]
        public void Test_CreateRestaurantOnGet_ShouldWork()
        {
            var result = controller.CreateRestaurant();

            Assert.That(result, Is.InstanceOf<ViewResult>());
        }

        [Test]
        public async Task Test_CreateRestaurantOnPost_ShouldThrowInvalidOperationException()
        {
            var model = new CreateRestaurantViewModel()
            {
                MinTimeToDeliver = 30,
                MaxTimeToDeliver = 20,
            };

            var result = await controller.CreateRestaurant(null, model);

            Assert.That(result, Is.InstanceOf<ViewResult>());
            Assert.That(controller.TempData["danger"], Is.EqualTo("Invalid operation!"));
        }

        [Test]
        public async Task Test_CreateRestaurantOnPost_ShouldWorkAdnRedirectToTheCorrectRoute()
        {
            var model = new CreateRestaurantViewModel()
            {
                MinTimeToDeliver = 10,
                MaxTimeToDeliver = 20,
            };

            var expectedModel = new RestaurantViewModel()
            {
                Name = "Test",
            };

            _restaurantService
                .Setup(r => r.CreateAsync(model))
                .ReturnsAsync(expectedModel);

            var result = await controller.CreateRestaurant(null, model);

            var redirectResult = (RedirectToActionResult)result;
            Assert.That(result, Is.InstanceOf<IActionResult>());
            Assert.That(redirectResult.ControllerName, Is.EqualTo("Restaurant"));
            Assert.That(redirectResult.ActionName, Is.EqualTo("RestaurantPage"));

            Assert.That(controller.TempData["success"], Is.EqualTo("Successfully created restaurant!"));
        }

        [Test]
        public async Task Test_EditRestaurantOnGet_ShouldReturnGeneralError()
        {
            string restaurantId = "cc8e5fa8-95d4-4463-a031-2ad55f69ac52";

            _restaurantService
                .Setup(r => r.HasRestaurantWithId(restaurantId))
                .ReturnsAsync(false);

            var result = await controller.EditRestaurant(restaurantId);

            var redirectResult = (RedirectToActionResult)result;
            Assert.That(result, Is.InstanceOf<IActionResult>());
            Assert.That(redirectResult.ControllerName, Is.EqualTo("Home"));
            Assert.That(redirectResult.ActionName, Is.EqualTo("Index"));

            Assert.That(controller.TempData["danger"], Is.EqualTo(generalErrorMessage));
        }

        [Test]
        public async Task Test_EditRestaurantOnGet_ShouldWork()
        {
            string restaurantId = "cc8e5fa8-95d4-4463-a031-2ad55f69ac52";

            var expectedModel = new CreateRestaurantViewModel()
            {
                Name = "Test",
            };

            _restaurantService
                .Setup(r => r.HasRestaurantWithId(restaurantId))
                .ReturnsAsync(true);

            _restaurantService
                .Setup(r => r.ProjectToModel<CreateRestaurantViewModel>(restaurantId))
                .ReturnsAsync(expectedModel);

            var result = await controller.EditRestaurant(restaurantId);

            Assert.That(result, Is.InstanceOf<ViewResult>());
        }

        [Test]
        public async Task Test_EditRestaurantOnPost_ShouldWork()
        {
            string restaurantId = "cc8e5fa8-95d4-4463-a031-2ad55f69ac52";

            var model = new CreateRestaurantViewModel()
            {
                MinTimeToDeliver = 10,
                MaxTimeToDeliver = 20,
                Name = "Test",
                Description = "Test",
            };

            var expectedModel = new RestaurantViewModel()
            {
                Name = "Test",
                Description = "Test",
            };

            _restaurantService
                .Setup(r => r.EditRestaurantAsync(restaurantId, model))
                .ReturnsAsync(expectedModel);

            var result = await controller.EditRestaurant(null, restaurantId, model);

            var redirectResult = (RedirectToActionResult)result;
            Assert.That(result, Is.InstanceOf<IActionResult>());
            Assert.That(redirectResult.ControllerName, Is.EqualTo("Restaurant"));
            Assert.That(redirectResult.ActionName, Is.EqualTo("RestaurantPage"));

            Assert.That(controller.TempData["success"], Is.EqualTo("Successfully updated restaurant!"));
        }

        [Test]
        public async Task Test_EditRestaurantOnPost_ShouldReturnItsView()
        {
            string restaurantId = "cc8e5fa8-95d4-4463-a031-2ad55f69ac52";

            var model = new CreateRestaurantViewModel()
            {
                MinTimeToDeliver = 10,
                MaxTimeToDeliver = 20,
                Name = "Test",
                Description = "Test",
            };

            var expectedModel = new RestaurantViewModel()
            {
                Name = "Test",
                Description = "Test",
            };

            controller.ModelState.AddModelError("Test", "Invalid");

            _restaurantService
                .Setup(r => r.EditRestaurantAsync(restaurantId, model))
                .ReturnsAsync(expectedModel);

            var result = await controller.EditRestaurant(null, restaurantId, model);

            Assert.That(result, Is.InstanceOf<ViewResult>());
        }

        [Test]
        public async Task Test_DeleteRestaurant_ShouldWork()
        {
            string restaurantId = "cc8e5fa8-95d4-4463-a031-2ad55f69ac52";
            string cityName = "Test city";
            string adminArea = "Admin";

            var city = new City()
            {
                Id = Guid.Parse("27974f3e-8df3-43cb-bccb-5977f5f95fe4"),
                CityName = cityName,
            };

            var expectedModel = new RestaurantViewModel()
            {
                Name = "Test",
                Description = "Test",
                City = city,
            };

            _restaurantService
                .Setup(r => r.DeleteAsync(restaurantId))
                .ReturnsAsync(expectedModel);

            var result = await controller.DeleteRestaurant(restaurantId);

            var redirectResult = (RedirectToActionResult)result;

            Assert.That(redirectResult.ControllerName, Is.EqualTo("Restaurant"));
            Assert.That(redirectResult.ActionName, Is.EqualTo("All"));
            Assert.AreEqual(cityName, redirectResult.RouteValues["city"]);
            Assert.AreEqual(adminArea, redirectResult.RouteValues["Area"]);

            Assert.That(controller.TempData["success"], Is.EqualTo("Successfully deleted restaurant!"));
        }
    }
}
