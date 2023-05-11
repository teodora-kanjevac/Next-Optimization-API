using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NextOptimization.Business.DTOs;
using NextOptimization.Business.Services;

namespace NextOptimization.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserAPIController : ControllerBase
    {
        private readonly IUserService _userService;

        public UserAPIController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpGet]
        public async Task<List<UserDTO>> GetAll()
        {
            return await _userService.GetAll();
        }

        [HttpGet("id")]
        public async Task<UserDTO> GetById(string id)
        {
            return await _userService.GetById(id);
        }

        [HttpGet("email")]
        public async Task<UserDTO> GetByEmail(string email)
        {
            return await _userService.GetByEmail(email);
        }

        [HttpGet("get-logged-in-user")]
        public async Task<UserDTO> GetLoggedInUser()
        {
            var userName = User.Identity.Name;

            return await _userService.GetByUsername(userName);
        }

        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] UserCreateDTO userCreateDTO)
        {
            var result = await _userService.Create(userCreateDTO);

            return Ok(result);
        }

        [HttpPut("id")]
        public async Task<IActionResult> Update(string id, [FromBody] UserUpdateDTO userUpdateDTO)
        {
            var username = User.Identity.Name;

            var result = await _userService.Update(username, id, userUpdateDTO);

            return Ok(result);
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("id")]
        public async Task<bool> Delete(string id)
        {
            return await _userService.Delete(id);
        }
    }
}
