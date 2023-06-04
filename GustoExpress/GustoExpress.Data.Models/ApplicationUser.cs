using Microsoft.AspNetCore.Identity;

namespace GustoExpress.Data.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string FirstName { get; set; } = null!;
        public string LastName { get; set; } = null!;

        public ICollection<Order> Orders { get; set; } = new HashSet<Order>();
        public ICollection<Review> Reviews { get; set; } = new HashSet<Review>();
    }
}
