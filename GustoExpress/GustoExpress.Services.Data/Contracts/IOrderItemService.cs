using GustoExpress.Data.Models;
using GustoExpress.Web.ViewModels;

namespace GustoExpress.Services.Data.Contracts
{
    public interface IOrderItemService
    {
        string GetRestaurantId(OrderItem item);
        Task<OrderItem> GetOrderItemByIdAsync(string itemId);
        Task<object> GetObjectAsync(string objId);
        CreateOrderItemViewModel GetOrderItemViewModel(object obj);
        Task<OrderItemViewModel> CreateOrderItem(CreateOrderItemViewModel model);
    }
}
