namespace GustoExpress.Services.Data
{
    using AutoMapper;
    using Microsoft.EntityFrameworkCore;

    using GustoExpress.Data.Models;
    using GustoExpress.Services.Data.Contracts;
    using GustoExpress.Services.Data.Helpers;
    using GustoExpress.Web.Data;
    using GustoExpress.Web.ViewModels;

    public class ReviewService : IReviewService, IProjectable<Review>
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

            reviewToDelete.IsDeleted = true;
            await _context.SaveChangesAsync();

            return ProjectTo<ReviewViewModel>(reviewToDelete);
        }

        private async Task<Review> GetByIdAsync(string id)
        {
            return await _context.Reviews
                .Include(r => r.User)
                .FirstOrDefaultAsync(r => r.Id.ToString() == id);
        }

        public T ProjectTo<T>(Review review)
        {
            return _mapper.Map<T>(review);
        }
    }
}
