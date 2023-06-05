using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GustoExpress.Web.Data.Migrations
{
    public partial class addedRestaurantToProduct : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Products_Restaurants_RestaurantId",
                table: "Products");

            migrationBuilder.DeleteData(
                table: "Restaurants",
                keyColumn: "Id",
                keyValue: new Guid("2821f72a-ce41-4b15-8106-f2770cfb0bcf"));

            migrationBuilder.DeleteData(
                table: "Restaurants",
                keyColumn: "Id",
                keyValue: new Guid("d5925bb4-381a-4115-b7e8-d7462fb678c3"));

            migrationBuilder.AlterColumn<Guid>(
                name: "RestaurantId",
                table: "Products",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Products",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(15)",
                oldMaxLength: 15);

            migrationBuilder.InsertData(
                table: "Restaurants",
                columns: new[] { "Id", "CityId", "DeliveryPrice", "Description", "ImageURL", "Name", "TimeToDeliver" },
                values: new object[] { new Guid("1538ec84-fb26-4287-935c-8371df8861f8"), new Guid("9c7ab005-24e6-4b2e-a54d-70cf4d9658d1"), 4.00m, "Nunc magna arcu, pharetra ut mi vitae, hendrerit laoreet lacus.", "~\\images\\Restaurants\\download.png", "McDonald's", "15-20" });

            migrationBuilder.InsertData(
                table: "Restaurants",
                columns: new[] { "Id", "CityId", "DeliveryPrice", "Description", "ImageURL", "Name", "TimeToDeliver" },
                values: new object[] { new Guid("97352fb3-fff1-4137-8ba3-24eacd9b274d"), new Guid("9c7ab005-24e6-4b2e-a54d-70cf4d9658d1"), 5.00m, "Lorem ipsum dolor sit amet, consectetur adipiscing elit.", "~\\images\\Restaurants\\og_image.jpg", "Aladin Foods", "20-30" });

            migrationBuilder.AddForeignKey(
                name: "FK_Products_Restaurants_RestaurantId",
                table: "Products",
                column: "RestaurantId",
                principalTable: "Restaurants",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Products_Restaurants_RestaurantId",
                table: "Products");

            migrationBuilder.DeleteData(
                table: "Restaurants",
                keyColumn: "Id",
                keyValue: new Guid("1538ec84-fb26-4287-935c-8371df8861f8"));

            migrationBuilder.DeleteData(
                table: "Restaurants",
                keyColumn: "Id",
                keyValue: new Guid("97352fb3-fff1-4137-8ba3-24eacd9b274d"));

            migrationBuilder.AlterColumn<Guid>(
                name: "RestaurantId",
                table: "Products",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Products",
                type: "nvarchar(15)",
                maxLength: 15,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(20)",
                oldMaxLength: 20);

            migrationBuilder.InsertData(
                table: "Restaurants",
                columns: new[] { "Id", "CityId", "DeliveryPrice", "Description", "ImageURL", "Name", "TimeToDeliver" },
                values: new object[] { new Guid("2821f72a-ce41-4b15-8106-f2770cfb0bcf"), new Guid("9c7ab005-24e6-4b2e-a54d-70cf4d9658d1"), 4.00m, "Nunc magna arcu, pharetra ut mi vitae, hendrerit laoreet lacus.", "~\\images\\Restaurants\\download.png", "McDonald's", "15-20" });

            migrationBuilder.InsertData(
                table: "Restaurants",
                columns: new[] { "Id", "CityId", "DeliveryPrice", "Description", "ImageURL", "Name", "TimeToDeliver" },
                values: new object[] { new Guid("d5925bb4-381a-4115-b7e8-d7462fb678c3"), new Guid("9c7ab005-24e6-4b2e-a54d-70cf4d9658d1"), 5.00m, "Lorem ipsum dolor sit amet, consectetur adipiscing elit.", "~\\images\\Restaurants\\og_image.jpg", "Aladin Foods", "20-30" });

            migrationBuilder.AddForeignKey(
                name: "FK_Products_Restaurants_RestaurantId",
                table: "Products",
                column: "RestaurantId",
                principalTable: "Restaurants",
                principalColumn: "Id");
        }
    }
}
