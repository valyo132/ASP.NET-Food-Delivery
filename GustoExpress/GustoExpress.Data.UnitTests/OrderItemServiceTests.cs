using AutoMapper;
using GustoExpress.Web.Data;
using GustoExpress.Data.Models;
using GustoExpress.Services.Data.Contracts;
using Microsoft.EntityFrameworkCore;
using GustoExpress.Services.Mapping;
using NUnit.Framework.Constraints;
using GustoExpress.Web.ViewModels;

namespace GustoExpress.Services.Data.UnitTests
{
    [TestFixture]
    public class OrderItemServiceTests
    {
        private ApplicationDbContext _context;
        private IOrderItemService _orderItemService;
        private IRestaurantService _restaurantService;
        private IProductService _productService;
        private IOfferService _offerService;
        private ICityService _cityService;
        private IMapper _mapper;

        private IEnumerable<Product> products;
        private IEnumerable<Offer> offers;
        private IEnumerable<OrderItem> orderItems;
        private IEnumerable<ApplicationUser> users;
        private IEnumerable<Restaurant> restaurants;

        [SetUp]
        public async Task SetUp()
        {
            var firstRestaurant = new Restaurant()
            {
                Id = Guid.NewGuid(),
                CityId = Guid.NewGuid(),
                City = new City() { Id = Guid.NewGuid(), CityName = "TestCity" },
                DeliveryPrice = 2,
                Description = "Test description",
                Name = "First test restaurant",
                TimeToDeliver = "20-30"
            };

            var product = new Product()
            {
                Id = Guid.NewGuid(),
                Name = "Test product",
                Description = "This is test product",
                Category = GustoExpress.Data.Models.Enums.Category.Meat,
                Price = 10m,
                RestaurantId = firstRestaurant.Id,
                Restaurant = firstRestaurant,
                Grams = 200m
            };

            var offer = new Offer()
            {
                Id = Guid.NewGuid(),
                Name = "Test product",
                Description = "This is test product",
                Price = 10m,
                RestaurantId = firstRestaurant.Id,
                Restaurant = firstRestaurant,
                Discount = 0m,
                ImageURL = "TestImgUrl"
            };

            var user = new ApplicationUser()
            {
                Id = "1cd0be45-b9be-4fc9-9b0f-46a1d20a77be",
                Email = "admin@admin.com",
                NormalizedEmail = "ADMIN@ADMIN.COM",
                UserName = "admin@admin.com",
                NormalizedUserName = "ADMIN@ADMIN.COM",
                EmailConfirmed = false,
                FirstName = "Best",
                LastName = "Admin",
                PasswordHash = "46708f23d682fef9aa996ecbb139bfb6c9ffdc039905ad6ad5c85a88b9411d97",
            };

            var orderItem = new OrderItem()
            {
                Id = Guid.NewGuid(),
                OrderId = null,
                Product = null,
                Offer = offer,
                User = user,
            };

            var secondOrderItem = new OrderItem()
            {
                Id = Guid.NewGuid(),
                OrderId = null,
                Product = product,
                Offer = null,
                User = user,
            };

            restaurants = new List<Restaurant>() { firstRestaurant };
            products = new List<Product>() { product };
            offers = new List<Offer>() { offer };
            users = new List<ApplicationUser>() { user };
            orderItems = new List<OrderItem>() { orderItem, secondOrderItem };

            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: "GustoExpressInMemory")
                .Options;
            _context = new ApplicationDbContext(options);

            _context.Database.EnsureDeleted();

            _context.OrderItems.AddRange(orderItems);
            _context.Products.AddRange(products);

            _context.SaveChanges();

