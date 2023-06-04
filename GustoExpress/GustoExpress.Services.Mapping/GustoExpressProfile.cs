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

            CreateMap<CreateRestaurantViewModel, Restaurant>()
                .ForMember(x => x.TimeToDeliver, y => y.MapFrom(s => $"{s.MinTimeToDeliver}-{s.MaxTimeToDeliver}"))
                .ForMember(x => x.City, y => y.MapFrom(s => "City"));
        }
    }
}
