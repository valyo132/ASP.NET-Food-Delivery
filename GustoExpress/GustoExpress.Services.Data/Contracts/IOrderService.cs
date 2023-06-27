namespace GustoExpress.Services.Data.Contracts
{
    using GustoExpress.Data.Models;
    using GustoExpress.Web.ViewModels;

    public interface IOrderService
    {
        Task<Order> GetOrder(string userId, string restaurantId);
        Task<OrderViewModel> AddItemToOrder(string userId, string itemId);
    }
}
