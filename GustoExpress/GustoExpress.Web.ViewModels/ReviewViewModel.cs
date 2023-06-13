using System.ComponentModel.DataAnnotations;

namespace GustoExpress.Web.ViewModels
{
    public class ReviewViewModel
    {
        public Guid Id { get; set; }

        public string UserId { get; set; } = null!;

        public string Title { get; set; } = null!;

        public string Text { get; set; } = null!;

        public int Stars { get; set; }
    }
}
