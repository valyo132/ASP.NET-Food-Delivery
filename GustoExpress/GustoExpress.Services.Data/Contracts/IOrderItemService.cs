namespace GustoExpress.Services.Data.Contracts
{
    using GustoExpress.Data.Models;
    using GustoExpress.Web.ViewModels;

    public interface IOrderItemService
    {
        Task<bool> HasOrderItemWithId(string id);
        Task<string> GetRestaurantIdAsync(string id);
        Task<OrderItem> GetOrderItemByIdAsync(string itemId);
        Task<object> GetObjectAsync(string objId);
        CreateOrderItemViewModel GetOrderItemViewModel(object obj);
        Task<OrderItemViewModel> CreateOrderItemAsync(CreateOrderItemViewModel model);
    }
}
