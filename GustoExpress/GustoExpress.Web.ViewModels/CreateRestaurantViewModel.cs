using System.ComponentModel.DataAnnotations;
using static GustoExpress.Data.Common.DataConstraints.Restaurant;
using static GustoExpress.Data.Common.DataConstraints.City;

namespace GustoExpress.Web.ViewModels
{
    public class CreateRestaurantViewModel
    {
        [Required]
        [MaxLength(RESTAURANT_NAME_MAX_LENGHT), MinLength(RESTAURANT_NAME_MIN_LENGHT)]
        public string Name { get; set; } = null!;

        [Required]
        [MaxLength(RESTAURANT_DESCRIPTION_MAX_LENGHT), MinLength(RESTAURANT_DESCRIPTION_MIN_LENGHT)]
        public string Description { get; set; } = null!;

        [Required]
        [MaxLength(CITY_NAME_MAX_LENGHT), MinLength(CITY_NAME_MIN_LENGHT)]
        public string City { get; set; } = null!;

        [Required]
        [Range(1, (double)decimal.MaxValue)]
        public decimal DeliveryPrice { get; set; }

        [Required]
        [Range(1, int.MaxValue)]
        public int MinTimeToDeliver { get; set; }

        [Required]
        [Range(1, int.MaxValue)]
        public int MaxTimeToDeliver { get; set; }

        public string? ImageURL { get; set; }
    }
}
