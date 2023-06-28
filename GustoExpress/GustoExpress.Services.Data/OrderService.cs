namespace GustoExpress.Services.Data
{
    using AutoMapper;
    using GustoExpress.Data.Models;
    using GustoExpress.Services.Data.Contracts;
    using GustoExpress.Web.Data;
    using GustoExpress.Web.ViewModels;
    using Microsoft.EntityFrameworkCore; 

    public class OrderService : IOrderService
    {
        private readonly ApplicationDbContext _context;
        private readonly IOrderItemService _orderItemService;
        private readonly IMapper _mapper;

        public OrderService(ApplicationDbContext context,
            IOrderItemService orderItemService,
            IMapper mapper)
        {
            _context = context;
            _orderItemService = orderItemService;
            _mapper = mapper;
        }

        public async Task<OrderViewModel> GetOrderToComplete(string userId, string restaurantId)
        {
            Order order = await GetOrder(userId, restaurantId);

            if (order == null)
            {
                throw new InvalidOperationException("You don't have any item in your order yet!");
            }

            order.TotalCost = GetOrderTotalCost(order);

            await _context.SaveChangesAsync();

            return ProjectTo<OrderViewModel>(order);
        }

        public async Task CompleteOrder(string userId, string restaurantId)
        {
            Order order = await GetOrder(userId, restaurantId);
            order.IsCompleted = true;

            await _context.SaveChangesAsync();
        }

        public async Task<Order> GetOrder(string userId, string restaurantId)
        {
            return await _context.Orders
                .Include(o => o.OrderItems)
                .FirstOrDefaultAsync(o => o.UserId == userId && o.IsCompleted == false && o.RestaurantId.ToString() == restaurantId);
        }

        public async Task<OrderViewModel> AddItemToOrder(string userId, string itemId)
        {
            OrderItem item = await _orderItemService.GetOrderItemByIdAsync(itemId);
            string restaurantId = _orderItemService.GetRestaurantId(item);

            Order userOrder = await GetOrder(userId, restaurantId);

            if (userOrder == null)
            {
                userOrder = await CreateOrderAsync(userId, restaurantId);
            }

            if (item != null)
            {
                if (userOrder.OrderItems.Any(oi => oi.ProductId == item.ProductId && item.ProductId != null) ||
                    userOrder.OrderItems.Any(oi => oi.OfferId == item.OfferId && item.OfferId != null))
                {
                    throw new InvalidOperationException("This item was aleady added to your order!");
                }

                userOrder.OrderItems.Add(item);

                await _context.SaveChangesAsync();
            }

            return ProjectTo<OrderViewModel>(userOrder);
        }

        private async Task<Order> CreateOrderAsync(string userId, string restaurantId)
        {
            Order order = new Order();
            order.UserId = userId;
            order.TotalCost = 0;
            order.RestaurantId = Guid.Parse(restaurantId);

            await _context.Orders.AddAsync(order);
            await _context.SaveChangesAsync();

            return order;
        }

        private decimal GetOrderTotalCost(Order order)
        {
            return order.OrderItems.Sum(oi => oi.TotalCost);
        }

        private T ProjectTo<T>(Order item)
        {
            return _mapper.Map<T>(item);
        }
    }
}
