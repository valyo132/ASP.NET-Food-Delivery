﻿using AutoMapper;
using GustoExpress.Data.Models;
using GustoExpress.Web.ViewModels;

namespace GustoExpress.Services.Mapping
{
    public class GustoExpressProfile : Profile
    {
        public GustoExpressProfile()
        {
            CreateMap<Restaurant, AllRestaurantViewModel>();

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

            CreateMap<CreateOfferViewModel, Offer>();
        }
    }
}
