namespace GustoExpress.Web.Controllers.Tests.Admin_area
{
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.ViewFeatures;

    using GustoExpress.Services.Data.Contracts;
    using GustoExpress.Web.Areas.Admin.Controllers;
    using GustoExpress.Web.ViewModels;

    using Moq;

    [TestFixture]
    public class AdminProductControllerTests
    {
        private ProductController controller;

        private Mock<IProductService> _productService;
        private Mock<IWebHostEnvironment> _webHostEnvironment;
        private Mock<IRestaurantService> _restaurantService;

        private string restaurantId = "7d188e14-4136-48fb-9eba-8f60fa6e02bc";
        private string productId = "f958fc05-fbe1-419e-89dc-7c890ae5d20f";
        private string generalErrorMessage = "Unexpected error occurred! Please try again later or contact administrator";

        [SetUp]
        public void SetUp()
        {
            _productService = new Mock<IProductService>();
            _webHostEnvironment = new Mock<IWebHostEnvironment>();
            _restaurantService = new Mock<IRestaurantService>();

            controller = new ProductController(_productService.Object, _webHostEnvironment.Object, _restaurantService.Object);

            controller.TempData = new TempDataDictionary(
               new DefaultHttpContext(),
               Mock.Of<ITempDataProvider>());
        }

        [Test]
        public async Task Test_CreateProductOnGet_ShouldReturnGeneralErrorForNonExistingRestaurant()
        {
            _restaurantService
                .Setup(r => r.HasRestaurantWithId(restaurantId))
                .ReturnsAsync(false);

            var result = await controller.CreateProduct(restaurantId);

            var redirectResult = (RedirectToActionResult)result;
            Assert.That(result, Is.InstanceOf<IActionResult>());
            Assert.That(redirectResult.ControllerName, Is.EqualTo("Home"));
            Assert.That(redirectResult.ActionName, Is.EqualTo("Index"));

            Assert.That(controller.TempData["danger"], Is.EqualTo(generalErrorMessage));
        }

        [Test]
        public async Task Test_CreateProductOnGet_ShouldWork()
        {
            _restaurantService
                .Setup(r => r.HasRestaurantWithId(restaurantId))
                .ReturnsAsync(true);

            var model = new CreateProductViewModel()
            {
                Name = "test product"
            };

            var result = await controller.CreateProduct(restaurantId);
            Assert.That(result, Is.InstanceOf<ViewResult>());
        }

        [Test]
        public async Task Test_CreateProductOnPost_ShouldWork()
        {
            var model = new CreateProductViewModel()
            {
                Name = "test product"
            };

            var expectedModel = new ProductViewModel()
            {
                Name = "test product"
            };

            _productService
                .Setup(r => r.CreateProductAsync(model))
                .ReturnsAsync(expectedModel);

            var result = await controller.CreateProduct(null, model);

            var redirectResult = (RedirectToActionResult)result;
            Assert.That(result, Is.InstanceOf<IActionResult>());
            Assert.That(redirectResult.ControllerName, Is.EqualTo("Restaurant"));
            Assert.That(redirectResult.ActionName, Is.EqualTo("RestaurantPage"));

            Assert.That(controller.TempData["success"], Is.EqualTo("Successfully created product!"));
        }

        [Test]
        public async Task Test_CreateProductOnPost_ShouldReturnItsViewForInvalidModelState()
        {
            var model = new CreateProductViewModel()
            {
                Name = "test product"
            };

            controller.ModelState.AddModelError("Error", "Invalid");

            var result = await controller.CreateProduct(null, model);

            Assert.That(result, Is.InstanceOf<ViewResult>());
        }

        [Test]
        public async Task Test_CreateProductOnPost_ShouldThrowInvalidOperationException()
        {
            var model = new CreateProductViewModel()
            {
                Name = "test product"
            };

            _productService
                .Setup(o => o.CreateProductAsync(model))
                .ThrowsAsync(new InvalidOperationException("An item with this name already exists!"));

            var result = await controller.CreateProduct(null, model);

            Assert.That(result, Is.InstanceOf<ViewResult>());

            Assert.That(controller.TempData["danger"], Is.EqualTo("An item with this name already exists!"));
        }

        [Test]
        public async Task Test_CreateProductOnPost_ShouldThrowException()
        {
            var model = new CreateProductViewModel()
            {
                Name = "test product"
            };

            _productService
                .Setup(o => o.CreateProductAsync(model))
                .ThrowsAsync(new Exception());

            var result = await controller.CreateProduct(null, model);

            var redirectResult = (RedirectToActionResult)result;
            Assert.That(result, Is.InstanceOf<IActionResult>());
            Assert.That(redirectResult.ControllerName, Is.EqualTo("Home"));
            Assert.That(redirectResult.ActionName, Is.EqualTo("Index"));

            Assert.That(controller.TempData["danger"], Is.EqualTo(generalErrorMessage));
        }

        [Test]
        public async Task Test_EditProductOnGet_ShouldReturnGeneralErrorForNonExistringProduct()
        {
            _productService
                .Setup(o => o.HasProductWithId(productId))
                .ReturnsAsync(false);

            var result = await controller.EditProduct(productId);

            var redirectResult = (RedirectToActionResult)result;
            Assert.That(result, Is.InstanceOf<IActionResult>());
            Assert.That(redirectResult.ControllerName, Is.EqualTo("Home"));
            Assert.That(redirectResult.ActionName, Is.EqualTo("Index"));

            Assert.That(controller.TempData["danger"], Is.EqualTo(generalErrorMessage));
        }

