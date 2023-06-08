using GustoExpress.Data.Models;
using GustoExpress.Web.ViewModels;

namespace GustoExpress.Services.Data.Contracts
{
    public interface IProductService
    {
        Task<Product> GetByIdAsync(string id);

        T ProjectTo<T>(Product product);
        Task<Product> CreateProductAsync(CreateProductViewModel model);

        Task<Product> EditProductAsync(string id, CreateProductViewModel model);
        Task<Product> DeleteAsync(string id);
        Task SaveImageURL(string url, Product product);
    }
}
