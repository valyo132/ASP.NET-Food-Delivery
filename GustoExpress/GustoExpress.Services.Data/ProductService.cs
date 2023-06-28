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

        public async Task<T> ProjectToModel<T>(string id)
        {
            Product product = await GetByIdAsync(id);
            T model = ProjectTo<T>(product);

            return model;
        }

        public async Task<Product> GetByIdAsync(string id)
        {
            return await _context.Products.FirstOrDefaultAsync(p => p.Id.ToString() == id);
        }

        public async Task<ProductViewModel> CreateProductAsync(CreateProductViewModel model)
        {
            Product newProduct = _mapper.Map<Product>(model);

            if (await CheckIfProductExists(newProduct.Name, newProduct.RestaurantId.ToString()))
            {
                throw new InvalidOperationException("An item with this name already exists!");
            }

            await _context.Products.AddAsync(newProduct);
            await _restaurantService.AddProductAsync(newProduct);

            await _context.SaveChangesAsync();

            return ProjectTo<ProductViewModel>(newProduct);
        }

        public async Task<ProductViewModel> EditProductAsync(string id, CreateProductViewModel model)
        {
            Product product = await GetByIdAsync(id);
            product.Name = model.Name;
            product.Description = model.Description;
            product.Category = model.Category;
            product.Price = model.Price;
            product.Grams = model.Grams;
            product.Discount = model.Discount;

            await _context.SaveChangesAsync();

            return ProjectTo<ProductViewModel>(product);
        }

        public async Task<ProductViewModel> DeleteAsync(string id)
        {
            Product product = await GetByIdAsync(id);
            product.IsDeleted = true;

            await _context.SaveChangesAsync();

            return ProjectTo<ProductViewModel>(product);
        }

        private async Task<bool> CheckIfProductExists(string name, string resntaurantId)
        {
            return await _context.Products
                .AnyAsync(p => p.Name.ToLower() == name.ToLower() && p.IsDeleted == false && p.RestaurantId.ToString() == resntaurantId);
        }

        public async Task SaveImageURL(string url, ProductViewModel productVm)
        {
            Product product = await GetByIdAsync(productVm.Id.ToString());
            product.ImageURL = url;

            await _context.SaveChangesAsync();
        }

        private T ProjectTo<T>(Product product)
        {
            return _mapper.Map<T>(product);
        }
    }
}
