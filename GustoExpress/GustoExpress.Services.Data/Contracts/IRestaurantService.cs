using GustoExpress.Web.ViewModels;

namespace GustoExpress.Services.Data.Contracts
{
    public interface IRestaurantService
    {
        Task<List<AllRestaurantViewModel>> All(string city);
    }
}
