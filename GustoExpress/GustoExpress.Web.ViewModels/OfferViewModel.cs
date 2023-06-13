namespace GustoExpress.Web.ViewModels
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    using GustoExpress.Data.Models;
    using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
    using static GustoExpress.Data.Common.DataConstraints.Offer;

    public class OfferViewModel
    {
        public Guid Id { get; set; }

        [Required]
        [MaxLength(OFFER_NAME_MAX_LENGHT), MinLength(OFFER_NAME_MIN_LENGHT)]
        public string Name { get; set; } = null!;

        [Required]
        [MaxLength(OFFER_DESCRIPTION_MAX_LENGHT), MinLength(OFFER_DESCRIPTION_MIN_LENGHT)]
        public string Description { get; set; } = null!;

        [ForeignKey(nameof(Restaurant))]
        public Guid RestaurantId { get; set; }
        public Restaurant Restaurant { get; set; }

        public ICollection<OfferProduct> OfferProducts { get; set; } = new List<OfferProduct>();

        [Required]
        public decimal Price { get; set; }

        public decimal? Discount { get; set; }

        [ValidateNever]
        public decimal DiscountedPrice
        {
            get
            { return this.Price - (this.Discount ?? 0); }
        }

        public bool IsDeleted { get; set; } = false;

        public string? ImageURL { get; set; }
    }
}
