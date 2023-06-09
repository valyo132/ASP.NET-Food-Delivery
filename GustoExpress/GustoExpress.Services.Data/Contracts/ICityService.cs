﻿using GustoExpress.Data.Models;

namespace GustoExpress.Services.Data.Contracts
{
    public interface ICityService
    {
        Task<City> CreateCityAsync(string cityName);
        Task<City> GetCityAsync(string cityName);
    }
}
