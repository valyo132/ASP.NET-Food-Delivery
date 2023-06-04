using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using static GustoExpress.Data.Common.DataConstraints.Review;

namespace GustoExpress.Data.Models
{
    public class Review
    {
        public Review()
        {
            Id = new Guid();
        }

        [Key]
        public Guid Id { get; set; }

        [ForeignKey(nameof(User))]
        public string UserId { get; set; }
        public ApplicationUser User { get; set; }

        [Required]
        [MaxLength(REVIEW_TITLE_MAX_LENGHT), MinLength(REVIEW_TITLE_MIN_LENGHT)]
        public string Title { get; set; } = null!;

        [Required]
        [MaxLength(REVIEW_TEXT_MAX_LENGHT), MinLength(REVIEW_TEXT_MIN_LENGHT)]
        public string Text { get; set; } = null!;

        [Required]
        public int Stars { get; set; }
    }
}
