using AutoMapper;
using GustoExpress.Data.Models;
using GustoExpress.Services.Data.Contracts;
using GustoExpress.Web.Data;
using GustoExpress.Web.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace GustoExpress.Services.Data
{
    public class ProductService : IProductService
    {
        private readonly ApplicationDbContext _context;
        private readonly IRestaurantService _restaurantService;
        private readonly IMapper _mapper;

        public ProductService(ApplicationDbContext context,
            IMapper mapper,
            IRestaurantService restaurantService)
        {
            _context = context;
            _mapper = mapper;
            _restaurantService = restaurantService;
        }

        public async Task<Product> GetById(string id)
        {
            return await _context.Products.FirstOrDefaultAsync(p => p.Id.ToString() == id);
        }

        public T ProjectTo<T>(Product product)
        {
            return _mapper.Map<T>(product);
        }

        public async Task<Product> CreateProduct(CreateProductViewModel model)
        {
            Product newProduct = _mapper.Map<Product>(model);
            await _context.Products.AddAsync(newProduct);
            await _restaurantService.AddProductAsync(newProduct);

            await _context.SaveChangesAsync();

            return newProduct;
        }

        public async Task<Product> EditProduct(string id, CreateProductViewModel model)
        {
            Product product = await GetById(id);
            product.Name = model.Name;
            product.Description = model.Description;
            product.Category = model.Category;
            product.Price = model.Price;
            product.Grams = model.Grams;

            await _context.SaveChangesAsync();

            return product;
        }

        public async Task<Product> DeleteAsync(string id)
        {
            Product product = await GetById(id);
            product.IsDeleted = true;

            await _context.SaveChangesAsync();

            return product;
        }

        public async Task SaveImageURL(string url, Product product)
        {
            product.ImageURL = url;
            await _context.SaveChangesAsync();
        }
    }
}
