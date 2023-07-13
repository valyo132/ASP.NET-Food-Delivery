namespace GustoExpress.Services.Data.Contracts
{
    using GustoExpress.Data.Models;
    using GustoExpress.Web.ViewModels;

    public interface IRestaurantService
    {
        Task<bool> HasRestaurantWithId(string id);
        Task<T> ProjectToModel<T>(string id);
        Task<Restaurant> GetByIdAsync(string id);
        Task<AllRestaurantViewModel> AllAsync(string city, AllRestaurantViewModel? model);
        Task<RestaurantViewModel> CreateAsync(CreateRestaurantViewModel model);
        Task<RestaurantViewModel> EditRestaurantAsync(string id, CreateRestaurantViewModel model);
        Task<RestaurantViewModel> DeleteAsync(string id);
        Task SaveImageURL(string url, RestaurantViewModel restaurant);
    }
}
