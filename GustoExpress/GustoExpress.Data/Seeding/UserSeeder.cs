namespace GustoExpress.Data.Seeding
{
    using Microsoft.AspNetCore.Identity;

    using GustoExpress.Data.Models;
    internal class UserSeeder
    {
        internal ApplicationUser GenerateAdmin()
        {
            ApplicationUser admin = new ApplicationUser();

            admin.Id = "1cd0be45-b9be-4fc9-9b0f-46a1d20a77be";
            admin.Email = "admin@admin.com";
            admin.NormalizedEmail = "ADMIN@ADMIN.COM";
            admin.UserName = "admin@admin.com";
            admin.NormalizedUserName = "ADMIN@ADMIN.COM";
            admin.EmailConfirmed = false;
            admin.FirstName = "Best";
            admin.LastName = "Admin";

            PasswordHasher<ApplicationUser> ph = new PasswordHasher<ApplicationUser>();
            admin.PasswordHash = ph.HashPassword(admin, "admin123");

            return admin;
        }
    }
}
