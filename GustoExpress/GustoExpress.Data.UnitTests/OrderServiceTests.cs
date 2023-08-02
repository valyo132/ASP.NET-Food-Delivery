namespace GustoExpress.Services.Data.UnitTests
{
    using Microsoft.EntityFrameworkCore;

    using GustoExpress.Data.Models;
    using GustoExpress.Services.Data.Contracts;
    using GustoExpress.Services.Mapping;
    using GustoExpress.Web.Data;
    using GustoExpress.Web.ViewModels;

    using AutoMapper;

    [TestFixture]
    public class OrderServiceTests
    {
        private IOrderService _orderService;
        private IOrderItemService _orderItemService;
        private IRestaurantService _restaurantService;
        private IProductService _productService;
        private IOfferService _offerService;
        private ICityService _cityService;
        private ApplicationDbContext _context;
        private IMapper _mapper;

        private IEnumerable<Order> orders;
        private IEnumerable<ApplicationUser> users;
        private IEnumerable<Restaurant> restaurants;
        private IEnumerable<Product> products;
        private IEnumerable<OrderItem> orderItems;

        [SetUp]
        public async Task SetUp()
        {
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

            var orderItem = new OrderItem()
            {
                Id = Guid.NewGuid(),
                OrderId = null,
                Product = product,
                Offer = null,
                User = user,
            };

            var secondOrderItem = new OrderItem()
            {
                Id = Guid.NewGuid(),
                OrderId = null,
                Product = null,
                Offer = offer,
                User = user,
            };

            var firstOrder = new Order()
            {
                Id = Guid.NewGuid(),
                User = user,
                Restaurant = firstRestaurant,
                TotalCost = 1000
            };

            products = new List<Product>() { product };
            orderItems = new List<OrderItem>() { orderItem, secondOrderItem };
            users = new List<ApplicationUser>() { user };
            restaurants = new List<Restaurant>() { firstRestaurant };
            orders = new List<Order>() { firstOrder };

            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: "GustoExpressInMemory")
                .Options;
            _context = new ApplicationDbContext(options);

            _context.Database.EnsureDeleted();

            _context.Orders.AddRange(orders);
            _context.OrderItems.AddRange(orderItems);

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

            _orderService = new OrderService(_context, _orderItemService, _mapper);
        }

        [Test]
        public async Task Test_HasOrderWithId_ShouldReturnTrue()
        {
            string id = orders.First().Id.ToString();

            bool actual = await _orderService.HasOrderWithId(id);

            Assert.True(actual);
        }

        [Test]
        public async Task Test_HasOrderWithId_ShouldReturnFalse()
        {
            string id = "26cd8da6-a215-4c71-bb31-de42d24ce859";

            bool actual = await _orderService.HasOrderWithId(id);

            Assert.False(actual);
        }

        [Test]
        public async Task Test_GetOrderToComplete_ShouldWork()
        {
            string userId = users.First().Id.ToString();
            string restaurantId = restaurants.First().Id.ToString();

            var actual = await _orderService.GetOrderToComplete(userId, restaurantId);

            Assert.IsNotNull(actual);
            Assert.That(actual.GetType(), Is.EqualTo(typeof(OrderViewModel)));
        }

        [Test]
        public async Task Test_GetOrderToComplete_ShouldReturnNullWithInvalidUserId()
        {
            string userId = "9dbb1db4-a847-4a06-bdcb-e6725be91eb2";
            string restaurantId = restaurants.First().Id.ToString();

            Assert.ThrowsAsync<InvalidOperationException>(async () =>
            {
                await _orderService.GetOrderToComplete(userId, restaurantId);
            });
        }

        [Test]
        public async Task Test_GetOrderToComplete_ShouldReturnNullWithInvalidRestaurantId()
        {
            string userId = users.First().Id.ToString();
            string restaurantId = "9dbb1db4-a847-4a06-bdcb-e6725be91eb2";

            Assert.ThrowsAsync<InvalidOperationException>(async () =>
            {
                await _orderService.GetOrderToComplete(userId, restaurantId);
            });
        }

        [Test]
        public async Task Test_CompleteOrder_ShouldWork()
        {
            string userId = users.First().Id.ToString();
            string restaurantId = restaurants.First().Id.ToString();

            await _orderService.CompleteOrder(userId, restaurantId);

            Assert.True(_context.Orders.First().IsCompleted);
        }

        [Test]
        public async Task Test_GetOrderByIdAsync_ShouldReturnOrder()
        {
            string orderId = orders.First().Id.ToString();

            var actual = await _orderService.GetOrderByIdAsync(orderId);

            Assert.IsNotNull(actual);
            Assert.That(actual.GetType(), Is.EqualTo(typeof(Order)));
        }

        [Test]
        public async Task Test_GetOrderByIdAsync_ShouldReturnNullWithInvalidId()
        {
            string orderId = "46c2f349-0cfc-49f5-84a8-49999e49e56d";

            var actual = await _orderService.GetOrderByIdAsync(orderId);

            Assert.Null(actual);
        }

        [Test]
        public async Task Test_GetUserOrderAsync_ShouldReturnOrder()
        {
            string userId = users.First().Id.ToString();
            string restaurantId = restaurants.First().Id.ToString();

            var actual = await _orderService.GetUserOrderAsync(userId, restaurantId);

            Assert.IsNotNull(actual);
            Assert.That(actual.GetType(), Is.EqualTo(typeof(Order)));
        }

        [Test]
        public async Task Test_CreateOrder_ShouldWork()
        {
            string userId = users.First().Id.ToString();
            string restaurantId = restaurants.First().Id.ToString();

            var actual = await _orderService.CreateOrderAsync(userId, restaurantId);

            Assert.IsNotNull(actual);
            Assert.That(_context.Orders.Count(), Is.EqualTo(2));
        }

        [Test]
        public async Task Test_AddItemToOrder_ShouldWork()
        {
            string userId = users.First().Id.ToString();
            string itemId = orderItems.First().Id.ToString();

            var actual = await _orderService.AddItemToOrder(userId, itemId);

            Assert.IsNotNull(actual);
            Assert.That(_context.Orders.First().OrderItems.Count, Is.EqualTo(1));
        }

        [Test]
        public async Task Test_AddItemToOrder_ShouldCreateNewOrder()
        {
            _context.Orders.Remove(orders.First());
            _context.SaveChanges();

            string userId = users.First().Id.ToString();
            string itemId = orderItems.First().Id.ToString();

            var actual = await _orderService.AddItemToOrder(userId, itemId);

            Assert.IsNotNull(actual);
            Assert.That(_context.Orders.Count(), Is.EqualTo(1));
        }

        [Test]
        public async Task Test_AddItemToOrder_ShouldThrowExceptionWhenTryToAddSameProduct()
        {
            string userId = users.First().Id.ToString();
            string itemId = orderItems.First().Id.ToString();

            var actual = await _orderService.AddItemToOrder(userId, itemId);

            Assert.ThrowsAsync<InvalidOperationException>(async () =>
            {
                await _orderService.AddItemToOrder(userId, itemId);
            });
        }

        [Test]
        public async Task Test_AddItemToOrder_ShouldThrowExceptionWhenTryToAddSameOffer()
        {
            string userId = users.First().Id.ToString();
            string itemId = orderItems.Last().Id.ToString();

            var actual = await _orderService.AddItemToOrder(userId, itemId);

            Assert.ThrowsAsync<InvalidOperationException>(async () =>
            {
                await _orderService.AddItemToOrder(userId, itemId);
            });
        }

        [Test]
        public async Task Test_GetOrderDetails_ShouldReturnOrderVeiwModel()
        {
            string orderId = orders.First().Id.ToString();

            var actual = await _orderService.GetOrderDetails(orderId);

            Assert.IsNotNull(actual);
            Assert.That(actual.GetType(), Is.EqualTo(typeof(OrderViewModel)));
        }
    }
}
