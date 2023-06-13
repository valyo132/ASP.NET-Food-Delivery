using GustoExpress.Data.Models;
using GustoExpress.Data.Models.Enums;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

using static GustoExpress.Data.Common.DataConstraints.Product;


namespace GustoExpress.Web.ViewModels
{
    public class ProductViewModel
    {
        public Guid Id { get; set; }

        [Required]
        [MaxLength(PRODUCT_NAME_MAX_LENGHT), MinLength(PRODUCT_NAME_MIN_LENGHT)]
        public string Name { get; set; } = null!;

        [Required]
        public Category Category { get; set; }

        [Required]
        [MaxLength(PRODUCT_DESCRIPTION_MAX_LENGHT), MinLength(PRODUCT_DESCRIPTION_MIN_LENGHT)]
        public string Description { get; set; } = null!;

        [ForeignKey(nameof(Restaurant))]
        public Guid RestaurantId { get; set; }
        public Restaurant Restaurant { get; set; }

        [Required]
        public decimal Price { get; set; }

        public decimal? Discount { get; set; }

        [ValidateNever]
        public decimal DiscountedPrice
        {
            get
            { return this.Price - (this.Discount ?? 0); }
        }

        [Required]
        public decimal Grams { get; set; }

        public bool IsDeleted { get; set; } = false;

        public string? ImageURL { get; set; }
    }
}
