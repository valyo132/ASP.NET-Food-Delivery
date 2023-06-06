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
        public string Name { get; set; } = null!;

        [Required]
        public Category Category { get; set; }

        [ValidateNever]
        public IEnumerable<SelectListItem> CategoryList { get; set; }

        [Required]
        [MaxLength(PRODUCT_DESCRIPTION_MAX_LENGHT), MinLength(PRODUCT_DESCRIPTION_MIN_LENGHT)]
        public string Description { get; set; } = null!;

        public string RestaurantId { get; set; }

        [Required]
        public decimal Price { get; set; }

        [Required]
        public decimal Grams { get; set; }

        public string? ImageURL { get; set; }
    }
}
