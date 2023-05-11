using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NextOptimization.Business.DTOs;
using NextOptimization.Business.Services;

namespace NextOptimization.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticationAPIController : ControllerBase
    {
        private readonly IAuthenticationService _authenticationService;

        public AuthenticationAPIController(IAuthenticationService authenticationService)
        {
            _authenticationService = authenticationService;
        }

        [AllowAnonymous]
        [HttpPost("sign-in")]
        public async Task<IActionResult> SignIn([FromBody] UserLoginDTO user)
        {
            var result = await _authenticationService.SignIn(user);

            return Ok(result);
        }

        [HttpPost("sign-out")]
        public async Task<IActionResult> SignOut()
        {
            await _authenticationService.SignOut();

            return Ok();
        }

        [AllowAnonymous]
        [HttpPost("sign-up")]
        public async Task<IActionResult> SignUp(UserRegisterDTO user, string encodedUserIdAndToken)
        {
            var result = await _authenticationService.SignUp(user, encodedUserIdAndToken);

            return Ok(result);
        }

        [HttpPut("change-password")]
        public async Task<IActionResult> ChangePassword(string id, [FromBody] UserChangePasswordDTO user)
        {
            var result = await _authenticationService.ChangePassword(id, user);

            return Ok(result);
        }

        [AllowAnonymous]
        [HttpPost("forgot-password")]
        public async Task<IActionResult> ForgotPassword(string email)
        {
            var result = await _authenticationService.ForgotPassword(email);

            return Ok(result);
        }

        [AllowAnonymous]
        [HttpPut("reset-password")]
        public async Task<IActionResult> ResetPassword([FromBody] UserResetPasswordDTO user, string encodedUserIdAndToken)
        {
            var result = await _authenticationService.ResetPassword(user, encodedUserIdAndToken);

            return Ok(result);
        }
    }
}
