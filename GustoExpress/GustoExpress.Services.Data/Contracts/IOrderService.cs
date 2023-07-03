namespace GustoExpress.Services.Data.Contracts
{
    using GustoExpress.Data.Models;
    using GustoExpress.Web.ViewModels;

    public interface IOrderService
    {
        Task<Order> CreateOrderAsync(string userId, string restaurantId);
        Task<OrderViewModel> GetOrderToComplete(string userId, string restaurantId);
        Task<Order> GetOrderByIdAsync(string orderId);
        Task CompleteOrder(string userId, string restaurantId);
        Task<Order> GetUserOrderAsync(string userId, string restaurantId);
        Task<OrderViewModel> AddItemToOrder(string userId, string itemId);
    }
}
