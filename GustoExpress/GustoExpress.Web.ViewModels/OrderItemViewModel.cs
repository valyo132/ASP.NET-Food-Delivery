namespace GustoExpress.Web.ViewModels
{
    using GustoExpress.Data.Models;
    using System.ComponentModel.DataAnnotations;

    public class CreateOrderItemViewModel
    {
        [Range(1, 1000)]
        public int Quantity { get; set; } = 1;

        [Required]
        public decimal TotalCost { get; set; }

        public string? ProductId { get; set; }
        public Product? Product { get; set; }

        public string? OfferId { get; set; }
        public Offer? Offer { get; set; }
    }
}
