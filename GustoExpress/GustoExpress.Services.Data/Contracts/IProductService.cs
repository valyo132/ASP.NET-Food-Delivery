using GustoExpress.Data.Models;
using GustoExpress.Web.ViewModels;

namespace GustoExpress.Services.Data.Contracts
{
    public interface IProductService
    {
        Task<Product> GetById(string id);
        Task<Product> CreateProduct(CreateProductViewModel model);
        Task SaveImageURL(string url, Product product);
        Task<Product> DeleteAsync(string id);
    }
}
