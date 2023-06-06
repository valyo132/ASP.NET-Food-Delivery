using GustoExpress.Data.Models;

namespace GustoExpress.Data.Seeding
{
    internal class RestaurantSeeder
    {
        internal Restaurant[] GenerateRestaurants()
        {
            ICollection<Restaurant> restaurants = new HashSet<Restaurant>();
            Restaurant currentRestaurant;

            currentRestaurant = new Restaurant()
            {
                Name = "Aladin Foods",
                Description = "Lorem ipsum dolor sit amet, consectetur adipiscing elit.",
                CityId = Guid.Parse("9c7ab005-24e6-4b2e-a54d-70cf4d9658d1"),
                DeliveryPrice = 5.00m,
                TimeToDeliver = "20-30",
                ImageURL = @"\images\Restaurants\og_image.jpg"
            };
            restaurants.Add(currentRestaurant);

            currentRestaurant = new Restaurant()
            {
                Name = "McDonald's",
                Description = "Nunc magna arcu, pharetra ut mi vitae, hendrerit laoreet lacus.",
                CityId = Guid.Parse("9c7ab005-24e6-4b2e-a54d-70cf4d9658d1"),
                DeliveryPrice = 4.00m,
                TimeToDeliver = "15-20",
                ImageURL = @"\images\Restaurants\download.png"
            };
            restaurants.Add(currentRestaurant);

            return restaurants.ToArray();
        }
    }
}
