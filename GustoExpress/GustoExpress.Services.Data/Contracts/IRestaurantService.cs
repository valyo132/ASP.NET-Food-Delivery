using GustoExpress.Data.Models;
using GustoExpress.Web.ViewModels;

namespace GustoExpress.Services.Data.Contracts
{
    public interface IRestaurantService
    {
        Task<T> ProjectToModel<T>(string id);
        Task<Restaurant> GetByIdAsync(string id);
        Task<List<AllRestaurantViewModel>> AllAsync(string city);
        Task<RestaurantViewModel> CreateAsync(CreateRestaurantViewModel model);
        Task<RestaurantViewModel> EditRestaurantAsync(string id, CreateRestaurantViewModel model);
        Task<RestaurantViewModel> DeleteAsync(string id);
        Task AddProductAsync(Product product);
        Task AddOfferAsync(Offer offer);
        Task SaveImageURL(string url, RestaurantViewModel restaurant);
    }
}
