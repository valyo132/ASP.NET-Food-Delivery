using AutoMapper;
using GustoExpress.Data.Models;
using GustoExpress.Services.Data.Contracts;
using GustoExpress.Web.Data;
using GustoExpress.Web.ViewModels;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Security.Policy;

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
                .Include(o => o.Products)
                .FirstOrDefaultAsync(o => o.Id.ToString() == id);
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

        public async Task<Offer> CreateOffer(string restaurantId, CreateOfferViewModel model)
        {
            Offer offer = _mapper.Map<Offer>(model);
            var products = new List<Product>()
            {
                await _productService.GetById(model.FirstProductId),
                await _productService.GetById(model.SecondProductId),
                await _productService.GetById(model.ThirdhProductId),
            };
            offer.Products = products.Where(p => p != null).ToList();

            await _context.Offers.AddAsync(offer);
            await _restaurantService.AddOfferAsync(restaurantId, offer);

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
