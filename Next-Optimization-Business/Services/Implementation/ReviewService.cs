using AutoMapper;
using NextOptimization.Business.DTOs;
using NextOptimization.Business.Middleware;
using NextOptimization.Data.Models;
using NextOptimization.Data.Repositories;
using System.Net;

namespace NextOptimization.Business.Services
{
    public class ReviewService : IReviewService
    {
        private readonly IReviewRepository _reviewRepository;
        private readonly IAppointmentRepository _appointmentRepository;
        private readonly IUserRepository _userRepository;
        private readonly IUserService _userService;
        private readonly IMapper _mapper;

        public ReviewService(IReviewRepository reviewRepository, IUserRepository userRepository, 
            IMapper mapper, IUserService userService, IAppointmentRepository appointmentRepository)
        {
            _reviewRepository = reviewRepository;
            _userRepository = userRepository;
            _mapper = mapper;
            _userService = userService;
            _appointmentRepository = appointmentRepository;
        }

        public async Task<List<ReviewDTO>> GetAll()
        {
            var reviews = await _reviewRepository.GetAll();

            return _mapper.Map<List<ReviewDTO>>(reviews);
        }

        public async Task<ReviewDTO> GetById(string id)
        {
            var review = await _reviewRepository.GetById(id);

            ApiExceptionHandler.ObjectNotNull(review, $"Review with {id}");

            return _mapper.Map<ReviewDTO>(review);
        }

        public async Task<List<ReviewDTO>> GetAllByReviewer(string reviewerId)
        {
            User user = await _userRepository.GetById(reviewerId);

            ApiExceptionHandler.ObjectNotNull(user, $"User with {reviewerId}");

            var reviews = await _reviewRepository.GetAllByReviewer(reviewerId);

            return _mapper.Map<List<ReviewDTO>>(reviews);
        }

        public async Task<ReviewDTO> Create(ReviewCreateDTO reviewCreateDTO, string username)
        {
            UserDTO userDTO = await _userService.GetByUsername(username);

            if (await _appointmentRepository.GetAllByBuyer(userDTO.Id) == null)
            {
                ApiExceptionHandler.ThrowApiException(HttpStatusCode.BadRequest, "You can't leave a review if you haven't purchased the service.");
            }

            ReviewDTO reviewDTO = _mapper.Map<ReviewDTO>(reviewCreateDTO);

            reviewDTO.Reviewer = userDTO;

            await _reviewRepository.Create(_mapper.Map<Review>(reviewDTO));

            return reviewDTO;
        }

        public async Task<ReviewDTO> Update(string id, ReviewUpdateDTO reviewUpdateDTO, string username)
        {
            UserDTO userDTO = await _userService.GetByUsername(username);

            Review review = await _reviewRepository.GetById(id);

            ApiExceptionHandler.ObjectNotNull(review, $"Review with {id}");

            if (userDTO.Id != review.ReviewerId)
            {
                ApiExceptionHandler.ThrowApiException(HttpStatusCode.BadRequest, "You can't update this review.");
            }

            review = _mapper.Map(reviewUpdateDTO, review);

            await _reviewRepository.Update(review);

            return _mapper.Map<ReviewDTO>(review);
        }

        public async Task<bool> Delete(string reviewId)
        {
            Review review = await _reviewRepository.GetById(reviewId);

            ApiExceptionHandler.ObjectNotNull(review, $"Review with {reviewId}");

            return await _reviewRepository.Delete(review);
        }
    }
}
