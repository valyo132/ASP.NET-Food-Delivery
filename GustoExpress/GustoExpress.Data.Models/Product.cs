using GustoExpress.Data.Models.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using static GustoExpress.Data.Common.DataConstraints.Product;

namespace GustoExpress.Data.Models
{
    public class Product
    {
        public Product()
        {
            Id = new Guid();
        }

        [Key]
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

        [Required]
        public decimal Grams { get; set; }

        public bool IsDeleted { get; set; } = false;

        public string? ImageURL { get; set; }
    }
}
