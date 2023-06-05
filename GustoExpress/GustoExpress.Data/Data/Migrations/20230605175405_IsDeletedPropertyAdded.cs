using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GustoExpress.Web.Data.Migrations
{
    public partial class IsDeletedPropertyAdded : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Restaurants",
                keyColumn: "Id",
                keyValue: new Guid("1538ec84-fb26-4287-935c-8371df8861f8"));

            migrationBuilder.DeleteData(
                table: "Restaurants",
                keyColumn: "Id",
                keyValue: new Guid("97352fb3-fff1-4137-8ba3-24eacd9b274d"));

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "Restaurants",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "Products",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.InsertData(
                table: "Restaurants",
                columns: new[] { "Id", "CityId", "DeliveryPrice", "Description", "ImageURL", "IsDeleted", "Name", "TimeToDeliver" },
                values: new object[] { new Guid("51c21cb7-4a9b-4593-a115-1b35fc631181"), new Guid("9c7ab005-24e6-4b2e-a54d-70cf4d9658d1"), 4.00m, "Nunc magna arcu, pharetra ut mi vitae, hendrerit laoreet lacus.", "~\\images\\Restaurants\\download.png", false, "McDonald's", "15-20" });

            migrationBuilder.InsertData(
                table: "Restaurants",
                columns: new[] { "Id", "CityId", "DeliveryPrice", "Description", "ImageURL", "IsDeleted", "Name", "TimeToDeliver" },
                values: new object[] { new Guid("e6665130-760d-48cd-bbe9-c7ccc8dc212a"), new Guid("9c7ab005-24e6-4b2e-a54d-70cf4d9658d1"), 5.00m, "Lorem ipsum dolor sit amet, consectetur adipiscing elit.", "~\\images\\Restaurants\\og_image.jpg", false, "Aladin Foods", "20-30" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Restaurants",
                keyColumn: "Id",
                keyValue: new Guid("51c21cb7-4a9b-4593-a115-1b35fc631181"));

            migrationBuilder.DeleteData(
                table: "Restaurants",
                keyColumn: "Id",
                keyValue: new Guid("e6665130-760d-48cd-bbe9-c7ccc8dc212a"));

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "Restaurants");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "Products");

            migrationBuilder.InsertData(
                table: "Restaurants",
                columns: new[] { "Id", "CityId", "DeliveryPrice", "Description", "ImageURL", "Name", "TimeToDeliver" },
                values: new object[] { new Guid("1538ec84-fb26-4287-935c-8371df8861f8"), new Guid("9c7ab005-24e6-4b2e-a54d-70cf4d9658d1"), 4.00m, "Nunc magna arcu, pharetra ut mi vitae, hendrerit laoreet lacus.", "~\\images\\Restaurants\\download.png", "McDonald's", "15-20" });

            migrationBuilder.InsertData(
                table: "Restaurants",
                columns: new[] { "Id", "CityId", "DeliveryPrice", "Description", "ImageURL", "Name", "TimeToDeliver" },
                values: new object[] { new Guid("97352fb3-fff1-4137-8ba3-24eacd9b274d"), new Guid("9c7ab005-24e6-4b2e-a54d-70cf4d9658d1"), 5.00m, "Lorem ipsum dolor sit amet, consectetur adipiscing elit.", "~\\images\\Restaurants\\og_image.jpg", "Aladin Foods", "20-30" });
        }
    }
}
