using GustoExpress.Data.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

using static GustoExpress.Data.Common.DataConstraints.Restaurant;

namespace GustoExpress.Web.ViewModels
{
    public class RestaurantViewModel
    {
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

        public bool IsDeleted { get; set; } = false;

        public string? ImageURL { get; set; }
    }
}
