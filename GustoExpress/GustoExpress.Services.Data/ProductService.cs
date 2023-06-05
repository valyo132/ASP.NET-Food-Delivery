using AutoMapper;
using GustoExpress.Data.Models;
using GustoExpress.Services.Data.Contracts;
using GustoExpress.Web.Data;
using GustoExpress.Web.ViewModels;

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

        public async Task<Product> CreateProduct(CreateProductViewModel model)
        {
            Product newProduct = _mapper.Map<Product>(model);
            await _context.Products.AddAsync(newProduct);
            await _restaurantService.AddProduct(newProduct);

            await _context.SaveChangesAsync();

            return newProduct;
        }

        public async Task SaveImageURL(string url, Product product)
        {
            product.ImageURL = url;
            await _context.SaveChangesAsync();
        }
    }
}
