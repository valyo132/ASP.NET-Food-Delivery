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
        }
    }
}
