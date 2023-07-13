
namespace GustoExpress.Services.Data.Contracts
{
    using Microsoft.AspNetCore.Mvc.Rendering;

    using GustoExpress.Data.Models;
    using GustoExpress.Web.ViewModels;

    public interface IOfferService
    {
        Task<bool> HasOfferWithId(string id);
        Task<T> ProjectToModel<T>(string id);
        Task<Offer> GetByIdAsync(string id);
        Task<IEnumerable<SelectListItem>> GetProductsByRestaurantIdAsync(string id);
        Task<OfferViewModel> CreateOfferAsync(string restaurantId, CreateOfferViewModel model);
        Task<OfferViewModel> EditOfferAsync(string id, CreateOfferViewModel model);
        Task<OfferViewModel> DeleteOfferAsync(string id);
        Task SaveImageURL(string imageURL, OfferViewModel offer);
    }
}
