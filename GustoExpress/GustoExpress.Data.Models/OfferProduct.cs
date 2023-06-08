using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GustoExpress.Data.Models
{
    public class OfferProduct
    {
        public OfferProduct()
        {
            Id = new Guid();
        }

        [Key]
        public Guid Id { get; set; }

        [ForeignKey(nameof(Product))]
        public Guid ProductId { get; set; }
        public Product Product { get; set; }

        [ForeignKey(nameof(Offer))]
        public Guid OfferId { get; set; }
        public Offer Offer { get; set; }
    }
}
