using AutoMapper;
using GustoExpress.Data.Models;
using GustoExpress.Services.Data.Contracts;
using GustoExpress.Web.Data;
using GustoExpress.Web.ViewModels;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace GustoExpress.Services.Data
{
    public class OfferService : IOfferService
    {
        private readonly ApplicationDbContext _context;
        private readonly IProductService _productService;
        private readonly IRestaurantService _restaurantService;
        private readonly IMapper _mapper;

        public OfferService(ApplicationDbContext context,
            IMapper mapper,
            IProductService productService,
            IRestaurantService restaurantService)
        {
            _context = context;
            _mapper = mapper;
            _productService = productService;
            _restaurantService = restaurantService;
        }

        public async Task<Offer> GetByIdAsync(string id)
        {
            return await _context.Offers
                .Include(o => o.OfferProducts)
                .FirstOrDefaultAsync(o => o.Id.ToString() == id);
        }

        public T ProjectTo<T>(Offer offer)
        {
            return _mapper.Map<T>(offer);
        }

        public async Task<IEnumerable<SelectListItem>> GetProductsByRestaurantIdAsync(string id)
        {
            return await _context.Products
                .Where(p => p.RestaurantId.ToString() == id && p.IsDeleted == false)
                .Select(e => new SelectListItem()
                {
                    Value = e.Id.ToString(),
                    Text = e.Name,
                })
                .ToListAsync();
        }

        public async Task<Offer> CreateOfferAsync(string restaurantId, CreateOfferViewModel model)
        {
            Offer offer = _mapper.Map<Offer>(model);
            var products = new List<Product>()
            {
                await _productService.GetByIdAsync(model.FirstProductId),
                await _productService.GetByIdAsync(model.SecondProductId),
                await _productService.GetByIdAsync(model.ThirdhProductId),
            };
            offer.OfferProducts = await CreateOfferProducts(products, offer);

            await _context.Offers.AddAsync(offer);
            await _restaurantService.AddOfferAsync(restaurantId, offer);

            await _context.SaveChangesAsync();

            return offer;
        }

        public async Task<List<OfferProduct>> CreateOfferProducts(ICollection<Product> products, Offer offer)
        {
            var offerProducts = new List<OfferProduct>();

            foreach (var item in products)
            {
                if (item != null)
                {
                    OfferProduct offerProduct = new OfferProduct();
                    offerProduct.Product = item;
                    offerProduct.Offer = offer;

                    await _context.OfferProducts.AddAsync(offerProduct);

                    offerProducts.Add(offerProduct);
                }
            }

            return offerProducts;
        }

        public async Task<Offer> EditOfferAsync(string id, CreateOfferViewModel model)
        {
            Offer offer = await GetByIdAsync(id);
            offer.Name = model.Name;
            offer.Description = model.Description;
            offer.Price = model.Price;
            var products = new List<Product>()
            {
                await _productService.GetByIdAsync(model.FirstProductId),
                await _productService.GetByIdAsync(model.SecondProductId),
                await _productService.GetByIdAsync(model.ThirdhProductId),
            };
            offer.OfferProducts = await CreateOfferProducts(products, offer);

            await _context.SaveChangesAsync();

            return offer;
        }

        public async Task<Offer> DeleteOfferAsync(string id)
        {
            Offer offer = await GetByIdAsync(id);
            offer.IsDeleted = true;

            await _context.SaveChangesAsync();

            return offer;
        }

        public async Task SaveImageURL(string imageURL, Offer offer)
        {
            offer.ImageURL = imageURL;
            await _context.SaveChangesAsync();
        }
    }
}
