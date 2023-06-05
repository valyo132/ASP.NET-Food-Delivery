using GustoExpress.Data.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GustoExpress.Web.ViewModels
{
    public class RestaurantPageViewModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = null!;
        public string Description { get; set; } = null!;
        public City City { get; set; }
        public decimal DeliveryPrice { get; set; }
        public string TimeToDeliver { get; set; } = null!;

        public string? ImageURL { get; set; }

        public ICollection<Product> Products { get; set; } = new HashSet<Product>();
        public ICollection<Offer> Offers { get; set; } = new HashSet<Offer>();
    }
}
