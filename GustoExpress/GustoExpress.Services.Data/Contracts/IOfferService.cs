using GustoExpress.Data.Models;
using GustoExpress.Web.ViewModels;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace GustoExpress.Services.Data.Contracts
{
    public interface IOfferService
    {
        Task<Offer> GetByIdAsync(string id);
        Task<IEnumerable<SelectListItem>> GetProductsByRestaurantIdAsync(string id);
        Task<Offer> CreateOffer(string restaurantId, CreateOfferViewModel model);
        Task<Offer> DeleteOfferAsync(string id);
        Task SaveImageURL(string imageURL, Offer offer);
    }
}
