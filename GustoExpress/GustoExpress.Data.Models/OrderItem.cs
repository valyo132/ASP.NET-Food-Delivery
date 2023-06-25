using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GustoExpress.Data.Models
{
    public class OrderItem
    {
        public OrderItem()
        {
            Id = new Guid();
        }

        [Key]
        public Guid Id { get; set; }

        [Required]
        public int Quantity { get; set; } = 1;

        [Required]
        public decimal TotalCost { get; set; }

        [ForeignKey(nameof(Product))]
        public Guid ProductId { get; set; }
        public Product Product { get; set; }

        [ForeignKey(nameof(Offer))]
        public Guid OfferId { get; set; }
        public Offer Offer { get; set; }
    }
}
