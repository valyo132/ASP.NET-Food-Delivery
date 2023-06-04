using System.ComponentModel.DataAnnotations;

using static GustoExpress.Data.Common.DataConstraints.City;

namespace GustoExpress.Data.Models
{
    public class City
    {
        public City()
        {
            Id = new Guid();
        }

        [Key]
        public Guid Id { get; set; }

        [Required]
        [MaxLength(CITY_NAME_MAX_LENGHT), MinLength(CITY_NAME_MIN_LENGHT)]
        public string CityName { get; set; } = null!;

        public ICollection<Restaurant> Restaurants { get; set; } = new HashSet<Restaurant>();
    }
}
