using GustoExpress.Data.Models;
using GustoExpress.Data.Seeding;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GustoExpress.Data.Data.Configuration
{
    public class RestaurantEntityConfiguration : IEntityTypeConfiguration<Restaurant>
    {
        private readonly RestaurantSeeder _restaurantSeeder;

        public RestaurantEntityConfiguration()
        {
            _restaurantSeeder = new RestaurantSeeder();
        }

        public void Configure(EntityTypeBuilder<Restaurant> builder)
        {
            builder.HasData(this._restaurantSeeder.GenerateRestaurants());
        }
    }
}
