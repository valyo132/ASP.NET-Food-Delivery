﻿namespace GustoExpress.Services.Data.Contracts
{
    using GustoExpress.Data.Models;
    using GustoExpress.Web.ViewModels;

    public interface IProductService
    {
        Task<bool> HasProductWithId(string id);
        Task<Product> GetByIdAsync(string id);
        Task<ProductViewModel> CreateProductAsync(CreateProductViewModel model);
        Task<ProductViewModel> EditProductAsync(string id, CreateProductViewModel model);
        Task<ProductViewModel> DeleteAsync(string id);
        Task<T> ProjectToModel<T>(string id);
        Task SaveImageURL(string url, ProductViewModel product);
    }
}
