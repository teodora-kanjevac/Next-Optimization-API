using Microsoft.EntityFrameworkCore;
using NextOptimization.Data.Models;

namespace NextOptimization.Data.Repositories
{
    public class ReviewRepository : IReviewRepository
    {
        private readonly NextOptimizationContext _nextOptimizationContext;

        public ReviewRepository(NextOptimizationContext nextOptimizationContext)
        {
            _nextOptimizationContext = nextOptimizationContext;
        }

        public async Task<List<Review>> GetAll()
        {
            return await _nextOptimizationContext.Reviews.ToListAsync();
        }

        public async Task<Review> GetById(string id)
        {
            return await _nextOptimizationContext.Reviews.FirstOrDefaultAsync(r => r.Id == id);
        }

        public async Task<List<Review>> GetAllByReviewer(string reviewerId)
        {
            return await _nextOptimizationContext.Reviews.Where(p => p.ReviewerId == reviewerId).ToListAsync();
        }

        public async Task<Review> Create(Review review)
        {
            await _nextOptimizationContext.Reviews.AddAsync(review);

            await _nextOptimizationContext.SaveChangesAsync();

            return review;
        }

        public async Task<Review> Update(Review review)
        {
            _nextOptimizationContext.Reviews.Update(review);

            await _nextOptimizationContext.SaveChangesAsync();

            return review;
        }

        public async Task<bool> Delete(Review review)
        {
            _nextOptimizationContext.Reviews.Remove(review);

            return await _nextOptimizationContext.SaveChangesAsync() > 0;
        }
    }
}
