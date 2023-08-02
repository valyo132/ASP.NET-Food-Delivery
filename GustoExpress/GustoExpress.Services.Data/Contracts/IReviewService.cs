using GustoExpress.Data.Models;
using GustoExpress.Web.ViewModels;

namespace GustoExpress.Services.Data.Contracts
{
    public interface IReviewService
    {
        Task<Review> GetByIdAsync(string id);
        Task<ReviewViewModel> CreateReview(string userId, CreateReviewViewModel model);
        Task<ReviewViewModel> DeleteAsync(string id);
    }
}
