using AutoMapper;
using GustoExpress.Data.Models;
using GustoExpress.Services.Data.Contracts;
using GustoExpress.Services.Mapping;
using GustoExpress.Web.Data;
using GustoExpress.Web.ViewModels;
using Microsoft.EntityFrameworkCore;
using Moq;

namespace GustoExpress.Services.Data.UnitTests
{
    [TestFixture]
    public class ProductServiceTests
    {
        private IEnumerable<Product> products;
        private ApplicationDbContext _context;
        private IMapper _mapper;
        private IRestaurantService _restaurantService;
        private IProductService _productService;
        private ICityService _cityService;

        [SetUp]
        public async Task SetUp()
        {
            var restaurant = new Restaurant()
            {
                Id = Guid.NewGuid(),
                CityId = Guid.NewGuid(),
                City = new City() { Id = Guid.NewGuid(), CityName = "TestCity" },
                DeliveryPrice = 2,
                Description = "Test description",
                Name = "Test restaurant",
                TimeToDeliver = "20-30"
            };

            products = new List<Product>()
            {
                new Product()
                {
                Id = Guid.NewGuid(),
                Name = "Test product",
                Description = "This is test product",
                Category = GustoExpress.Data.Models.Enums.Category.Meat,
                Price = 10m,
                RestaurantId = restaurant.Id,
                Restaurant = restaurant,
                Grams = 200m
                }
            };

            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(databaseName: "GustoExpressInMemory")
               .Options;
            _context = new ApplicationDbContext(options);

            _context.Database.EnsureDeleted();

            await _context.Products.AddRangeAsync(products);
            await _context.Restaurants.AddAsync(restaurant);
            await _context.SaveChangesAsync();

            var configuration = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<GustoExpressProfile>();
            });
            _mapper = configuration.CreateMapper();

            _cityService = new CityService(_context);
            _restaurantService = new RestaurantService(_context, _mapper, _cityService);
            _productService = new ProductService(_context, _mapper, _restaurantService);
        }

        [Test]
        public void Test_Constructor_ShouldWork()
        {
            Assert.IsNotNull(_productService);
            Assert.IsNotNull(_restaurantService);
            Assert.IsNotNull(_cityService);
            Assert.IsNotNull(_mapper);
        }

        [Test]
        public async Task Test_ProjectToModel_ShouldWork()
        {
            var createProductViewModel = await _productService.ProjectToModel<CreateProductViewModel>(products.First().Id.ToString());
            var productViewModel = await _productService.ProjectToModel<ProductViewModel>(products.First().Id.ToString());

            Assert.That(createProductViewModel.GetType(), Is.EqualTo(typeof(CreateProductViewModel)));
            Assert.That(productViewModel.GetType(), Is.EqualTo(typeof(ProductViewModel)));
        }

        [Test]
        public async Task Test_GetById_ShouldWork()
        {
            var product = await _productService.GetByIdAsync(products.First().Id.ToString());

            Assert.IsNotNull(product);
            Assert.That(product.Id, Is.EqualTo(products.First().Id));
        }

        [Test]
        public async Task Test_GetById_ShoudlReturnNull()
        {
            var product = await _productService.GetByIdAsync("0475e554-e1d8-4a5d-8a63-ee2657bfa4ed");

            Assert.IsNull(product);
        }

        [Test]
        public async Task Test_CreateProduct_ShouldWork()
        {
            var model = new CreateProductViewModel()
            {
                Name = "Created product",
                Description = "This is test product",
                Category = GustoExpress.Data.Models.Enums.Category.Meat,
                Price = 10m,
                RestaurantId = _context.Restaurants.First().Id.ToString(),
                Grams = 200m
            };

            var createdProduct = await _productService.CreateProductAsync(model);

            Assert.IsNotNull(createdProduct);
            Assert.That(typeof(ProductViewModel), Is.EqualTo(createdProduct.GetType()));
            Assert.That(_context.Products.Count(), Is.EqualTo(2));
            Assert.True(_context.Products.Any(p => p.Name == "Created product"));
        }

        [Test]
        public async Task Test_EditProduct_ShouldWork()
        {
            var model = new CreateProductViewModel()
            {
                Name = "Created product",
                Description = "This is test product",
                Category = GustoExpress.Data.Models.Enums.Category.Meat,
                Price = 10m,
                RestaurantId = _context.Restaurants.First().Id.ToString(),
                Grams = 200m
            };

            var editedProduct = await _productService.EditProductAsync(products.First().Id.ToString(), model);

            Assert.IsNotNull(editedProduct);
            Assert.That(typeof(ProductViewModel), Is.EqualTo(editedProduct.GetType()));
            Assert.That(_context.Products.Count(), Is.EqualTo(1));
            Assert.True(_context.Products.Any(p => p.Name == "Created product"));
        }

        [Test]
        public async Task Test_DeleteProduct_ShouldWork()
        {
            var deletedProduct = await _productService.DeleteAsync(products.First().Id.ToString());

            Assert.IsNotNull(deletedProduct);
            Assert.That(typeof(ProductViewModel), Is.EqualTo(deletedProduct.GetType()));
            Assert.True(_context.Products.First(p => p.Name == "Test product").IsDeleted);
        }

        [Test]
        public async Task Test_SaveImageUrl_ShouldWork()
        {
            var model = await _productService.ProjectToModel<ProductViewModel>(products.First().Id.ToString());
            await _productService.SaveImageURL("Test Url", model);

            Assert.That(_context.Products.First(p => p.Name == "Test product").ImageURL, Is.EqualTo("Test Url"));
        }
    }
}
