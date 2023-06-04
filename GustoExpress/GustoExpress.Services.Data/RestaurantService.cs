using AutoMapper;
using AutoMapper.QueryableExtensions;
using GustoExpress.Services.Data.Contracts;
using GustoExpress.Web.Data;
using GustoExpress.Web.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace GustoExpress.Services.Data
{
    public class RestaurantService : IRestaurantService
    {
        private readonly IMapper _mapper;
        private readonly ApplicationDbContext _context;

        public RestaurantService(ApplicationDbContext context,
            IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<List<AllRestaurantViewModel>> All(string city)
        {
            return await _context.Restaurants
                .Where(r => r.City.CityName == city)
                .ProjectTo<AllRestaurantViewModel>(_mapper.ConfigurationProvider)
                .ToListAsync();
        }
    }
}
