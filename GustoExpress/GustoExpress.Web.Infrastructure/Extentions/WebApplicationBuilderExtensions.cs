namespace GustoExpress.Web.Infrastructure.Extentions
{
    using System.Security.Claims;

    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.Extensions.DependencyInjection;

    using GustoExpress.Data.Models;

    public static class WebApplicationBuilderExtensions
    {
        public static IApplicationBuilder SeedAdministrator(this IApplicationBuilder app, string email)
        {
            using IServiceScope scopedServices = app.ApplicationServices.CreateScope();

            IServiceProvider serviceProvider = scopedServices.ServiceProvider;

            UserManager<ApplicationUser> userManager =
                serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();
            RoleManager<IdentityRole> roleManager =
                serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();

            Task.Run(async () =>
            {
                if (await roleManager.RoleExistsAsync("admin@admin.com"))
                {
                    return;
                }

                IdentityRole role = new IdentityRole() { Name = "Admin" };

                await roleManager.CreateAsync(role);

                ApplicationUser adminUser =
                    await userManager.FindByEmailAsync(email);

                await userManager.AddToRoleAsync(adminUser, "Admin");

                var fiendlyNameClaim = new Claim("FriendlyName", $"{adminUser.FirstName} {adminUser.LastName}");
                await userManager.AddClaimAsync(adminUser, fiendlyNameClaim);
            })
            .GetAwaiter()
            .GetResult();

            return app;
        }
    }
}
