using GustoExpress.Data.Models;
using GustoExpress.Data.Models.Enums;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

using static GustoExpress.Data.Common.DataConstraints.Product;

namespace GustoExpress.Web.ViewModels
{
    public class CreateProductViewModel
    {
        [Required]
        [MaxLength(PRODUCT_NAME_MAX_LENGHT), MinLength(PRODUCT_NAME_MIN_LENGHT)]
        public string Name { get; set; } = null!;

        [Required]
        public Category Category { get; set; }

        [ValidateNever]
        public IEnumerable<SelectListItem> CategoryList { get; set; }

        [Required]
        [MaxLength(PRODUCT_DESCRIPTION_MAX_LENGHT), MinLength(PRODUCT_DESCRIPTION_MIN_LENGHT)]
        public string Description { get; set; } = null!;

        public string RestaurantId { get; set; } = null!;

        [Required]
        [Range(0, 10_000)]
        public decimal Price { get; set; }

        [Range(0, 10_000)]
        public decimal? Discount { get; set; }

        [Required]
        [Range(1, 2_000)]
        public decimal Grams { get; set; }

        public string? ImageURL { get; set; }
    }
}
