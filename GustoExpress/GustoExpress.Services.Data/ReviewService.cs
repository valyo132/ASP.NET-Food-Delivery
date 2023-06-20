using AutoMapper;
using GustoExpress.Data.Models;
using GustoExpress.Services.Data.Contracts;
using GustoExpress.Web.Data;
using GustoExpress.Web.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace GustoExpress.Services.Data
{
    public class ReviewService : IReviewService
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public ReviewService(ApplicationDbContext context,
            IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<Review> CreateReview(string userId, CreateReviewViewModel model)
        {
            Review review = _mapper.Map<Review>(model);
            review.UserId = userId;

            await _context.Reviews.AddAsync(review);
            await _context.SaveChangesAsync();

            return review;
        }

        public async Task<ReviewViewModel> DeleteAsync(string id)
        {
            Review reviewToDelete = await GetByIdAsync(id);

            _context.Reviews.Remove(reviewToDelete);
            await _context.SaveChangesAsync();

            return ProjectTo<ReviewViewModel>(reviewToDelete);
        }

        private async Task<Review> GetByIdAsync(string id)
        {
            return await _context.Reviews
                .Include(r => r.User)
                .FirstOrDefaultAsync(r => r.Id.ToString() == id);
        }

        private T ProjectTo<T>(Review review)
        {
            return _mapper.Map<T>(review);
        }
    }
}
