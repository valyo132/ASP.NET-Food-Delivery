using GustoExpress.Data.Models;
using GustoExpress.Web.ViewModels;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace GustoExpress.Services.Data.Contracts
{
    public interface IRestaurantService
    {
        Task<T> ProjectToModel<T>(string id);
        Task<Restaurant> GetByIdAsync(string id);
        Task<AllRestaurantViewModel> AllAsync(string city, AllRestaurantViewModel? model);
        Task<RestaurantViewModel> CreateAsync(CreateRestaurantViewModel model);
        Task<RestaurantViewModel> EditRestaurantAsync(string id, CreateRestaurantViewModel model);
        Task<RestaurantViewModel> DeleteAsync(string id);
        IEnumerable<SelectListItem> GetRestaurantSortingValues();
        Task SaveImageURL(string url, RestaurantViewModel restaurant);
    }
}
