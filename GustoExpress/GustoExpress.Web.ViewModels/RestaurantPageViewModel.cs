using GustoExpress.Data.Models;
using System.Runtime.CompilerServices;

namespace GustoExpress.Web.ViewModels
{
    public class RestaurantPageViewModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = null!;
        public string Description { get; set; } = null!;
        public City City { get; set; }
        public Order Order { get; set; }
        public decimal DeliveryPrice { get; set; }
        public string TimeToDeliver { get; set; } = null!;

        public string? ImageURL { get; set; }

        public double Rating
        {
            get
            {
                if (this.Reviews.Where(r => r.IsDeleted == false).Any())
                    return this.Reviews.Where(r => r.IsDeleted == false).Average(r => r.Stars);

                return 0;
            }
        }

        public ICollection<ProductViewModel> Products { get; set; } = new HashSet<ProductViewModel>();
        public ICollection<OfferViewModel> Offers { get; set; } = new HashSet<OfferViewModel>();
        public ICollection<ReviewViewModel> Reviews { get; set; } = new HashSet<ReviewViewModel>();
    }
}
