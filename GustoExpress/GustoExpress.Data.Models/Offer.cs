using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using static GustoExpress.Data.Common.DataConstraints.Offer;

namespace GustoExpress.Data.Models
{
    public class Offer
    {
        public Offer()
        {
            Id = new Guid();
        }

        [Key]
        public Guid Id { get; set; }

        [Required]
        [MaxLength(OFFER_NAME_MAX_LENGHT), MinLength(OFFER_NAME_MIN_LENGHT)]
        public string Name { get; set; }

        [Required]
        [MaxLength(OFFER_DESCRIPTION_MAX_LENGHT), MinLength(OFFER_DESCRIPTION_MIN_LENGHT)]
        public string Description { get; set; }

        [ForeignKey(nameof(Restaurant))]
        public Guid RestaurantId { get; set; }
        public Restaurant Restaurant { get; set; }

        public ICollection<OfferProduct> OfferProducts { get; set; } = new List<OfferProduct>();

        [Required]
        public decimal Price { get; set; }

        public bool IsDeleted { get; set; } = false;

        public string? ImageURL { get; set; }
    }
}
