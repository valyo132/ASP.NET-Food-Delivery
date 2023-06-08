using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GustoExpress.Web.Data.Migrations
{
    public partial class AddedOfferProductEntity : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Products_Offers_OfferId",
                table: "Products");

            migrationBuilder.DropIndex(
                name: "IX_Products_OfferId",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "OfferId",
                table: "Products");

            migrationBuilder.CreateTable(
                name: "OfferProducts",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ProductId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    OfferId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OfferProducts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_OfferProducts_Offers_OfferId",
                        column: x => x.OfferId,
                        principalTable: "Offers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_OfferProducts_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_OfferProducts_OfferId",
                table: "OfferProducts",
                column: "OfferId");

            migrationBuilder.CreateIndex(
                name: "IX_OfferProducts_ProductId",
                table: "OfferProducts",
                column: "ProductId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "OfferProducts");

            migrationBuilder.AddColumn<Guid>(
                name: "OfferId",
                table: "Products",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Products_OfferId",
                table: "Products",
                column: "OfferId");

            migrationBuilder.AddForeignKey(
                name: "FK_Products_Offers_OfferId",
                table: "Products",
                column: "OfferId",
                principalTable: "Offers",
                principalColumn: "Id");
        }
    }
}
