﻿namespace GustoExpress.Services.Data
{
    using Microsoft.EntityFrameworkCore;

    using GustoExpress.Data.Models;
    using GustoExpress.Services.Data.Contracts;
    using GustoExpress.Web.Data;

    public class CityService : ICityService
    {
        private readonly ApplicationDbContext _context;

        public CityService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<City> CreateCityAsync(string cityName)
        {
            City newCity = new City();
            newCity.CityName = cityName;

            await _context.Cities.AddAsync(newCity);
            await _context.SaveChangesAsync();

            return newCity;
        }

        public async Task<City> GetCityAsync(string cityName)
        {
            return await _context.Cities
                .FirstOrDefaultAsync(x => x.CityName.ToLower() == cityName.ToLower());
        }
    }
}
