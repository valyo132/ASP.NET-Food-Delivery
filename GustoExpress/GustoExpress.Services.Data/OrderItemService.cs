
namespace GustoExpress.Services.Data
{
    using Microsoft.EntityFrameworkCore;
    using AutoMapper;

    using GustoExpress.Data.Models;
    using GustoExpress.Services.Data.Contracts;
    using GustoExpress.Web.Data;
    using GustoExpress.Web.ViewModels;
    using GustoExpress.Services.Data.Helpers;

    public class OrderItemService : IOrderItemService, IProjectable<OrderItem>
    {
        private readonly ApplicationDbContext _context;
        private readonly IProductService _productService;
        private readonly IOfferService _offerService;
        private readonly IMapper _mapper;

        public OrderItemService(ApplicationDbContext context,
            IProductService productService,
            IOfferService offerService,
            IMapper mapper)
        {
            _context = context;
            _productService = productService;
            _offerService = offerService;
            _mapper = mapper;
        }

        public async Task<OrderItem> GetOrderItemByIdAsync(string itemId)
        {
            return await _context.OrderItems
                .Include(oi => oi.Product)
                .Include(oi => oi.Offer)
                .FirstOrDefaultAsync(oi => oi.Id.ToString() == itemId);
        }

        public async Task<string> GetRestaurantIdAsync(string id)
        {
            OrderItem item = await GetOrderItemByIdAsync(id);
            string restaurantId = null;

            if (item.Product == null)
                restaurantId = item.Offer.RestaurantId.ToString();
            else
                restaurantId = item.Product.RestaurantId.ToString();

            return restaurantId;
        }

        public async Task<object> GetObjectAsync(string objId)
        {
            object @object = await _productService.GetByIdAsync(objId);

            if (@object == null)
                @object = await _offerService.GetByIdAsync(objId);

            if (@object is Product product)
                return product;
            else if (@object is Offer offer)
                return offer;
            else
                return null;
        }

        public CreateOrderItemViewModel GetOrderItemViewModel(object obj)
        {
            CreateOrderItemViewModel orderItemViewModel = new CreateOrderItemViewModel();

            var @object = obj;

            if (@object is Product)
            {
                var product = @object as Product;
                orderItemViewModel.ProductId = product.Id.ToString();
                orderItemViewModel.Product = product;
                orderItemViewModel.RestaurantId = product.RestaurantId.ToString();
            }
            else
            {
                var offer = @object as Offer;
                orderItemViewModel.OfferId = offer.Id.ToString();
                orderItemViewModel.Offer = offer;
                orderItemViewModel.RestaurantId = offer.RestaurantId.ToString();
            }

            return orderItemViewModel;
        }

        public async Task<OrderItemViewModel> CreateOrderItemAsync(CreateOrderItemViewModel model)
        {
            OrderItem orderItem = _mapper.Map<OrderItem>(model);

            await _context.OrderItems.AddAsync(orderItem);
            await _context.SaveChangesAsync();

            return ProjectTo<OrderItemViewModel>(orderItem);
        }

        public T ProjectTo<T>(OrderItem obj)
        {
            return _mapper.Map<T>(obj);
        }
    }
}
