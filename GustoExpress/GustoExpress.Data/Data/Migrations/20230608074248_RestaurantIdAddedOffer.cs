using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GustoExpress.Web.Data.Migrations
{
    public partial class RestaurantIdAddedOffer : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Offers_Restaurants_RestaurantId",
                table: "Offers");

            migrationBuilder.AlterColumn<Guid>(
                name: "RestaurantId",
                table: "Offers",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Offers_Restaurants_RestaurantId",
                table: "Offers",
                column: "RestaurantId",
                principalTable: "Restaurants",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Offers_Restaurants_RestaurantId",
                table: "Offers");

            migrationBuilder.AlterColumn<Guid>(
                name: "RestaurantId",
                table: "Offers",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AddForeignKey(
                name: "FK_Offers_Restaurants_RestaurantId",
                table: "Offers",
                column: "RestaurantId",
                principalTable: "Restaurants",
                principalColumn: "Id");
        }
    }
}
