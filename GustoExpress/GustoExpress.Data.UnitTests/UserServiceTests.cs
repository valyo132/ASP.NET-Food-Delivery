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
    public class UserServiceTests
    {
        private IUserService _userService;
        private ApplicationDbContext _context;
        private IMapper _mapper;

        private IEnumerable<ApplicationUser> users;

        [SetUp]
        public void SetUp()
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

            users = new List<ApplicationUser>() { user };

            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: "GustoExpressInMemory")
                .Options;
            _context = new ApplicationDbContext(options);

            _context.Database.EnsureDeleted();

            _context.ApplicationUsers.AddRange(users);

            _context.SaveChanges();

            var configuration = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<GustoExpressProfile>();
            });
            _mapper = configuration.CreateMapper();

            _userService = new UserService(_context, _mapper);
        }

        [Test]
        public async Task Test_GetUserEmailByUsername_ShouldWork()
        {
            string username = "Best Admin";

            string actual = await _userService.GetUserEmailByUsername(username);
            string expected = "admin@admin.com";

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public async Task Test_AllUsersAsync_ShouldWork()
        {
            var expected = await _userService.AllUsersAsync();

            Assert.That(expected.All(u => u.GetType() == typeof(UserViewModel)));
            Assert.That(expected.Count, Is.EqualTo(1));
        }
    }
}
