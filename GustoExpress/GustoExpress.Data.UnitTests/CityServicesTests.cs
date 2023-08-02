namespace GustoExpress.Services.Data.UnitTests
{
    using Microsoft.EntityFrameworkCore;

    using GustoExpress.Data.Models;
    using GustoExpress.Services.Data.Contracts;
    using GustoExpress.Web.Data;

    [TestFixture]
    public class CityServicesTests
    {
        private IEnumerable<City> cities;
        private ApplicationDbContext _context;

        [SetUp]
        public void Setup()
        {
            cities = new List<City>()
            {
                new City(){ Id = new Guid(), CityName = "Sofia" },
                new City(){ Id = new Guid(), CityName = "Targovishte" },
                new City(){ Id = new Guid(), CityName = "Plovdiv" }
            };

            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: "GustoExpressInMemory")
                .Options;

            _context = new ApplicationDbContext(options);
            _context.AddRange(this.cities);
            _context.SaveChanges();
        }

        [TestCase("Sofia")]
        [TestCase("SOFIA")]
        [TestCase("sofia")]
        [TestCase("SoFiA")]
        public async Task Test_GetCityByName(string name)
        {
            ICityService _service = new CityService(_context);
            var serviceResult = await _service.GetCityAsync(name);

            var dbCity = cities.FirstOrDefault(c => c.CityName.ToLower() == name.ToLower());

            Assert.IsNotNull(serviceResult);
            Assert.That(dbCity.CityName, Is.EqualTo(serviceResult.CityName));
        }

        [Test]
        public async Task Test_AddCityToDb()
        {
            var name = "Test";

            ICityService _service = new CityService(_context);
            var result = await _service.CreateCityAsync(name);

            Assert.IsNotNull(result);
            Assert.That(result.CityName, Is.EqualTo(name));
            Assert.That(_context.Cities.Count(), Is.EqualTo(4));
        }
    }
}