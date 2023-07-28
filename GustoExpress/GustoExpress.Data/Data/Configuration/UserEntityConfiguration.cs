namespace GustoExpress.Data.Data.Configuration
{
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    using GustoExpress.Data.Models;
    using GustoExpress.Data.Seeding;

    public class UserEntityConfiguration : IEntityTypeConfiguration<ApplicationUser>
    {
        private readonly UserSeeder _userSeeder;

        public UserEntityConfiguration()
        {
            _userSeeder = new UserSeeder();
        }

        public void Configure(EntityTypeBuilder<ApplicationUser> builder)
        {
            builder.HasData(this._userSeeder.GenerateAdmin());
        }
    }
}
