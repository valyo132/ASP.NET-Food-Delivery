namespace GustoExpress.Web.ViewModels
{
    using GustoExpress.Data.Models;

    public class OrderViewModel
    {
        public Guid Id { get; set; }
        public string UserId { get; set; }
        public Guid RestaurantId { get; set; }
        public Restaurant Restaurant { get; set; }

        public decimal TotalCost { get; set; }

        public ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();
    }
}
