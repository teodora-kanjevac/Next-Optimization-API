using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NextOptimization.Business.DTOs;
using NextOptimization.Business.Services;

namespace NextOptimization.API.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class ReviewAPIController : Controller
    {
        private readonly IReviewService _reviewService;

        public ReviewAPIController(IReviewService reviewService)
        {
            _reviewService = reviewService;
        }

        [HttpGet]
        public async Task<IEnumerable<ReviewDTO>> GetAll()
        {
            return await _reviewService.GetAll();
        }

        [HttpGet("id")]
        public async Task<ReviewDTO> GetById(string id)
        {
            return await _reviewService.GetById(id);
        }

        [HttpGet("all-by-reviewer")]
        public async Task<IEnumerable<ReviewDTO>> GetAllByReviewer(string id)
        {
            return await _reviewService.GetAllByReviewer(id);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] ReviewCreateDTO reviewCreateDTO)
        {
            string username = User.Identity.Name;

            var result = await _reviewService.Create(reviewCreateDTO, username);

            return Ok(result);
        }

        [HttpPut]
        public async Task<IActionResult> Update(string id, [FromBody] ReviewUpdateDTO reviewUpdateDTO)
        {
            string username = User.Identity.Name;

            var result = await _reviewService.Update(id, reviewUpdateDTO, username);

            return Ok(result);
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete]
        public async Task<IActionResult> Delete(string id)
        {
            var result = await _reviewService.Delete(id);

            return Ok(result);
        }
    }
}
