using GustoExpress.Data.Models;
using GustoExpress.Web.ViewModels;

namespace GustoExpress.Services.Data.Contracts
{
    public interface IReviewService
    {
        Task<Review> CreateReview(string userId, CreateReviewViewModel model);
    }
}
