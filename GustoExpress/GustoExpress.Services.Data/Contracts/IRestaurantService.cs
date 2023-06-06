﻿using GustoExpress.Data.Models;
using GustoExpress.Web.ViewModels;

namespace GustoExpress.Services.Data.Contracts
{
    public interface IRestaurantService
    {
        Task<List<AllRestaurantViewModel>> All(string city);
        Task<Restaurant> CreateAsync(CreateRestaurantViewModel model);
        Task<Restaurant> DeleteAsync(string id);
        Task SaveImageURL(string url, Restaurant restaurant);
        Task<Restaurant> GetByIdAsync(string id);
        T ProjectTo<T>(Restaurant restaurant);
        Task AddProductAsync(Product product);
    }
}