        [Test]
        public async Task Test_EditProductOnGet_ShouldWorkAndReturnView()
        {
            var expectedModel = new CreateProductViewModel()
            {
                Name = "test product",
                RestaurantId = restaurantId
            };

            _productService
                .Setup(o => o.HasProductWithId(productId))
                .ReturnsAsync(true);

            _productService
                .Setup(o => o.ProjectToModel<CreateProductViewModel>(productId))
                .ReturnsAsync(expectedModel);

            var result = await controller.EditProduct(productId);

            Assert.That(result, Is.InstanceOf<ViewResult>());
        }

        [Test]
        public async Task Test_EditProductOnPost_ShouldRedirectWithInvalidOperationForDiscount()
        {
            var model = new CreateProductViewModel()
            {
                Name = "test product",
                RestaurantId = restaurantId,
                Discount = 1,
                Price = 0.5m
            };

            var result = await controller.EditProduct(null, productId, model);

            var redirectResult = (RedirectToActionResult)result;
            Assert.That(result, Is.InstanceOf<IActionResult>());
            Assert.That(redirectResult.ControllerName, Is.EqualTo("Restaurant"));
            Assert.That(redirectResult.ActionName, Is.EqualTo("RestaurantPage"));

            Assert.That(controller.TempData["danger"], Is.EqualTo("Invalid operation!"));
        }

        [Test]
        public async Task Test_EditProductOnPost_ShouldWork()
        {
            var model = new CreateProductViewModel()
            {
                Name = "test product",
                RestaurantId = restaurantId,
                Discount = 1,
                Price = 5
            };

            var expectedModel = new ProductViewModel()
            {
                Name = "test product",
                RestaurantId = Guid.Parse(restaurantId),
                Discount = 1,
                Price = 5,
            };

            _productService
                .Setup(o => o.EditProductAsync(productId, model))
                .ReturnsAsync(expectedModel);

            var result = await controller.EditProduct(null, productId, model);

            var redirectResult = (RedirectToActionResult)result;
            Assert.That(result, Is.InstanceOf<IActionResult>());
            Assert.That(redirectResult.ControllerName, Is.EqualTo("Restaurant"));
            Assert.That(redirectResult.ActionName, Is.EqualTo("RestaurantPage"));

            Assert.That(controller.TempData["success"], Is.EqualTo("Successfully updated product!"));
        }

        [Test]
        public async Task Test_EditProductOnPost_ShouldReturnViewWithInvalidModelState()
        {
            var model = new CreateProductViewModel()
            {
                Name = "test product",
                RestaurantId = restaurantId,
                Discount = 1,
                Price = 5
            };

            controller.ModelState.AddModelError("Error", "Invalid");

            var result = await controller.EditProduct(null, productId, model);

            Assert.That(result, Is.InstanceOf<ViewResult>());
        }

        [Test]
        public async Task Test_EditProductOnPost_ShouldThrowException()
        {
            var model = new CreateProductViewModel()
            {
                Name = "test product",
                RestaurantId = restaurantId,
                Discount = 1,
                Price = 5
            };

            _productService
                .Setup(o => o.EditProductAsync(productId, model))
                .ThrowsAsync(new Exception());

            var result = await controller.EditProduct(null, productId, model);

            var redirectResult = (RedirectToActionResult)result;
            Assert.That(result, Is.InstanceOf<IActionResult>());
            Assert.That(redirectResult.ControllerName, Is.EqualTo("Home"));
            Assert.That(redirectResult.ActionName, Is.EqualTo("Index"));

            Assert.That(controller.TempData["danger"], Is.EqualTo(generalErrorMessage));
        }

        [Test]
        public async Task Test_DeleteProduct_ShouldReturnGeneralErrorForNonExistringProduct()
        {
            _productService
                .Setup(o => o.HasProductWithId(productId))
                .ReturnsAsync(false);

            var result = await controller.DeleteProduct(productId);

            var redirectResult = (RedirectToActionResult)result;
            Assert.That(result, Is.InstanceOf<IActionResult>());
            Assert.That(redirectResult.ControllerName, Is.EqualTo("Home"));
            Assert.That(redirectResult.ActionName, Is.EqualTo("Index"));

            Assert.That(controller.TempData["danger"], Is.EqualTo(generalErrorMessage));
        }

        [Test]
        public async Task Test_DeleteProduct_ShouldThrowException()
        {
            _productService
                .Setup(o => o.HasProductWithId(productId))
                .ReturnsAsync(true);

            _productService
                .Setup(o => o.DeleteAsync(productId))
                .ThrowsAsync(new Exception());

            var result = await controller.DeleteProduct(productId);

            var redirectResult = (RedirectToActionResult)result;
            Assert.That(result, Is.InstanceOf<IActionResult>());
            Assert.That(redirectResult.ControllerName, Is.EqualTo("Home"));
            Assert.That(redirectResult.ActionName, Is.EqualTo("Index"));

            Assert.That(controller.TempData["danger"], Is.EqualTo(generalErrorMessage));
        }

        [Test]
        public async Task Test_DeleteProduct_ShouldWork()
        {
            var expectedModel = new ProductViewModel()
            {
                Name = "test product",
                RestaurantId = Guid.Parse(restaurantId),
                Discount = 6,
                Price = 5,
            };

            _productService
                .Setup(o => o.HasProductWithId(productId))
                .ReturnsAsync(true);

            _productService
                .Setup(o => o.DeleteAsync(productId))
                .ReturnsAsync(expectedModel);

            var result = await controller.DeleteProduct(productId);

            var redirectResult = (RedirectToActionResult)result;
            Assert.That(result, Is.InstanceOf<IActionResult>());
            Assert.That(redirectResult.ControllerName, Is.EqualTo("Restaurant"));
            Assert.That(redirectResult.ActionName, Is.EqualTo("RestaurantPage"));

            Assert.That(controller.TempData["success"], Is.EqualTo("Successfully deleted product!"));
        }
    }
}
