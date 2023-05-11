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

            return await MapReviewsWithUsers(reviews);
        }

        public async Task<ReviewDTO> GetById(string id)
        {
            var review = await _reviewRepository.GetById(id);

            ApiExceptionHandler.ObjectNotNull(review, $"Review with id '{id}'");

            return await MapReviewWithUser(review);
        }

        public async Task<List<ReviewDTO>> GetAllByReviewer(string reviewerId)
        {
            User user = await _userRepository.GetById(reviewerId);

            ApiExceptionHandler.ObjectNotNull(user, $"User with {reviewerId}");

            var reviews = await _reviewRepository.GetAllByReviewer(reviewerId);

            return await MapReviewsWithUsers(reviews);
        }

        public async Task<ReviewDTO> Create(ReviewCreateDTO reviewCreateDTO, string username)
        {
            UserDTO userDTO = await _userService.GetByUsername(username);

            var appointments = await _appointmentRepository.GetAllByBuyer(userDTO.Id);

            if (appointments.Count == 0)
            {
                ApiExceptionHandler.ThrowApiException(HttpStatusCode.BadRequest, "You can't leave a review if you haven't purchased the service.");
            }

            Review review = _mapper.Map<Review>(reviewCreateDTO);

            review.ReviewerId = userDTO.Id;

            await _reviewRepository.Create(review);

            return await MapReviewWithUser(review);
        }

        public async Task<ReviewDTO> Update(string id, ReviewUpdateDTO reviewUpdateDTO, string username)
        {
            UserDTO userDTO = await _userService.GetByUsername(username);

            Review review = await _reviewRepository.GetById(id);

            ApiExceptionHandler.ObjectNotNull(review, $"Review with id '{id}'");

            if (userDTO.Id != review.ReviewerId)
            {
                ApiExceptionHandler.ThrowApiException(HttpStatusCode.BadRequest, "You can't update this review.");
            }

            review = _mapper.Map(reviewUpdateDTO, review);

            await _reviewRepository.Update(review);

            return await MapReviewWithUser(review);
        }

        public async Task<bool> Delete(string reviewId)
        {
            Review review = await _reviewRepository.GetById(reviewId);

            ApiExceptionHandler.ObjectNotNull(review, $"Review with id '{reviewId}'");

            return await _reviewRepository.Delete(review);
        }

        private async Task<ReviewDTO> MapReviewWithUser(Review review)
        {
            ReviewDTO reviewDTO = _mapper.Map<ReviewDTO>(review);

            User user = await _userRepository.GetById(review.ReviewerId);
            reviewDTO.Reviewer = _mapper.Map<UserDTO>(user);

            return reviewDTO;
        }

        private async Task<List<ReviewDTO>> MapReviewsWithUsers(List<Review> reviews)
        {
            List<ReviewDTO> reviewsDTO = new();

            foreach (var review in reviews)
            {
                reviewsDTO.Add(await MapReviewWithUser(review));
            }

            return reviewsDTO;
        }
    }
}
