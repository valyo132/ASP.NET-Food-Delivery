using GustoExpress.Data.Models;
using GustoExpress.Web.ViewModels;

namespace GustoExpress.Services.Data.Contracts
{
    public interface IProductService
    {
        Task<Product> CreateProduct(CreateProductViewModel model);
        Task SaveImageURL(string url, Product product);
    }
}
