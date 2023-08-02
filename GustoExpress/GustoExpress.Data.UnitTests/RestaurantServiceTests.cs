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
    public class RestaurantServiceTests
    {
        private IEnumerable<Restaurant> restaurants;
        private IEnumerable<Product> products;
        private IEnumerable<Offer> offers;
        private IEnumerable<OfferProduct> offerProducts;
        private ApplicationDbContext _context;
        private IMapper _mapper;
        private IRestaurantService _restaurantService;

        [SetUp]
        public async Task Setup()
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

            var secondRestaurant = new Restaurant()
            {
                Id = Guid.NewGuid(),
                CityId = Guid.NewGuid(),
                City = new City() { Id = Guid.NewGuid(), CityName = "TestCity" },
                DeliveryPrice = 5,
                Description = "Test description",
                Name = "Second test restaurant",
                TimeToDeliver = "20-40"
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

            offerProducts = new List<OfferProduct>()
            {
                new OfferProduct() { Id = Guid.NewGuid(), OfferId =  offer.Id, Offer = offer, Product = product, ProductId = product.Id },
                new OfferProduct() { Id = Guid.NewGuid(), OfferId =  offer.Id, Offer = offer, Product = product, ProductId = product.Id },
                new OfferProduct() { Id = Guid.NewGuid(), OfferId =  offer.Id, Offer = offer, Product = product, ProductId = product.Id }
            };
            offer.OfferProducts = offerProducts.ToList();

            restaurants = new List<Restaurant>() { firstRestaurant, secondRestaurant };
            products = new List<Product>() { product };
            offers = new List<Offer>() { offer };

            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: "GustoExpressInMemory")
                .Options;
            _context = new ApplicationDbContext(options);

            _context.Database.EnsureDeleted();

            await _context.Restaurants.AddRangeAsync(restaurants);
            await _context.Products.AddRangeAsync(products);
            await _context.Offers.AddRangeAsync(offers);
            await _context.SaveChangesAsync();

            var configuration = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<GustoExpressProfile>();
            });
            _mapper = configuration.CreateMapper();

            var _cityService = new CityService(_context);
            _restaurantService = new RestaurantService(_context, _mapper, _cityService);
        }

        [Test]
        public async Task Test_ProjectToModel_ShouldWork()
        {
            var restaurantId = restaurants.First().Id.ToString();

            var allRestaurantViewModel = await _restaurantService.ProjectToModel<AllRestaurantViewModel>(restaurantId);
            var restaurantViewModel = await _restaurantService.ProjectToModel<RestaurantViewModel>(restaurantId);
            var createRestaurantViewModel = await _restaurantService.ProjectToModel<CreateRestaurantViewModel>(restaurantId);
            var restaurantPageViewModel = await _restaurantService.ProjectToModel<RestaurantPageViewModel>(restaurantId);

            Assert.That(allRestaurantViewModel.GetType(), Is.EqualTo(typeof(AllRestaurantViewModel)));
            Assert.That(restaurantViewModel.GetType(), Is.EqualTo(typeof(RestaurantViewModel)));
            Assert.That(createRestaurantViewModel.GetType(), Is.EqualTo(typeof(CreateRestaurantViewModel)));
            Assert.That(restaurantPageViewModel.GetType(), Is.EqualTo(typeof(RestaurantPageViewModel)));
        }

        [Test]
        public async Task Test_HasRestaurantWithId_ShouldReturnTrue()
        {
            string restaurantId = restaurants.First().Id.ToString();

            bool actual = await _restaurantService.HasRestaurantWithId(restaurantId);

            Assert.True(actual);
        }

        [Test]
        public async Task Test_HasRestaurantWithId_ShouldReturnFalse()
        {
            string restaurantId = "6ec3c5cc-ac13-40a7-b089-f2efa8636609";

            bool actual = await _restaurantService.HasRestaurantWithId(restaurantId);

            Assert.False(actual);
        }

        [Test]
        public async Task Test_GetRestaurantById_ShouldWork()
        {
            var id = restaurants.First().Id;

            var actualResult = await _restaurantService.GetByIdAsync(id.ToString());

            var expected = restaurants
                .FirstOrDefault(r => r.Id.ToString() == id.ToString());

            Assert.IsNotNull(actualResult);
            Assert.That(actualResult, Is.EqualTo(expected));
            Assert.That(actualResult.Id, Is.EqualTo(expected.Id));
        }

        [Test]
        public async Task Test_AllAsyncMethod_ShouldWork()
        {
            var actualResult = await _restaurantService.AllAsync("TestCity", new AllRestaurantViewModel());

            Assert.That(actualResult.Restaurants.Count, Is.EqualTo(2));
            Assert.That(typeof(RestaurantViewModel), Is.EqualTo(actualResult.Restaurants.First().GetType()));
            Assert.That(typeof(AllRestaurantViewModel), Is.EqualTo(actualResult.GetType()));
        }

        [Test]
        public async Task Test_AllAsyncMethod_ShouldReturnZero()
        {
            var actualResult = await _restaurantService.AllAsync("Invalid City", new AllRestaurantViewModel());

            Assert.That(0, Is.EqualTo(actualResult.Restaurants.Count));
        }

        [TestCase("second")]
        [TestCase("SECOND")]
        [TestCase("SeCoNd")]
        public async Task Test_SearchingSystemInAllAsync_ShouldWork(string search)
        {
            AllRestaurantViewModel model = new AllRestaurantViewModel()
            {
                SearchString = search
            };

            var actual = await _restaurantService.AllAsync("TestCity", model);

            Assert.That(actual.Restaurants.Count, Is.EqualTo(1));
        }

        [Test]
        public async Task Test_OrderBYPriceAscInAll_ShouldReturnRestaurantsInCorrectOrder()
        {
            AllRestaurantViewModel model = new AllRestaurantViewModel()
            {
                Sort = Web.ViewModels.Enums.Restaurant.RestaurantSorting.DeliveryPriceAscending
            };

            var actual = await _restaurantService.AllAsync("TestCity", model);

            Assert.That(actual.Restaurants.Count, Is.EqualTo(2));
            Assert.That(actual.Restaurants.First().Name, Is.EqualTo("First test restaurant"));
            Assert.That(actual.Restaurants.Last().Name, Is.EqualTo("Second test restaurant"));
        }

        [Test]
        public async Task Test_OrderBYPriceDescInAll_ShouldReturnRestaurantsInCorrectOrder()
        {
            AllRestaurantViewModel model = new AllRestaurantViewModel()
            {
                Sort = Web.ViewModels.Enums.Restaurant.RestaurantSorting.DeliveryPriceDescending
            };

            var actual = await _restaurantService.AllAsync("TestCity", model);

            Assert.That(actual.Restaurants.Count, Is.EqualTo(2));
            Assert.That(actual.Restaurants.First().Name, Is.EqualTo("Second test restaurant"));
            Assert.That(actual.Restaurants.Last().Name, Is.EqualTo("First test restaurant"));
        }

        [Test]
        public async Task Test_OriderByTimeToDeliver_ShouldReturnRestaurantsInCorrectOrder()
        {
            AllRestaurantViewModel model = new AllRestaurantViewModel()
            {
                Sort = Web.ViewModels.Enums.Restaurant.RestaurantSorting.TimeToDeliver
            };

            var actual = await _restaurantService.AllAsync("TestCity", model);

            Assert.That(actual.Restaurants.Count, Is.EqualTo(2));
            Assert.That(actual.Restaurants.First().Name, Is.EqualTo("First test restaurant"));
            Assert.That(actual.Restaurants.Last().Name, Is.EqualTo("Second test restaurant"));
        }

        [Test]
        public async Task Test_AllWithoutCityAsync_ShouldWorkAndReturnTwoRestaurants()
        {
            var actual = await _restaurantService.AllWithoutCityAsync();

            Assert.That(actual.Restaurants.Count, Is.EqualTo(2));
        }

        [Test]
        public async Task Test_CreateRestaurant_ShouldWork()
        {
            CreateRestaurantViewModel model = new CreateRestaurantViewModel()
            {
                Name = "Test",
                Description = "Test restaurant",
                City = "TestCity",
                DeliveryPrice = 10,
                MinTimeToDeliver = 10,
                MaxTimeToDeliver = 10
            };

            var actualResult = await _restaurantService.CreateAsync(model);

            Assert.That(3, Is.EqualTo(_context.Restaurants.Count()));
            Assert.IsNotNull(_context.Restaurants.FirstOrDefault(r => r.Name == "Test"));
            Assert.That(typeof(RestaurantViewModel), Is.EqualTo(actualResult.GetType()));
        }

        [Test]
        public async Task Test_CreateRestaurantant_ShouldCreateANewCity()
        {
            CreateRestaurantViewModel model = new CreateRestaurantViewModel()
            {
                Name = "Test",
                Description = "Test restaurant",
                City = "New City",
                DeliveryPrice = 10,
                MinTimeToDeliver = 10,
                MaxTimeToDeliver = 10
            };

            await _restaurantService.CreateAsync(model);

            Assert.True(_context.Cities.Any(c => c.CityName == "New City"));
        }

        [Test]
        public async Task Test_EditRestaurantant_ShouldCreateANewCity()
        {
            Assert.False(_context.Cities.Any(c => c.CityName == "New City"));

            string restaurantId = restaurants.First().Id.ToString();

            CreateRestaurantViewModel model = new CreateRestaurantViewModel()
            {
                Name = "Test",
                Description = "Test restaurant",
                City = "New City",
                DeliveryPrice = 10,
                MinTimeToDeliver = 10,
                MaxTimeToDeliver = 10
            };

            await _restaurantService.EditRestaurantAsync(restaurantId, model);

            Assert.True(_context.Cities.Any(c => c.CityName == "New City"));
        }

        [Test]
        public async Task Test_EditRestaurant_ShouldWork()
        {
            var restaurant = restaurants.First();

            CreateRestaurantViewModel model = new CreateRestaurantViewModel()
            {
                Name = "Edited restaurant",
                Description = "Test restaurant",
                City = "TestCity",
                DeliveryPrice = 10,
                MinTimeToDeliver = 10,
                MaxTimeToDeliver = 10
            };

            var actualResult = await _restaurantService.EditRestaurantAsync(restaurant.Id.ToString(), model);

            Assert.IsNotNull(actualResult);
            Assert.That(typeof(RestaurantViewModel), Is.EqualTo(actualResult.GetType()));
            Assert.That(restaurant.Name, Is.EqualTo("Edited restaurant"));
        }

        [Test]
        public async Task Test_Delete_ShouldWork()
        {
            var deletedRestaurant = await _restaurantService.DeleteAsync(restaurants.First().Id.ToString());

            var restaurant = restaurants.First();

            Assert.IsNotNull(deletedRestaurant);
            Assert.That(1, Is.EqualTo(_context.Restaurants.Count(r => r.IsDeleted == true)));
            Assert.That(restaurant.Id, Is.EqualTo(deletedRestaurant.Id));
        }

        [Test]
        public async Task Test_SaveImageURL_ShouldWork()
        {
            var restaurant = await _restaurantService.ProjectToModel<RestaurantViewModel>(restaurants.First().Id.ToString());
            await _restaurantService.SaveImageURL("Test URL", restaurant);

            Assert.That(restaurants.First().ImageURL, Is.EqualTo("Test URL"));
        }
    }
}
