using AutoMapper;
using GustoExpress.Data.Models;
using GustoExpress.Services.Data.Contracts;
using GustoExpress.Services.Mapping;
using GustoExpress.Web.Data;
using GustoExpress.Web.ViewModels;
using Microsoft.EntityFrameworkCore;
using System.Security.Permissions;

namespace GustoExpress.Services.Data.UnitTests
{
    [TestFixture]
    public class OfferServiceTests
    {
        private IEnumerable<Offer> offers;
        private IEnumerable<Product> products;
        private IEnumerable<OfferProduct> offerProducts;

        private ApplicationDbContext _context;
        private IMapper _mapper;
        private IRestaurantService _restaurantService;
        private IProductService _productService;
        private ICityService _cityService;
        private IOfferService _offerService;

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
                    Id = Guid.Parse("ac5d914f-e8d7-42b7-b451-fa1cf598a5c6"),
                    Name = "Test product",
                    Description = "This is test product",
                    Category = GustoExpress.Data.Models.Enums.Category.Meat,
                    Price = 10m,
                    RestaurantId = restaurant.Id,
                    Restaurant = restaurant,
                    Grams = 200m
                },
                new Product()
                {
                    Id = Guid.Parse("2ecac7ee-f9b6-40d2-8d2e-e505827de26f"),
                    Name = "Second test product",
                    Description = "This is test product",
                    Category = GustoExpress.Data.Models.Enums.Category.Meat,
                    Price = 10m,
                    RestaurantId = restaurant.Id,
                    Restaurant = restaurant,
                    Grams = 200m
                },
                new Product()
                {
                    Id = Guid.Parse("e98302f4-ebb4-4d59-80ba-aeedd755ebbb"),
                    Name = "Third test product",
                    Description = "This is test product",
                    Category = GustoExpress.Data.Models.Enums.Category.Meat,
                    Price = 10m,
                    RestaurantId = restaurant.Id,
                    Restaurant = restaurant,
                    Grams = 200m
                }
            };

            offers = new List<Offer>()
            {
                new Offer()
                {
                    Id = Guid.NewGuid(),
                    Name = "Test offer",
                    Description = "This is test offer",
                    RestaurantId = restaurant.Id,
                    Restaurant = restaurant,
                    Price = 10m,
                    Discount = 0m,
                }
            };

            offerProducts = new List<OfferProduct>()
            {
                new OfferProduct()
                {
                    Id = Guid.NewGuid(),
                    ProductId = Guid.Parse("e98302f4-ebb4-4d59-80ba-aeedd755ebbb"),
                    Offer = offers.First(),
                    OfferId = offers.First().Id
                },
                new OfferProduct()
                {
                    Id = Guid.NewGuid(),
                    ProductId = Guid.Parse("2ecac7ee-f9b6-40d2-8d2e-e505827de26f"),
                    Offer = offers.First(),
                    OfferId = offers.First().Id
                },
                new OfferProduct()
                {
                    Id = Guid.NewGuid(),
                    ProductId = Guid.Parse("ac5d914f-e8d7-42b7-b451-fa1cf598a5c6"),
                    Offer = offers.First(),
                    OfferId = offers.First().Id
                },
            };

            offers.First().OfferProducts = new List<OfferProduct>() { offerProducts.First(), offerProducts.Last() };

            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(databaseName: "GustoExpressInMemory")
               .Options;
            _context = new ApplicationDbContext(options);

            _context.Database.EnsureDeleted();

            await _context.Products.AddRangeAsync(products);
            await _context.Restaurants.AddAsync(restaurant);
            await _context.Offers.AddRangeAsync(offers);
            await _context.OfferProducts.AddRangeAsync(offerProducts);
            await _context.SaveChangesAsync();

            var configuration = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<GustoExpressProfile>();
            });
            _mapper = configuration.CreateMapper();

            _cityService = new CityService(_context);
            _restaurantService = new RestaurantService(_context, _mapper, _cityService);
            _productService = new ProductService(_context, _mapper, _restaurantService);
            _offerService = new OfferService(_context, _mapper, _productService, _restaurantService);
        }

        [Test]
        public void Test_Constructor_ShouldWork()
        {
            Assert.IsNotNull(_offerService);
            Assert.IsNotNull(_productService);
            Assert.IsNotNull(_restaurantService);
            Assert.IsNotNull(_cityService);
            Assert.IsNotNull(_mapper);
        }

        [Test]
        public async Task Test_HasOfferWithId_ShouldReturnTrue()
        {
            string id = offers.First().Id.ToString();

            bool actual = await _offerService.HasOfferWithId(id);

            Assert.True(actual);
        }

        [Test]
        public async Task Test_HasOfferWithId_ShouldReturnFalse()
        {
            string id = "ecd68e73-b904-4abf-b49d-7f6d4490afa5";

            bool actual = await _offerService.HasOfferWithId(id);

            Assert.False(actual);
        }

        [Test]
        public async Task Test_GetById_ShouldWork()
        {
            string id = offers.First().Id.ToString();

            var actual = await _offerService.GetByIdAsync(id);
            var expected = offers.First();

            Assert.IsNotNull(actual);
            Assert.That(actual, Is.EqualTo(expected));
        }

        [Test]
        public async Task Test_GetById_ShouldReturnNull()
        {
            string id = Guid.NewGuid().ToString();

            var actual = await _offerService.GetByIdAsync(id);

            Assert.IsNull(actual);
        }

        [Test]
        public async Task Test_ProjectToModel_ShouldWork()
        {
            var offerId = offers.First().Id.ToString();
            var offerViewModel = await _offerService.ProjectToModel<OfferViewModel>(offerId);
            var createOfferViewModel = await _offerService.ProjectToModel<CreateOfferViewModel>(offerId);

            Assert.IsNotNull(offerViewModel);
            Assert.That(typeof(OfferViewModel), Is.EqualTo(offerViewModel.GetType()));
            Assert.IsNotNull(createOfferViewModel);
            Assert.That(typeof(CreateOfferViewModel), Is.EqualTo(createOfferViewModel.GetType()));
        }

        [Test]
        public async Task Test_GetProductsByRestaurantId_ShouldWork()
        {
            string restaurantId = offers.First().RestaurantId.ToString();
            var restaurant = await _restaurantService.GetByIdAsync(restaurantId);

            var actual = await _offerService.GetProductsByRestaurantIdAsync(restaurantId);

            Assert.IsNotNull(actual);
            var products = actual.ToList();
            Assert.That(restaurant.Products.Count, Is.EqualTo(products.Count));
            Assert.That(products.Any(p => p.Text == "Test product"));
        }

        [Test]
        public async Task Test_CreateOffer_SholdWork()
        {
            string restaurantId = offers.First().RestaurantId.ToString();
            var restaurant = await _restaurantService.GetByIdAsync(restaurantId);

            var mdoel = new CreateOfferViewModel()
            {
                Name = "New Test offer",
                Description = "Test Description",
                FirstProductId = "ac5d914f-e8d7-42b7-b451-fa1cf598a5c6",
                SecondProductId = "e98302f4-ebb4-4d59-80ba-aeedd755ebbb",
                ThirdhProductId = "2ecac7ee-f9b6-40d2-8d2e-e505827de26f",
                RestaurantId = restaurantId,
                ProductsToChoose = await _offerService.GetProductsByRestaurantIdAsync(restaurantId),
                Price = 10m,
                Discount = 1m,
            };

            var createdOffer = await _offerService.CreateOfferAsync(restaurantId, mdoel);

            Assert.IsNotNull(createdOffer);
            Assert.That(createdOffer.GetType(), Is.EqualTo(typeof(OfferViewModel)));
            Assert.That(restaurant.Offers.Count, Is.EqualTo(2));
        }

        [Test]
        public async Task Test_CreateOfferShouldTrowInvalidOperationException()
        {
            string restaurantId = _context.Restaurants.First().Id.ToString();

            var model = new CreateOfferViewModel()
            {
                Name = "Test offer",
                Description = "Test Description",
                FirstProductId = "ac5d914f-e8d7-42b7-b451-fa1cf598a5c6",
                SecondProductId = "e98302f4-ebb4-4d59-80ba-aeedd755ebbb",
                ThirdhProductId = "2ecac7ee-f9b6-40d2-8d2e-e505827de26f",
                RestaurantId = restaurantId,
                ProductsToChoose = await _offerService.GetProductsByRestaurantIdAsync(restaurantId),
                Price = 10m,
                Discount = 1m,
            };

            Assert.ThrowsAsync<InvalidOperationException>(async () =>
            {
                await _offerService.CreateOfferAsync(restaurantId, model);
            });
        }

        [Test]
        public async Task Test_EditOffer_ShouldWork()
        {
            var offer = offers.First();
            string restaurantId = offers.First().RestaurantId.ToString();

            var mdoel = new CreateOfferViewModel()
            {
                Name = "Edited test offer",
                Description = "Test Description",
                FirstProductId = "ac5d914f-e8d7-42b7-b451-fa1cf598a5c6",
                SecondProductId = "e98302f4-ebb4-4d59-80ba-aeedd755ebbb",
                ThirdhProductId = "2ecac7ee-f9b6-40d2-8d2e-e505827de26f",
                RestaurantId = restaurantId,
                ProductsToChoose = await _offerService.GetProductsByRestaurantIdAsync(restaurantId),
                Price = 10m,
                Discount = 1m,
            };

            var editedOffer = await _offerService.EditOfferAsync(offer.Id.ToString(), mdoel);

            Assert.IsNotNull(editedOffer);
            Assert.That(editedOffer.Id, Is.EqualTo(offer.Id));
            Assert.That(_context.Offers.Count(), Is.EqualTo(1));
            Assert.That(offer.Name, Is.EqualTo("Edited test offer"));
            Assert.IsFalse(_context.Offers.Any(o => o.Name == "Test offer"));
        }

        [Test]
        public async Task Test_DeleteOffer_ShouldWork()
        {
            var offer = offers.First();

            var deletedOffer = await _offerService.DeleteOfferAsync(offer.Id.ToString());

            Assert.IsNotNull(deletedOffer);
            Assert.IsTrue(deletedOffer.IsDeleted);
        }

        [Test]
        public async Task Test_SaveImgUrl_ShouldWork()
        {
            string url = "Test url";
            var offer = offers.First();
            var offerViewModel = await _offerService.ProjectToModel<OfferViewModel>(offer.Id.ToString());

            await _offerService.SaveImageURL(url, offerViewModel);

            Assert.IsNotNull(offer.ImageURL);
            Assert.That(offer.ImageURL, Is.EqualTo("Test url"));
        }
    }
}
