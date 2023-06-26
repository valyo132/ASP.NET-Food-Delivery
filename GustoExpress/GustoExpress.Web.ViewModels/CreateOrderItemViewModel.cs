namespace GustoExpress.Web.ViewModels
{
    using GustoExpress.Data.Models;
    using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
    using System.ComponentModel.DataAnnotations;

    public class CreateOrderItemViewModel
    {
        [Range(1, 1000)]
        public int Quantity { get; set; } = 1;

        [ValidateNever]
        public string UserId { get; set; } = null!;

        [Required]
        public decimal TotalCost { get; set; }

        [Required]
        public string RestaurantId { get; set; } = null!;

        public string? ProductId { get; set; }
        public Product? Product { get; set; }

        public string? OfferId { get; set; }
        public Offer? Offer { get; set; }
    }
}
