
namespace GustoExpress.Services.Data
{
    using Microsoft.AspNetCore.Mvc.Rendering;
    using Microsoft.EntityFrameworkCore;

    using AutoMapper;
    using GustoExpress.Data.Models;
    using GustoExpress.Services.Data.Contracts;
    using GustoExpress.Web.Data;
    using GustoExpress.Web.ViewModels;
    using GustoExpress.Services.Data.Helpers.Contracts;

    public class OfferService : IOfferService, IProjectable<Offer>
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

        public async Task<OfferViewModel> CreateOfferAsync(string restaurantId, CreateOfferViewModel model)
        {
            Offer offer = _mapper.Map<Offer>(model);
            var products = new List<Product>()
            {
                await _productService.GetByIdAsync(model.FirstProductId),
                await _productService.GetByIdAsync(model.SecondProductId),
                await _productService.GetByIdAsync(model.ThirdhProductId),
            };
            offer.OfferProducts = await CreateOfferProducts(products, offer);

            if (await CheckIfProductExists(offer.Name, offer.RestaurantId.ToString()))
            {
                throw new InvalidOperationException("An item with this name already exists!");
            }

            await _context.Offers.AddAsync(offer);

            await _context.SaveChangesAsync();

            return ProjectTo<OfferViewModel>(offer);
        }

        public async Task<OfferViewModel> EditOfferAsync(string id, CreateOfferViewModel model)
        {
            Offer offer = await GetByIdAsync(id);
            offer.Name = model.Name;
            offer.Description = model.Description;
            offer.Price = model.Price;
            offer.Discount = model.Discount;
            var products = new List<Product>()
            {
                await _productService.GetByIdAsync(model.FirstProductId),
                await _productService.GetByIdAsync(model.SecondProductId),
                await _productService.GetByIdAsync(model.ThirdhProductId),
            };
            offer.OfferProducts = await CreateOfferProducts(products, offer);

            await _context.SaveChangesAsync();

            return ProjectTo<OfferViewModel>(offer);
        }

        public async Task<OfferViewModel> DeleteOfferAsync(string id)
        {
            Offer offer = await GetByIdAsync(id);
            offer.IsDeleted = true;

            await _context.SaveChangesAsync();

            return ProjectTo<OfferViewModel>(offer);
        }

        public async Task SaveImageURL(string imageURL, OfferViewModel offerVm)
        {
            Offer offer = await GetByIdAsync(offerVm.Id.ToString());
            offer.ImageURL = imageURL;
            await _context.SaveChangesAsync();
        }

        private async Task<List<OfferProduct>> CreateOfferProducts(ICollection<Product> products, Offer offer)
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

        private async Task<bool> CheckIfProductExists(string name, string resntaurantId)
        {
            return await _context.Offers
                .AnyAsync(o => o.Name.ToLower() == name.ToLower() && o.IsDeleted == false && o.RestaurantId.ToString() == resntaurantId);
        }

        public async Task<T> ProjectToModel<T>(string id)
        {
            Offer offer = await GetByIdAsync(id);
            T model = ProjectTo<T>(offer);

            return model;
        }

        public T ProjectTo<T>(Offer obj)
        {
            return _mapper.Map<T>(obj);
        }
    }
}
