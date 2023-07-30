namespace GustoExpress.Web.Data
{
    using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore;

    using GustoExpress.Data.Data.Configuration;
    using GustoExpress.Data.Models;

    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<ApplicationUser> ApplicationUsers { get; set; } = null!;
        public DbSet<City> Cities { get; set; } = null!;
        public DbSet<Offer> Offers { get; set; } = null!;
        public DbSet<Order> Orders { get; set; } = null!;
        public DbSet<OrderItem> OrderItems { get; set; } = null!;
        public DbSet<Product> Products { get; set; } = null!;
        public DbSet<Restaurant> Restaurants { get; set; } = null!;
        public DbSet<Review> Reviews { get; set; } = null!;
        public DbSet<OfferProduct> OfferProducts { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.ApplyConfiguration(new RestaurantEntityConfiguration());

            builder.ApplyConfiguration(new UserEntityConfiguration());

            builder.ApplyConfiguration(new ProductEntityConfiguration());

            builder.Entity<Offer>()
                .HasOne(o => o.Restaurant)
                .WithMany(r => r.Offers)
                .HasForeignKey(o => o.RestaurantId)
                .OnDelete(DeleteBehavior.Restrict);

            base.OnModelCreating(builder);
        }
    }
}