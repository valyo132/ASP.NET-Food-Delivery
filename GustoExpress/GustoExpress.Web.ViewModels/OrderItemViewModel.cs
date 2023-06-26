namespace GustoExpress.Web.ViewModels
{
    using GustoExpress.Data.Models;

    public class OrderItemViewModel
    {
        public int Quantity { get; set; } = 1;

        public decimal TotalCost { get; set; }

        public string UserId { get; set; } = null!;

        public string RestaurantId { get; set; } = null!;

        public string? ProductId { get; set; }
        public Product? Product { get; set; }

        public string? OfferId { get; set; }
        public Offer? Offer { get; set; }
    }
}
