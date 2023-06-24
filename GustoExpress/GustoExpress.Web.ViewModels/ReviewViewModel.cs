namespace GustoExpress.Web.ViewModels
{
    using System.ComponentModel.DataAnnotations.Schema;

    using GustoExpress.Data.Models;

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

        public bool IsDeleted { get; set; }

        public DateTime CreatedOn { get; set; }
    }
}
