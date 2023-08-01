using AutoMapper;
using GustoExpress.Data.Models;
using GustoExpress.Web.ViewModels;

namespace GustoExpress.Services.Mapping
{
    public class GustoExpressProfile : Profile
    {
        public GustoExpressProfile()
        {
            CreateMap<Restaurant, AllRestaurantViewModel>();

            CreateMap<Restaurant, RestaurantViewModel>();

            CreateMap<Restaurant, CreateRestaurantViewModel>()
                .ForMember(x => x.MinTimeToDeliver, y => y.MapFrom(s => int.Parse(new string(s.TimeToDeliver.TakeWhile(c => c != '-').ToArray()))))
                .ForMember(x => x.MaxTimeToDeliver, y => y.MapFrom(s => int.Parse(new string(s.TimeToDeliver.Skip(s.TimeToDeliver.IndexOf('-') + 1).ToArray()))))
                .ForMember(x => x.City, y => y.MapFrom(s => s.City.CityName));

            CreateMap<Restaurant, RestaurantPageViewModel>()
                .ForMember(x => x.City, y => y.MapFrom(s => s.City));

            CreateMap<CreateRestaurantViewModel, Restaurant>()
                .ForMember(x => x.TimeToDeliver, y => y.MapFrom(s => $"{s.MinTimeToDeliver}-{s.MaxTimeToDeliver}"))
                .ForMember(x => x.City, y => y.MapFrom(s => "City"));

            CreateMap<CreateProductViewModel, Product>();

            CreateMap<Product, CreateProductViewModel>();

            CreateMap<Product, ProductViewModel>();

            CreateMap<CreateOfferViewModel, Offer>();

            CreateMap<Offer, OfferViewModel>();

            CreateMap<Offer, CreateOfferViewModel>()
                .ForMember(x => x.FirstProductId, y => y.MapFrom(s => s.OfferProducts.FirstOrDefault().ProductId))
                .ForMember(x => x.SecondProductId, y => y.MapFrom(s => s.OfferProducts.Skip(1).FirstOrDefault().ProductId))
                .ForMember(x => x.ThirdhProductId, y => y.MapFrom(s => s.OfferProducts.Skip(2).FirstOrDefault().ProductId));

            CreateMap<CreateReviewViewModel, Review>();

            CreateMap<Review, ReviewViewModel>();

            CreateMap<OrderItem, CreateOrderItemViewModel>();

            CreateMap<OrderItem, OrderItemViewModel>();

            CreateMap<CreateOrderItemViewModel, OrderItem>();

            CreateMap<Order, OrderViewModel>();

            CreateMap<ApplicationUser, UserViewModel>();
        }
    }
}
