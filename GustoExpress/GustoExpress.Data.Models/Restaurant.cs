using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using static GustoExpress.Data.Common.DataConstraints.Restaurant;

namespace GustoExpress.Data.Models
{
    public class Restaurant
    {
        public Restaurant()
        {
            Id = Guid.NewGuid();
        }

        [Key]
        public Guid Id { get; set; }

        [Required]
        [MaxLength(RESTAURANT_NAME_MAX_LENGHT), MinLength(RESTAURANT_NAME_MIN_LENGHT)]
        public string Name { get; set; } = null!;

        [Required]
        [MaxLength(RESTAURANT_DESCRIPTION_MAX_LENGHT), MinLength(RESTAURANT_DESCRIPTION_MIN_LENGHT)]
        public string Description { get; set; } = null!;

        [ForeignKey(nameof(City))]
        public Guid CityId { get; set; }
        public City City { get; set; }

        [Required]
        public decimal DeliveryPrice { get; set; }

        [Required]
        public string TimeToDeliver { get; set; } = null!;
        
        public string? ImageURL { get; set; }

        public ICollection<Product> Products { get; set; } = new HashSet<Product>();
        public ICollection<Offer> Offers { get; set; } = new HashSet<Offer>();
    }
}
