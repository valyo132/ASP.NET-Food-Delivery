namespace GustoExpress.Web.ViewModels
{
    using System.ComponentModel.DataAnnotations;

    using static GustoExpress.Data.Common.DataConstraints.Offer;
    using GustoExpress.Data.Models;
    using Microsoft.AspNetCore.Mvc.Rendering;
    using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

    public class CreateOfferViewModel
    {
        [Required]
        [MaxLength(OFFER_NAME_MAX_LENGHT), MinLength(OFFER_NAME_MIN_LENGHT)]
        public string Name { get; set; } = null!;

        [Required]
        [MaxLength(OFFER_DESCRIPTION_MAX_LENGHT), MinLength(OFFER_DESCRIPTION_MIN_LENGHT)]
        public string Description { get; set; } = null!;

        [Required]
        public string FirstProductId { get; set; } = null!;

        [Required]
        public string SecondProductId { get; set; } = null!;
        public string ThirdhProductId { get; set; } = null!;

        [ValidateNever]
        public string RestaurantId { get; set; } = null!;

        [ValidateNever]
        public IEnumerable<SelectListItem> ProductsToChoose { get; set; }

        [Required]
        [Range(1, (double)decimal.MaxValue)]
        public decimal Price { get; set; }

        [Range(1, (double)decimal.MaxValue)]
        public decimal? Discount { get; set; }

        public string? ImageURL { get; set; }
    }
}
