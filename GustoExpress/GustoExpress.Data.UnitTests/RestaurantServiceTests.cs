using AutoMapper;
using GustoExpress.Data.Models;
using GustoExpress.Services.Data.Contracts;
using GustoExpress.Services.Mapping;
using GustoExpress.Web.Data;
using GustoExpress.Web.ViewModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Moq;

namespace GustoExpress.Services.Data.UnitTests
{
    [TestFixture]
    public class RestaurantServiceTests
    {
        private IEnumerable<Restaurant> restaurants;
        private ApplicationDbContext _context;
        private IMapper _mapper;

        [SetUp]
        public void Setup()
        {
            var restaurant = new Restaurant()
            {
                    Id = new Guid(),
                    CityId = new Guid(),
                    City = new City() { Id = new Guid(), CityName = "TestCity" },
                    DeliveryPrice = 2,
                    Description = "Test description",
                    Name = "Test restaurant", 
                    TimeToDeliver = "20-30"
            };

            restaurants = new List<Restaurant>() { restaurant };

            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: "GustoExpressInMemory")
                .Options;
            _context = new ApplicationDbContext(options);

            _context.Restaurants.RemoveRange(_context.Restaurants);
            _context.SaveChanges();

            _context.Restaurants.Add(restaurant);
            _context.SaveChanges();

            var configuration = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<GustoExpressProfile>();
            });
            _mapper = configuration.CreateMapper();
        }

        [Test]
        public async Task Test_GetRestaurantByIdShouldWork()
        {
            var id = restaurants.First().Id;
            var _cityService = new Mock<ICityService>();
            var _restaurantService = new RestaurantService(_context, _mapper, _cityService.Object);

            var actualResult = await _restaurantService.GetByIdAsync(id.ToString());

            var expected = restaurants
                .FirstOrDefault(r => r.Id.ToString() == id.ToString());

            Assert.IsNotNull(actualResult);
            Assert.That(actualResult, Is.EqualTo(expected));
            Assert.That(actualResult.Id, Is.EqualTo(expected.Id));
        }

        [Test]
        public async Task Test_AllAsyncMethodShouldWork()
        {
            var _cityService = new Mock<ICityService>();
            var _restaurantService = new RestaurantService(_context, _mapper, _cityService.Object);

            var actualResult = await _restaurantService.AllAsync("TestCity");

            Assert.IsNotNull(actualResult);
            Assert.That(actualResult.Count, Is.EqualTo(1));
            Assert.That(typeof(AllRestaurantViewModel), Is.EqualTo(actualResult.First().GetType()));
            Assert.That(typeof(List<AllRestaurantViewModel>), Is.EqualTo(actualResult.GetType()));
        }

        [Test]
        public async Task Test_AllAsyncMethodShouldReturnNull()
        {
            var _cityService = new Mock<ICityService>();
            var _restaurantService = new RestaurantService(_context, _mapper, _cityService.Object);

            var actualResult = await _restaurantService.AllAsync("Invalid City");

            Assert.That(0, Is.EqualTo(actualResult.Count));
        }

        [Test]
        public async Task Test_CreateRestaurantShouldWork()
        {
            var _cityService = new Mock<ICityService>();
            var _restaurantService = new RestaurantService(_context, _mapper, _cityService.Object);

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
        public async Task Test_EditRestaurantShouldWork()
        {
            var _cityService = new Mock<ICityService>();
            var _restaurantService = new RestaurantService(_context, _mapper, _cityService.Object);

            CreateRestaurantViewModel model = new CreateRestaurantViewModel()
            {
                Name = "EditedRestaurant",
                Description = "Test restaurant",
                City = "TestCity",
                DeliveryPrice = 10,
                MinTimeToDeliver = 10,
                MaxTimeToDeliver = 10
            };

            var actualResult = await _restaurantService.EditRestaurantAsync(restaurants.First().Id.ToString(), model);

            Assert.AreEqual(1, _context.Restaurants.Count());
            Assert.IsNotNull(_context.Restaurants.FirstOrDefault(r => r.Name == "EditedRestaurant"));
            Assert.That(typeof(RestaurantViewModel), Is.EqualTo(actualResult.GetType()));
        }
    }
}