            var configuration = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<GustoExpressProfile>();
            });
            _mapper = configuration.CreateMapper();

            _cityService = new CityService(_context);
            _restaurantService = new RestaurantService(_context, _mapper, _cityService);
            _productService = new ProductService(_context, _mapper, _restaurantService);
            _offerService = new OfferService(_context, _mapper, _productService, _restaurantService);

            _orderItemService = new OrderItemService(_context, _productService, _offerService, _mapper);
        }

        [Test]
        public async Task Test_HasOrderItemWithId_ShoudlReturnTrue()
        {
            string id = orderItems.First().Id.ToString();

            var actual = await _orderItemService.HasOrderItemWithId(id);

            Assert.True(actual);
        }

        [Test]
        public async Task Test_HasOrderItemWithId_ShoudlReturnFalse()
        {
            string id = "b92fe21a-11e9-442e-b4df-71c7eb45cc87";

            var actual = await _orderItemService.HasOrderItemWithId(id);

            Assert.False(actual);
        }

        [Test]
        public async Task Test_GetOrderItemByIdAsync_ShouldWork()
        {
            string id = orderItems.First().Id.ToString();

            var actual = await _orderItemService.GetOrderItemByIdAsync(id);

            Assert.That(actual, Is.EqualTo(orderItems.First()));
        }

        [Test]
        public async Task Test_GetOrderItemByIdAsync_ShouldReturnNull()
        {
            string id = "b92fe21a-11e9-442e-b4df-71c7eb45cc87";

            var actual = await _orderItemService.GetOrderItemByIdAsync(id);

            Assert.Null(actual);
        }

        [Test]
        public async Task Test_GetRestaurantIdAsync_ShouldWork()
        {
            string firstItemId = orderItems.First().Id.ToString();
            string secondItemId = orderItems.Last().Id.ToString();

            string firstActual = await _orderItemService.GetRestaurantIdAsync(firstItemId);
            string firstExpected = restaurants.First().Id.ToString();

            string secondActual = await _orderItemService.GetRestaurantIdAsync(secondItemId);
            string secondExpected = restaurants.First().Id.ToString();

            Assert.That(firstActual, Is.EqualTo(firstExpected));
            Assert.That(secondActual, Is.EqualTo(secondExpected));
        }

        [Test]
        public async Task Test_GetObjectAsync_ShouldReturnObjectOfTypeOffer()
        {
            string offerId = offers.First().Id.ToString();

            var actual = await _orderItemService.GetObjectAsync(offerId);

            Assert.That(actual.GetType(), Is.EqualTo(typeof(Offer)));
        }

        [Test]
        public async Task Test_GetObjectAsync_ShouldReturnObjectOfTypeProduct()
        {
            string productId = products.First().Id.ToString();

            var actual = await _orderItemService.GetObjectAsync(productId);

            Assert.That(actual.GetType(), Is.EqualTo(typeof(Product)));
        }

        [Test]
        public async Task Test_GetObjectAsync_ShouldThrowInvalidOperationException()
        {
            string invalidId = "c622c4b6-675a-42b9-9697-018165954dfc";

            Assert.ThrowsAsync<InvalidOperationException>(async () =>
            {
                await _orderItemService.GetObjectAsync(invalidId);
            });
        }

        [Test]
        public void Test_GetOrderItemViewModel_ShouldWorkWithProduct()
        {
            object obj = products.First();

            var actual = _orderItemService.GetOrderItemViewModel(obj);

            Assert.That(actual.GetType(), Is.EqualTo(typeof(CreateOrderItemViewModel)));
        }

        [Test]
        public void Test_GetOrderItemViewModel_ShouldWorkWithOffer()
        {
            object obj = offers.First();

            var actual = _orderItemService.GetOrderItemViewModel(obj);

            Assert.That(actual.GetType(), Is.EqualTo(typeof(CreateOrderItemViewModel)));
        }

        [Test]
        public async Task Test_CreateOrderItemAsync_ShouldWork()
        {
            CreateOrderItemViewModel viewModel = new CreateOrderItemViewModel()
            {
                Quantity = 1,
                UserId = users.First().Id,
                TotalCost = 100,
                RestaurantId = restaurants.First().Id.ToString(),
                Product = products.First(),
            };

            await _orderItemService.CreateOrderItemAsync(viewModel);

            Assert.That(_context.OrderItems.Count(), Is.EqualTo(3));
        }
    }
}
