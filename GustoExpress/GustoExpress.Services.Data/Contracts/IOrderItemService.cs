using GustoExpress.Data.Models;
using GustoExpress.Web.ViewModels;

namespace GustoExpress.Services.Data.Contracts
{
    public interface IOrderItemService
    {
        Task<string> GetRestaurantIdAsync(string id);
        Task<OrderItem> GetOrderItemByIdAsync(string itemId);
        Task<object> GetObjectAsync(string objId);
        CreateOrderItemViewModel GetOrderItemViewModel(object obj);
        Task<OrderItemViewModel> CreateOrderItemAsync(CreateOrderItemViewModel model);
    }
}
