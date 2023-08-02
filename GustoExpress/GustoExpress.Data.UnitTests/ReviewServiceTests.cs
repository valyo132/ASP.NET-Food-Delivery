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
    public class ReviewServiceTests
    {
        private IReviewService _reviewService;
        private ApplicationDbContext _context;
        private IMapper _mapper;

        private IEnumerable<Review> reviews;
        private IEnumerable<ApplicationUser> users;
        private IEnumerable<Restaurant> restaurants;

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
                Name = "First test restaurant",
                TimeToDeliver = "20-30"
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

            var review = new Review()
            {
                Title = "Test review",
                Text = "Test test test",
                User = user,
                Restaurant = restaurant,
                Stars = 5
            };

            reviews = new List<Review>() { review };
            users = new List<ApplicationUser>() { user };
            restaurants = new List<Restaurant>() { restaurant };

            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: "GustoExpressInMemory")
                .Options;
            _context = new ApplicationDbContext(options);

            _context.Database.EnsureDeleted();

            _context.Reviews.Add(review);

            _context.SaveChanges();

            var configuration = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<GustoExpressProfile>();
            });
            _mapper = configuration.CreateMapper();

            _reviewService = new ReviewService(_context, _mapper);
        }

        [Test]
        public async Task Test_CreateReview_ShouldWork()
        {
            string userId = users.First().Id;
            var model = new CreateReviewViewModel()
            {
                Title = "New review",
                Text = "Good review",
                Stars = 4,
                RestaurantId = restaurants.First().Id.ToString(),
            };

            var actual = await _reviewService.CreateReview(userId, model);

            Assert.IsNotNull(actual);
            Assert.That(_context.Reviews.Count(), Is.EqualTo(2));
            Assert.True(_context.Reviews.Any(r => r.Title == "New review"));
        }

        [Test]
        public async Task Test_DeleteReview_ShouldWork()
        {
            string id = reviews.First().Id.ToString();

            var actual = await _reviewService.DeleteAsync(id);

            Assert.IsNotNull(actual);
            Assert.True(_context.Reviews.First().IsDeleted);
        }

        [Test]
        public async Task Test_GetByIdAsync_ShouldWork()
        {
            string id = reviews.First().Id.ToString();

            var actual = await _reviewService.GetByIdAsync(id);

            Assert.IsNotNull(actual);
            Assert.That(actual.Id.ToString(), Is.EqualTo(id));
        }
    }
}
