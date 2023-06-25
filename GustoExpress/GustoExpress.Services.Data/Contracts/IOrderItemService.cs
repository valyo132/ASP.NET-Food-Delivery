using GustoExpress.Web.ViewModels;

namespace GustoExpress.Services.Data.Contracts
{
    public interface IOrderItemService
    {
        Task<object> GetObjectAsync(string objId);
        CreateOrderItemViewModel GetOrderItemViewModel(object obj);
    }
}
