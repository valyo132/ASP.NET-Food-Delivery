using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GustoExpress.Web.Data.Migrations
{
    public partial class RestaurantSeeder : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "Restaurants",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(60)",
                oldMaxLength: 60);

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "Products",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(60)",
                oldMaxLength: 60);

            migrationBuilder.InsertData(
                table: "Cities",
                columns: new[] { "Id", "CityName" },
                values: new object[] { new Guid("9c7ab005-24e6-4b2e-a54d-70cf4d9658d1"), "Sofia" });

            migrationBuilder.InsertData(
                table: "Restaurants",
                columns: new[] { "Id", "CityId", "DeliveryPrice", "Description", "ImageURL", "Name", "TimeToDeliver" },
                values: new object[] { new Guid("2821f72a-ce41-4b15-8106-f2770cfb0bcf"), new Guid("9c7ab005-24e6-4b2e-a54d-70cf4d9658d1"), 4.00m, "Nunc magna arcu, pharetra ut mi vitae, hendrerit laoreet lacus.", "~\\images\\Restaurants\\download.png", "McDonald's", "15-20" });

            migrationBuilder.InsertData(
                table: "Restaurants",
                columns: new[] { "Id", "CityId", "DeliveryPrice", "Description", "ImageURL", "Name", "TimeToDeliver" },
                values: new object[] { new Guid("d5925bb4-381a-4115-b7e8-d7462fb678c3"), new Guid("9c7ab005-24e6-4b2e-a54d-70cf4d9658d1"), 5.00m, "Lorem ipsum dolor sit amet, consectetur adipiscing elit.", "~\\images\\Restaurants\\og_image.jpg", "Aladin Foods", "20-30" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Restaurants",
                keyColumn: "Id",
                keyValue: new Guid("2821f72a-ce41-4b15-8106-f2770cfb0bcf"));

            migrationBuilder.DeleteData(
                table: "Restaurants",
                keyColumn: "Id",
                keyValue: new Guid("d5925bb4-381a-4115-b7e8-d7462fb678c3"));

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "Restaurants",
                type: "nvarchar(60)",
                maxLength: 60,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldMaxLength: 100);

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "Products",
                type: "nvarchar(60)",
                maxLength: 60,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldMaxLength: 100);
        }
    }
}
