namespace GustoExpress.Web.Controllers.Tests
{
    using System.Security.Claims;

    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc.ViewFeatures;
    using Microsoft.AspNetCore.Mvc;

    using GustoExpress.Services.Data.Contracts;
    using GustoExpress.Web.ViewModels;

    using Moq;

    [TestFixture]
    public class ReviewControllerTests
    {
        private ReviewController controller;

        private Mock<IReviewService> _reviewService;

        private string generalErrorMessage = "Unexpected error occurred! Please try again later or contact administrator";
        private string userId = "0cb9b955-9152-42fd-a899-84b4d6f89c21";

        [SetUp]
        public void Setup()
        {
            _reviewService = new Mock<IReviewService>();

            controller = new ReviewController(_reviewService.Object)
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
        public async Task Test_AddReview_ShouldWork()
        {
            var model = new CreateReviewViewModel()
            {
                Text = "test",
                Title = "test",
                Stars = 3,
            };

            var exprectedModel = new ReviewViewModel()
            {
                Text = "test",
                Title = "test",
                Stars = 3,
                UserId = userId,
                RestaurantId = "someId"
            };

            _reviewService
                .Setup(r => r.CreateReview(userId, model))
                .ReturnsAsync(exprectedModel);

            var result = await controller.AddReview(model);

            var redirectResult = (RedirectToActionResult)result;
            Assert.That(result, Is.InstanceOf<IActionResult>());
            Assert.That(redirectResult.ControllerName, Is.EqualTo("Restaurant"));
            Assert.That(redirectResult.ActionName, Is.EqualTo("RestaurantPage"));

            Assert.That(controller.TempData["success"], Is.EqualTo("Successfully created review!"));
        }

        [Test]
        public async Task Test_AddReview_ShouldReturnInvalidOperation()
        {
            var model = new CreateReviewViewModel()
            {
                Text = "test",
                Title = "test",
                Stars = 3,
            };

            var exprectedModel = new ReviewViewModel()
            {
                Text = "test",
                Title = "test",
                Stars = 3,
                UserId = userId,
                RestaurantId = "someId"
            };

            controller.ModelState.AddModelError("test", "invalid operation");

            _reviewService
                .Setup(r => r.CreateReview(userId, model))
                .ReturnsAsync(exprectedModel);

            var result = await controller.AddReview(model);

            var redirectResult = (RedirectToActionResult)result;
            Assert.That(result, Is.InstanceOf<IActionResult>());
            Assert.That(redirectResult.ControllerName, Is.EqualTo("Restaurant"));
            Assert.That(redirectResult.ActionName, Is.EqualTo("RestaurantPage"));

            Assert.That(controller.TempData["danger"], Is.EqualTo("Invalid operation!"));
        }

        [Test]
        public async Task Test_DeleteReview_ShouldWork()
        {
            var exprectedModel = new ReviewViewModel()
            {
                Text = "test",
                Title = "test",
                Stars = 3,
                UserId = userId,
                RestaurantId = "someId"
            };

            _reviewService
                .Setup(r => r.DeleteAsync("someId"))
                .ReturnsAsync(exprectedModel);

            var result = await controller.DeleteReview("someId");

            var redirectResult = (RedirectToActionResult)result;
            Assert.That(result, Is.InstanceOf<IActionResult>());
            Assert.That(redirectResult.ControllerName, Is.EqualTo("Restaurant"));
            Assert.That(redirectResult.ActionName, Is.EqualTo("RestaurantPage"));

            Assert.That(controller.TempData["success"], Is.EqualTo("Successfully deleted review!"));
        }
    }
}
