using AutoMapper;
using AutoMapper.QueryableExtensions;
using GustoExpress.Data.Models;
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
        private readonly ICityService _cityService;

        public RestaurantService(ApplicationDbContext context,
            IMapper mapper,
            ICityService cityService)
        {
            _context = context;
            _mapper = mapper;
            _cityService = cityService;
        }

        public async Task<T> ProjectToModel<T>(string id)
        {
            Restaurant restaurant = await GetByIdAsync(id);
            T restaurantViewModel = ProjectTo<T>(restaurant);
            return restaurantViewModel;
        }

        public async Task<Restaurant> GetByIdAsync(string id)
        {
            return await _context.Restaurants
                .Include(r => r.City)
                .Include(r => r.Products)
                .Include(r => r.Offers)
                .Include(r => r.Reviews)
                    .ThenInclude(r => r.User)
                .FirstOrDefaultAsync(r => r.Id.ToString() == id);
        }

        public async Task<List<AllRestaurantViewModel>> AllAsync(string city)
        {
            return await _context.Restaurants
                .Where(r => r.City.CityName == city && r.IsDeleted == false)
                .ProjectTo<AllRestaurantViewModel>(_mapper.ConfigurationProvider)
                .ToListAsync();
        }

        public async Task<RestaurantViewModel> CreateAsync(CreateRestaurantViewModel model)
        {
            Restaurant newRestaurant = new Restaurant();
            newRestaurant.Name = model.Name;
            newRestaurant.Description = model.Description;
            newRestaurant.DeliveryPrice = model.DeliveryPrice;
            newRestaurant.TimeToDeliver = $"{model.MinTimeToDeliver}-{model.MaxTimeToDeliver}";
            newRestaurant.ImageURL = model.ImageURL;

            City city = await _cityService.GetCityAsync(model.City);

            if (city == null)
            {
                city = await _cityService.CreateCityAsync(model.City);
            }
            newRestaurant.City = city;

            await _context.Restaurants.AddAsync(newRestaurant);
            await _context.SaveChangesAsync();

            return ProjectTo<RestaurantViewModel>(newRestaurant);
        }

        public async Task<RestaurantViewModel> EditRestaurantAsync(string id, CreateRestaurantViewModel model)
        {
            Restaurant restaurant = await GetByIdAsync(id);
            restaurant.Name = model.Name;
            restaurant.Description = model.Description;
            restaurant.DeliveryPrice = model.DeliveryPrice;
            restaurant.TimeToDeliver = $"{model.MinTimeToDeliver}-{model.MaxTimeToDeliver}";

            City city = await _cityService.GetCityAsync(model.City);

            if (city == null)
            {
                city = await _cityService.CreateCityAsync(model.City);
            }
            restaurant.City = city;

            await _context.SaveChangesAsync();

            return ProjectTo<RestaurantViewModel>(restaurant);
        }

        public async Task<RestaurantViewModel> DeleteAsync(string id)
        {
            Restaurant restaurant = await GetByIdAsync(id);
            restaurant.IsDeleted = true;
            await _context.SaveChangesAsync();

            return ProjectTo<RestaurantViewModel>(restaurant);
        }

        public async Task AddProductAsync(Product product)
        {
            var restaurant = await GetByIdAsync(product.RestaurantId.ToString());
            restaurant.Products.Add(product);

            await _context.SaveChangesAsync();
        }

        public async Task AddOfferAsync(Offer offer)
        {
            var restaurant = await GetByIdAsync(offer.RestaurantId.ToString());
            restaurant.Offers.Add(offer);

            await _context.SaveChangesAsync();
        }

        public async Task SaveImageURL(string url, RestaurantViewModel restaurantVm)
        {
            Restaurant restraurant = await GetByIdAsync(restaurantVm.Id.ToString());
            restraurant.ImageURL = url;
            await _context.SaveChangesAsync();
        }

        private T ProjectTo<T>(Restaurant restaurant)
        {
            return _mapper.Map<T>(restaurant);
        }
    }
}
