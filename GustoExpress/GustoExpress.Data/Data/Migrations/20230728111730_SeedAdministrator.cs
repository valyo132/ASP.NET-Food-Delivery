using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GustoExpress.Web.Data.Migrations
{
    public partial class SeedAdministrator : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "ConcurrencyStamp", "Discriminator", "Email", "EmailConfirmed", "FirstName", "LastName", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "SecurityStamp", "TwoFactorEnabled", "UserName" },
                values: new object[] { "1cd0be45-b9be-4fc9-9b0f-46a1d20a77be", 0, "cde4c5fa-4c51-48e2-a7f8-c2e3323a5f56", "ApplicationUser", "admin@admin.com", false, "Best", "Admin", false, null, "ADMIN@ADMIN.COM", "ADMIN@ADMIN.COM", "AQAAAAEAACcQAAAAEKOrNtp2wPZoUJvrcv40hB63DrqaZhmec26/m0WJzM9xvHeLEJnEfEcDI/IaoahU7w==", null, false, "11169612-f2e1-400f-a440-c6bdd2efc430", false, "admin@admin.com" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "1cd0be45-b9be-4fc9-9b0f-46a1d20a77be");
        }
    }
}
