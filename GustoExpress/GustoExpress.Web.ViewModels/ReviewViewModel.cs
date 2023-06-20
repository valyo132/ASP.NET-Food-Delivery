using GustoExpress.Data.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GustoExpress.Web.ViewModels
{
    public class ReviewViewModel
    {
        public Guid Id { get; set; }

        public string UserId { get; set; } = null!;

        [ForeignKey(nameof(UserId))]
        public ApplicationUser User { get; set; } = null!;

        public string Title { get; set; } = null!;

        public string Text { get; set; } = null!;

        public int Stars { get; set; }

        public string RestaurantId { get; set; } = null!;
    }
}
