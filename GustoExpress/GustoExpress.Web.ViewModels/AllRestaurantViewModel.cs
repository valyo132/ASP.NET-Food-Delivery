using GustoExpress.Data.Models;

namespace GustoExpress.Web.ViewModels
{
    public class AllRestaurantViewModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = null!;
        public string Description { get; set; } = null!;
        public City City { get; set; }
        public decimal DeliveryPrice { get; set; }
        public string TimeToDeliver { get; set; } = null!;
        public string? ImageURL { get; set; }
        public bool IsDeleted { get; set; }
    }
}
