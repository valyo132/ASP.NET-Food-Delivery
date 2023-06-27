﻿using AutoMapper;
using GustoExpress.Data.Models;
using GustoExpress.Services.Data.Contracts;
using GustoExpress.Web.Data;
using GustoExpress.Web.ViewModels;
using Microsoft.EntityFrameworkCore;
using System;

namespace GustoExpress.Services.Data
{
    public class OrderItemService : IOrderItemService
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

        public string GetRestaurantId(OrderItem item)
        {
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
            }
            else
            {
                var offer = @object as Offer;
                orderItemViewModel.OfferId = offer.Id.ToString();
                orderItemViewModel.Offer = offer;
            }

            return orderItemViewModel;
        }

        public async Task<OrderItemViewModel> CreateOrderItem(CreateOrderItemViewModel model)
        {
            OrderItem orderItem = _mapper.Map<OrderItem>(model);

            await _context.OrderItems.AddAsync(orderItem);
            await _context.SaveChangesAsync();

            return ProjectTo<OrderItemViewModel>(orderItem);
        }

        private T ProjectTo<T>(OrderItem item)
        {
            return _mapper.Map<T>(item);
        }
    }
}
