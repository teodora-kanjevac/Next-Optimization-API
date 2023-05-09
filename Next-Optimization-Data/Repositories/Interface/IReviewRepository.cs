using NextOptimization.Data.Models;

namespace NextOptimization.Data.Repositories
{
    public interface IReviewRepository
    {
        Task<Review> Create(Review review);
        Task<bool> Delete(Review review);
        Task<List<Review>> GetAll();
        Task<List<Review>> GetAllByReviewer(string reviewerId);
        Task<Review> GetById(string id);
        Task<Review> Update(Review review);
    }
}