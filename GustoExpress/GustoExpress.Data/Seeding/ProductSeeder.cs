namespace GustoExpress.Data.Seeding
{
    using GustoExpress.Data.Models;

    internal class ProductSeeder
    {
        internal Product[] GenerateProducts()
        {
            ICollection<Product> products = new HashSet<Product>();
            Product currentProduct;

            currentProduct = new Product()
            {
                Id = Guid.Parse("34e46ce4-b7d1-4642-9b8a-15d1666f0dbe"),
                Name = "Cheeseburger",
                Category = Models.Enums.Category.FastFood,
                Description = "Simple cheeseburger",
                Price = 4.00m,
                Discount = null,
                Grams = 150m,
                RestaurantId = Guid.Parse("c5f0aaf2-99f2-43de-8e48-9a8fb97ccc44"),
                IsDeleted = false,
                ImageURL = "/images/Products/2af60ede-d017-4c1a-9dda-27bc4fd9fb6b.jpg"
            };
            products.Add(currentProduct);

            currentProduct = new Product()
            {
                Id = Guid.Parse("b55643d9-5372-442b-b17d-c463bb1f4eaf"),
                Name = "Bubble tea",
                Category = Models.Enums.Category.Drink,
                Description = "Very taste bubble tea",
                Price = 3.00m,
                Discount = null,
                Grams = 300m,
                RestaurantId = Guid.Parse("c5f0aaf2-99f2-43de-8e48-9a8fb97ccc44"),
                IsDeleted = false,
                ImageURL = "/images/Products/0d3fa9fe-d37c-4769-9f8b-bac4424ee3b1.png"
            };
            products.Add(currentProduct);

            return products.ToArray();
        }
    }
}
