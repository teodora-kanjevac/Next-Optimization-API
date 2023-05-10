using NextOptimization.Business.DTOs;

namespace NextOptimization.Business.Services
{
    public interface IReviewService
    {
        Task<ReviewDTO> Create(ReviewCreateDTO reviewCreateDTO, string username);
        Task<bool> Delete(string reviewId);
        Task<List<ReviewDTO>> GetAll();
        Task<List<ReviewDTO>> GetAllByReviewer(string reviewerId);
        Task<ReviewDTO> GetById(string id);
        Task<ReviewDTO> Update(string id, ReviewUpdateDTO reviewUpdateDTO, string username);
    }
}