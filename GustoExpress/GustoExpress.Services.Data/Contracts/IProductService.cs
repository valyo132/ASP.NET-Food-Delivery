using GustoExpress.Data.Models;
using GustoExpress.Web.ViewModels;

namespace GustoExpress.Services.Data.Contracts
{
    public interface IProductService
    {
        Task<Product> GetById(string id);

        T ProjectTo<T>(Product product);
        Task<Product> CreateProduct(CreateProductViewModel model);

        Task<Product> EditProduct(string id, CreateProductViewModel model);
        Task<Product> DeleteAsync(string id);
        Task SaveImageURL(string url, Product product);
    }
}
