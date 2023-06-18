using System.ComponentModel.DataAnnotations;

using static GustoExpress.Data.Common.DataConstraints.Review;

namespace GustoExpress.Web.ViewModels
{
    public class CreateReviewViewModel
    {
        [Required]
        [MaxLength(REVIEW_TITLE_MAX_LENGHT), MinLength(REVIEW_TITLE_MIN_LENGHT)]
        public string Title { get; set; } = null!;

        [Required]
        [MaxLength(REVIEW_TEXT_MAX_LENGHT), MinLength(REVIEW_TEXT_MIN_LENGHT)]
        public string Text { get; set; } = null!;

        [Required]
        public string RestaurantId { get; set; } = null!;

        [Required]
        [Range(0, 5)]
        public int Stars { get; set; }
    }
}
