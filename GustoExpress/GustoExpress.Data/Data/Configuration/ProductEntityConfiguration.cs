namespace GustoExpress.Data.Data.Configuration
{
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    using GustoExpress.Data.Models;
    using GustoExpress.Data.Seeding;

    public class ProductEntityConfiguration : IEntityTypeConfiguration<Product>
    {
        private readonly ProductSeeder _productSeeder;

        public ProductEntityConfiguration()
        {
            _productSeeder = new ProductSeeder();
        }

        public void Configure(EntityTypeBuilder<Product> builder)
        {
            builder.HasData(this._productSeeder.GenerateProducts());
        }
    }
}
