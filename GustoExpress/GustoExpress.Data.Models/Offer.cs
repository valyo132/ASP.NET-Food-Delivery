using System.ComponentModel.DataAnnotations;

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

        public ICollection<Product> Products { get; set; } = new HashSet<Product>();

        [Required]
        public decimal Price { get; set; }

        public string? ImageURL { get; set; }
    }
}
