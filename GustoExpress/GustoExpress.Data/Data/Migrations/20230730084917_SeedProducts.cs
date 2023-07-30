using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GustoExpress.Web.Data.Migrations
{
    public partial class SeedProducts : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "1cd0be45-b9be-4fc9-9b0f-46a1d20a77be",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "cbba6db5-7180-405b-a3ba-9f1c10f500a1", "AQAAAAEAACcQAAAAEJBzMkmkvusKIhM13nevotYSRGkvGMC2aTplC/TPiQ2YqDy1Z0xCMdCMnHYVUsMOvw==", "00accc74-758b-4bfe-b53e-90a28992f2c4" });

            migrationBuilder.InsertData(
                table: "Products",
                columns: new[] { "Id", "Category", "Description", "Discount", "Grams", "ImageURL", "IsDeleted", "Name", "Price", "RestaurantId" },
                values: new object[] { new Guid("34e46ce4-b7d1-4642-9b8a-15d1666f0dbe"), 7, "Simple cheeseburger", null, 150m, "/images/Products/2af60ede-d017-4c1a-9dda-27bc4fd9fb6b.jpg", false, "Cheeseburger", 4.00m, new Guid("c5f0aaf2-99f2-43de-8e48-9a8fb97ccc44") });

            migrationBuilder.InsertData(
                table: "Products",
                columns: new[] { "Id", "Category", "Description", "Discount", "Grams", "ImageURL", "IsDeleted", "Name", "Price", "RestaurantId" },
                values: new object[] { new Guid("b55643d9-5372-442b-b17d-c463bb1f4eaf"), 6, "Very taste bubble tea", null, 300m, "/images/Products/0d3fa9fe-d37c-4769-9f8b-bac4424ee3b1.png", false, "Bubble tea", 3.00m, new Guid("c5f0aaf2-99f2-43de-8e48-9a8fb97ccc44") });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "1cd0be45-b9be-4fc9-9b0f-46a1d20a77be",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "15086804-17c3-4ee6-b842-59fe07b2cfc1", "AQAAAAEAACcQAAAAEA77BIjwrVc2GdwszatREPI672ukqI5EcageMxcTwY/SXMw0OfuWil/im97OhmLsew==", "0c623910-d79d-44f8-8542-ecfaea1a7b1a" });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: new Guid("34e46ce4-b7d1-4642-9b8a-15d1666f0dbe"),
                column: "ImageURL",
                value: "/images/Products/0d3fa9fe-d37c-4769-9f8b-bac4424ee3b1.png");

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: new Guid("b55643d9-5372-442b-b17d-c463bb1f4eaf"),
                column: "ImageURL",
                value: "/images/Products/2af60ede-d017-4c1a-9dda-27bc4fd9fb6b.jpg");
        }
    }
}
