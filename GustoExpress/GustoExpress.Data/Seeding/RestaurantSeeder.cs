namespace GustoExpress.Data.Seeding
{
    using GustoExpress.Data.Models;

    internal class RestaurantSeeder
    {
        internal Restaurant[] GenerateRestaurants()
        {
            ICollection<Restaurant> restaurants = new HashSet<Restaurant>();
            Restaurant currentRestaurant;

            currentRestaurant = new Restaurant()
            {
                Id = Guid.Parse("419ede12-6073-42fb-ac30-217430d61382"),
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
                Id = Guid.Parse("c5f0aaf2-99f2-43de-8e48-9a8fb97ccc44"),
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
