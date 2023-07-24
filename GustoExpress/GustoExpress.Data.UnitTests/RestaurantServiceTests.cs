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

            var product = new Product()
            {
                Id = Guid.NewGuid(),
                Name = "Test product",
                Description = "This is test product",
                Category = GustoExpress.Data.Models.Enums.Category.Meat,
                Price = 10m,
                RestaurantId = restaurant.Id,
                Restaurant = restaurant,
                Grams = 200m
            };

            var offer = new Offer()
            {
                Id = Guid.NewGuid(),
                Name = "Test product",
                Description = "This is test product",
                Price = 10m,
                RestaurantId = restaurant.Id,
                Restaurant = restaurant,
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

            restaurants = new List<Restaurant>() { restaurant };
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

            Assert.That(actualResult.Restaurants.Count, Is.EqualTo(1));
            Assert.That(typeof(RestaurantViewModel), Is.EqualTo(actualResult.Restaurants.First().GetType()));
            Assert.That(typeof(AllRestaurantViewModel), Is.EqualTo(actualResult.GetType()));
        }

        [Test]
        public async Task Test_AllAsyncMethod_ShouldReturnZero()
        {
            var actualResult = await _restaurantService.AllAsync("Invalid City", new AllRestaurantViewModel());

            Assert.That(0, Is.EqualTo(actualResult.Restaurants.Count));
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

            Assert.That(2, Is.EqualTo(_context.Restaurants.Count()));
            Assert.IsNotNull(_context.Restaurants.FirstOrDefault(r => r.Name == "Test"));
            Assert.That(typeof(RestaurantViewModel), Is.EqualTo(actualResult.GetType()));
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
