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

        public bool IsDeleted { get; set; } = false;

        [Required]
        [ForeignKey(nameof(User))]
        public string UserId { get; set; }
        public ApplicationUser User { get; set; }

        [ForeignKey(nameof(Order))]
        public Guid? OrderId { get; set; }
        public Order? Order { get; set; }

        [ForeignKey(nameof(Product))]
        public Guid? ProductId { get; set; }
        public Product? Product { get; set; }

        [ForeignKey(nameof(Offer))]
        public Guid? OfferId { get; set; }
        public Offer? Offer { get; set; }
    }
}
