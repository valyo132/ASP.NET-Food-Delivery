using AutoMapper;
using AutoMapper.QueryableExtensions;
using GustoExpress.Data.Models;
using GustoExpress.Services.Data.Contracts;
using GustoExpress.Web.Data;
using GustoExpress.Web.ViewModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Migrations.Operations;

namespace GustoExpress.Services.Data
{
    public class RestaurantService : IRestaurantService
    {
        private readonly IMapper _mapper;
        private readonly ApplicationDbContext _context;
        private readonly ICityService _cityService;

        public RestaurantService(ApplicationDbContext context,
            IMapper mapper,
            ICityService cityService)
        {
            _context = context;
            _mapper = mapper;
            _cityService = cityService;
        }

        public async Task<Restaurant> GetByIdAsync(string id)
        {
            return await _context.Restaurants.FirstOrDefaultAsync(r => r.Id.ToString() == id);
        }

        public T ProjectTo<T>(Restaurant restaurant)
        {
            return _mapper.Map<T>(restaurant);
        }

        public async Task<List<AllRestaurantViewModel>> All(string city)
        {
            return await _context.Restaurants
                .Where(r => r.City.CityName == city)
                .ProjectTo<AllRestaurantViewModel>(_mapper.ConfigurationProvider)
                .ToListAsync();
        }

        public async Task<Restaurant> Create(CreateRestaurantViewModel model)
        {
            Restaurant newRestaurant = new Restaurant();
            newRestaurant.Name = model.Name;
            newRestaurant.Description = model.Description;
            newRestaurant.DeliveryPrice = model.DeliveryPrice;
            newRestaurant.TimeToDeliver = $"{model.MinTimeToDeliver}-{model.MaxTimeToDeliver}";
            newRestaurant.ImageURL = model.ImageURL;

            var city = await _cityService.GetCityAsync(model.City);

            if (city == null)
            {
                city = await _cityService.CreateCity(model.City);
            }
            newRestaurant.City = city;

            await _context.Restaurants.AddAsync(newRestaurant);
            await _context.SaveChangesAsync();

            return newRestaurant;
        }

        public async Task SaveImageURL(string url, Restaurant restaurant)
        {
            restaurant.ImageURL = url;
            await _context.SaveChangesAsync();
        }
    }
}
