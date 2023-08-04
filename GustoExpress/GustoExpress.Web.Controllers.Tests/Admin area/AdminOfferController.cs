namespace GustoExpress.Web.Controllers.Tests.Admin_area
{
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.Rendering;
    using Microsoft.AspNetCore.Mvc.ViewFeatures;

    using GustoExpress.Services.Data.Contracts;
    using GustoExpress.Web.Areas.Admin.Controllers;
    using GustoExpress.Web.ViewModels;
    using Microsoft.AspNetCore.Hosting;

    using Moq;

    [TestFixture]
    public class AdminOfferController
    {
        private OfferController controller;

        private Mock<IOfferService> _offerService;
        private Mock<IWebHostEnvironment> _webHostEnvironment;
        private Mock<IRestaurantService> _restaurantService;

        private string restaurantId = "7d188e14-4136-48fb-9eba-8f60fa6e02bc";
        private string offerId = "f958fc05-fbe1-419e-89dc-7c890ae5d20f";
        private string generalErrorMessage = "Unexpected error occurred! Please try again later or contact administrator";

        [SetUp]
        public void SetUp()
        {
            _offerService = new Mock<IOfferService>();
            _webHostEnvironment = new Mock<IWebHostEnvironment>();
            _restaurantService = new Mock<IRestaurantService>();

            controller = new OfferController(_offerService.Object, _webHostEnvironment.Object, _restaurantService.Object);

            controller.TempData = new TempDataDictionary(
               new DefaultHttpContext(),
               Mock.Of<ITempDataProvider>());
        }

        [Test]
        public async Task Test_CreateOfferOnGet_ShouldReturnGeneralErrorForNonExistingRestaurant()
        {
            _restaurantService
                .Setup(r => r.HasRestaurantWithId(restaurantId))
                .ReturnsAsync(false);

            var result = await controller.CreateOffer(restaurantId);

            var redirectResult = (RedirectToActionResult)result;
            Assert.That(result, Is.InstanceOf<IActionResult>());
            Assert.That(redirectResult.ControllerName, Is.EqualTo("Home"));
            Assert.That(redirectResult.ActionName, Is.EqualTo("Index"));

            Assert.That(controller.TempData["danger"], Is.EqualTo(generalErrorMessage));
        }

        [Test]
        public async Task Test_CreateOfferOnGet_ShouldWork()
        {
            _restaurantService
                .Setup(r => r.HasRestaurantWithId(restaurantId))
                .ReturnsAsync(true);

            var model = new CreateOfferViewModel()
            {
                Name = "test offer"
            };

            var result = await controller.CreateOffer(restaurantId);
            Assert.That(result, Is.InstanceOf<ViewResult>());
        }

        [Test]
        public async Task Test_CreateOfferOnPost_ShouldWork()
        {
            var model = new CreateOfferViewModel()
            {
                Name = "test offer"
            };

            var expectedModel = new OfferViewModel()
            {
                Name = "test offer"
            };

            _offerService
                .Setup(r => r.CreateOfferAsync(restaurantId, model))
                .ReturnsAsync(expectedModel);

            var result = await controller.CreateOffer(null, restaurantId, model);

            var redirectResult = (RedirectToActionResult)result;
            Assert.That(result, Is.InstanceOf<IActionResult>());
            Assert.That(redirectResult.ControllerName, Is.EqualTo("Restaurant"));
            Assert.That(redirectResult.ActionName, Is.EqualTo("RestaurantPage"));

            Assert.That(controller.TempData["success"], Is.EqualTo("Successfully created offer!"));
        }

        [Test]
        public async Task Test_CreateOfferOnPost_ShouldReturnItsViewForInvalidModelState()
        {
            var model = new CreateOfferViewModel()
            {
                Name = "test offer"
            };

            controller.ModelState.AddModelError("Error", "Invalid");

            var result = await controller.CreateOffer(null, restaurantId, model);

            Assert.That(result, Is.InstanceOf<ViewResult>());
        }

        [Test]
        public async Task Test_CreateOfferOnPost_ShouldThrowInvalidOperationException()
        {
            var model = new CreateOfferViewModel()
            {
                Name = "test offer"
            };

            _offerService
                .Setup(o => o.CreateOfferAsync(restaurantId, model))
                .ThrowsAsync(new InvalidOperationException("An item with this name already exists!"));

            var result = await controller.CreateOffer(null, restaurantId, model);

            Assert.That(result, Is.InstanceOf<ViewResult>());

            Assert.That(controller.TempData["danger"], Is.EqualTo("An item with this name already exists!"));
        }

        [Test]
        public async Task Test_CreateOfferOnPost_ShouldThrowException()
        {
            var model = new CreateOfferViewModel()
            {
                Name = "test offer"
            };

            _offerService
                .Setup(o => o.CreateOfferAsync(restaurantId, model))
                .ThrowsAsync(new Exception());

            var result = await controller.CreateOffer(null, restaurantId, model);

            var redirectResult = (RedirectToActionResult)result;
            Assert.That(result, Is.InstanceOf<IActionResult>());
            Assert.That(redirectResult.ControllerName, Is.EqualTo("Home"));
            Assert.That(redirectResult.ActionName, Is.EqualTo("Index"));

            Assert.That(controller.TempData["danger"], Is.EqualTo(generalErrorMessage));
        }

        [Test]
        public async Task Test_EditOfferOnGet_ShouldReturnGeneralErrorForNonExistringOffer()
        {
            _offerService
                .Setup(o => o.HasOfferWithId(offerId))
                .ReturnsAsync(false);

            var result = await controller.EditOffer(offerId);

            var redirectResult = (RedirectToActionResult)result;
            Assert.That(result, Is.InstanceOf<IActionResult>());
            Assert.That(redirectResult.ControllerName, Is.EqualTo("Home"));
            Assert.That(redirectResult.ActionName, Is.EqualTo("Index"));

            Assert.That(controller.TempData["danger"], Is.EqualTo(generalErrorMessage));
        }

        [Test]
        public async Task Test_EditOfferOnGet_ShouldWorkAndReturnView()
        {
            var expectedModel = new CreateOfferViewModel()
            {
                Name = "test offer",
                RestaurantId = restaurantId
            };

            var expectedList = new List<SelectListItem>()
            {
                new SelectListItem { Value = "1", Text = "Option 1" },
                new SelectListItem { Value = "2", Text = "Option 2" },
                new SelectListItem { Value = "3", Text = "Option 3" },
            };

            _offerService
                .Setup(o => o.HasOfferWithId(offerId))
                .ReturnsAsync(true);

            _offerService
                .Setup(o => o.ProjectToModel<CreateOfferViewModel>(offerId))
                .ReturnsAsync(expectedModel);

            _offerService
                .Setup(o => o.GetProductsByRestaurantIdAsync(restaurantId))
                .ReturnsAsync(expectedList);

            var result = await controller.EditOffer(offerId);

            Assert.That(result, Is.InstanceOf<ViewResult>());
        }

        [Test]
        public async Task Test_EditOfferOnPost_ShouldRedirectWithInvalidOperationForDiscount()
        {
            var model = new CreateOfferViewModel()
            {
                Name = "test offer",
                RestaurantId = restaurantId,
                Discount = 1,
                Price = 0.5m
            };

            var result = await controller.EditOffer(null, offerId, model);

            var redirectResult = (RedirectToActionResult)result;
            Assert.That(result, Is.InstanceOf<IActionResult>());
            Assert.That(redirectResult.ControllerName, Is.EqualTo("Restaurant"));
            Assert.That(redirectResult.ActionName, Is.EqualTo("RestaurantPage"));

            Assert.That(controller.TempData["danger"], Is.EqualTo("Invalid operation!"));
        }

        [Test]
        public async Task Test_EditOfferOnPost_ShouldWork()
        {
            var model = new CreateOfferViewModel()
            {
                Name = "test offer",
                RestaurantId = restaurantId,
                Discount = 1,
                Price = 5
            };

            var expectedModel = new OfferViewModel()
            {
                Name = "test offer",
                RestaurantId = Guid.Parse(restaurantId),
                Discount = 1,
                Price = 5,
            };

            _offerService
                .Setup(o => o.EditOfferAsync(offerId, model))
                .ReturnsAsync(expectedModel);

            var result = await controller.EditOffer(null, offerId, model);

            var redirectResult = (RedirectToActionResult)result;
            Assert.That(result, Is.InstanceOf<IActionResult>());
            Assert.That(redirectResult.ControllerName, Is.EqualTo("Restaurant"));
            Assert.That(redirectResult.ActionName, Is.EqualTo("RestaurantPage"));

            Assert.That(controller.TempData["success"], Is.EqualTo("Successfully updated offer!"));
        }

        [Test]
        public async Task Test_EditOfferOnPost_ShouldReturnViewWithInvalidOperationErrorMessage()
        {
            var model = new CreateOfferViewModel()
            {
                Name = "test offer",
                RestaurantId = restaurantId,
                Discount = 1,
                Price = 5
            };

            var expectedModel = new OfferViewModel()
            {
                Name = "test offer",
                RestaurantId = Guid.Parse(restaurantId),
                Discount = 6,
                Price = 5,
            };

            _offerService
                .Setup(o => o.EditOfferAsync(offerId, model))
                .ReturnsAsync(expectedModel);

            var result = await controller.EditOffer(null, offerId, model);

            Assert.That(result, Is.InstanceOf<ViewResult>());

            Assert.That(controller.TempData["danger"], Is.EqualTo("Invalid operation!"));
        }

        [Test]
        public async Task Test_EditOfferOnPost_ShouldReturnViewWithInvalidModelState()
        {
            var model = new CreateOfferViewModel()
            {
                Name = "test offer",
                RestaurantId = restaurantId,
                Discount = 1,
                Price = 5
            };

            controller.ModelState.AddModelError("Error", "Invalid");

            var result = await controller.EditOffer(null, offerId, model);

            Assert.That(result, Is.InstanceOf<ViewResult>());
        }

        [Test]
        public async Task Test_EditOfferOnPost_ShouldThrowException()
        {
            var model = new CreateOfferViewModel()
            {
                Name = "test offer",
                RestaurantId = restaurantId,
                Discount = 1,
                Price = 5
            };

            _offerService
                .Setup(o => o.EditOfferAsync(offerId, model))
                .ThrowsAsync(new Exception());

            var result = await controller.EditOffer(null, offerId, model);

            var redirectResult = (RedirectToActionResult)result;
            Assert.That(result, Is.InstanceOf<IActionResult>());
            Assert.That(redirectResult.ControllerName, Is.EqualTo("Home"));
            Assert.That(redirectResult.ActionName, Is.EqualTo("Index"));

            Assert.That(controller.TempData["danger"], Is.EqualTo(generalErrorMessage));
        }

        [Test]
        public async Task Test_DeleteOffer_ShouldReturnGeneralErrorForNonExistringOffer()
        {
            _offerService
                .Setup(o => o.HasOfferWithId(offerId))
                .ReturnsAsync(false);

            var result = await controller.DeleteOffer(offerId);

            var redirectResult = (RedirectToActionResult)result;
            Assert.That(result, Is.InstanceOf<IActionResult>());
            Assert.That(redirectResult.ControllerName, Is.EqualTo("Home"));
            Assert.That(redirectResult.ActionName, Is.EqualTo("Index"));

            Assert.That(controller.TempData["danger"], Is.EqualTo(generalErrorMessage));
        }

        [Test]
        public async Task Test_DeleteOffer_ShouldThrowException()
        {
            _offerService
                .Setup(o => o.HasOfferWithId(offerId))
                .ReturnsAsync(true);

            _offerService
                .Setup(o => o.DeleteOfferAsync(offerId))
                .ThrowsAsync(new Exception());

            var result = await controller.DeleteOffer(offerId);

            var redirectResult = (RedirectToActionResult)result;
            Assert.That(result, Is.InstanceOf<IActionResult>());
            Assert.That(redirectResult.ControllerName, Is.EqualTo("Home"));
            Assert.That(redirectResult.ActionName, Is.EqualTo("Index"));

            Assert.That(controller.TempData["danger"], Is.EqualTo(generalErrorMessage));
        }

        [Test]
        public async Task Test_DeleteOffer_ShouldWork()
        {
            var expectedModel = new OfferViewModel()
            {
                Name = "test offer",
                RestaurantId = Guid.Parse(restaurantId),
                Discount = 6,
                Price = 5,
            };

            _offerService
                .Setup(o => o.HasOfferWithId(offerId))
                .ReturnsAsync(true);

            _offerService
                .Setup(o => o.DeleteOfferAsync(offerId))
                .ReturnsAsync(expectedModel);

            var result = await controller.DeleteOffer(offerId);

            var redirectResult = (RedirectToActionResult)result;
            Assert.That(result, Is.InstanceOf<IActionResult>());
            Assert.That(redirectResult.ControllerName, Is.EqualTo("Restaurant"));
            Assert.That(redirectResult.ActionName, Is.EqualTo("RestaurantPage"));

            Assert.That(controller.TempData["success"], Is.EqualTo("Successfully deleted offer!"));
        }
    }
}
