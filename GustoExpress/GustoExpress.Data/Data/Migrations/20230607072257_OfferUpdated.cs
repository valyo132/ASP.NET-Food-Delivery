using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GustoExpress.Web.Data.Migrations
{
    public partial class OfferUpdated : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Restaurants",
                keyColumn: "Id",
                keyValue: new Guid("51c21cb7-4a9b-4593-a115-1b35fc631181"));

            migrationBuilder.DeleteData(
                table: "Restaurants",
                keyColumn: "Id",
                keyValue: new Guid("e6665130-760d-48cd-bbe9-c7ccc8dc212a"));

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "Offers",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "Offers",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "Offers",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: false,
                defaultValue: "");

            migrationBuilder.InsertData(
                table: "Restaurants",
                columns: new[] { "Id", "CityId", "DeliveryPrice", "Description", "ImageURL", "IsDeleted", "Name", "TimeToDeliver" },
                values: new object[] { new Guid("419ede12-6073-42fb-ac30-217430d61382"), new Guid("9c7ab005-24e6-4b2e-a54d-70cf4d9658d1"), 5.00m, "Lorem ipsum dolor sit amet, consectetur adipiscing elit.", "\\images\\Restaurants\\og_image.jpg", false, "Aladin Foods", "20-30" });

            migrationBuilder.InsertData(
                table: "Restaurants",
                columns: new[] { "Id", "CityId", "DeliveryPrice", "Description", "ImageURL", "IsDeleted", "Name", "TimeToDeliver" },
                values: new object[] { new Guid("c5f0aaf2-99f2-43de-8e48-9a8fb97ccc44"), new Guid("9c7ab005-24e6-4b2e-a54d-70cf4d9658d1"), 4.00m, "Nunc magna arcu, pharetra ut mi vitae, hendrerit laoreet lacus.", "\\images\\Restaurants\\download.png", false, "McDonald's", "15-20" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Restaurants",
                keyColumn: "Id",
                keyValue: new Guid("419ede12-6073-42fb-ac30-217430d61382"));

            migrationBuilder.DeleteData(
                table: "Restaurants",
                keyColumn: "Id",
                keyValue: new Guid("c5f0aaf2-99f2-43de-8e48-9a8fb97ccc44"));

            migrationBuilder.DropColumn(
                name: "Description",
                table: "Offers");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "Offers");

            migrationBuilder.DropColumn(
                name: "Name",
                table: "Offers");

            migrationBuilder.InsertData(
                table: "Restaurants",
                columns: new[] { "Id", "CityId", "DeliveryPrice", "Description", "ImageURL", "IsDeleted", "Name", "TimeToDeliver" },
                values: new object[] { new Guid("51c21cb7-4a9b-4593-a115-1b35fc631181"), new Guid("9c7ab005-24e6-4b2e-a54d-70cf4d9658d1"), 4.00m, "Nunc magna arcu, pharetra ut mi vitae, hendrerit laoreet lacus.", "~\\images\\Restaurants\\download.png", false, "McDonald's", "15-20" });

            migrationBuilder.InsertData(
                table: "Restaurants",
                columns: new[] { "Id", "CityId", "DeliveryPrice", "Description", "ImageURL", "IsDeleted", "Name", "TimeToDeliver" },
                values: new object[] { new Guid("e6665130-760d-48cd-bbe9-c7ccc8dc212a"), new Guid("9c7ab005-24e6-4b2e-a54d-70cf4d9658d1"), 5.00m, "Lorem ipsum dolor sit amet, consectetur adipiscing elit.", "~\\images\\Restaurants\\og_image.jpg", false, "Aladin Foods", "20-30" });
        }
    }
}
