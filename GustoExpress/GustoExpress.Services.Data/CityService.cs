using GustoExpress.Data.Models;
using GustoExpress.Services.Data.Contracts;
using GustoExpress.Web.Data;

namespace GustoExpress.Services.Data
{
    public class CityService : ICityService
    {
        private readonly ApplicationDbContext _context;

        public CityService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<City> CreateCity(string cityName)
        {
            City newCity = new City();
            newCity.CityName = cityName;

            await _context.Cities.AddAsync(newCity);
            await _context.SaveChangesAsync();

            return newCity;
        }
    }
}
