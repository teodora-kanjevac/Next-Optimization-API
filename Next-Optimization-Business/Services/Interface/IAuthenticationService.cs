using NextOptimization.Business.DTOs;
using NextOptimization.Data.Models;

namespace NextOptimization.Business.Services
{
    public interface IAuthenticationService
    {
        Task<(bool, string)> AddPassword(User user, string password);
        Task<bool> ChangePassword(string id, UserChangePasswordDTO userDTO);
        Task<(bool, string)> ConfirmEmail(User user, string token);
        Task<bool> ForgotPassword(string email);
        Task<bool> ResetPassword(User user, string token, string newPassword);
        Task<bool> ResetPassword(UserResetPasswordDTO userDTO, string encodedUserIdAndToken);
        Task<JWTResultsDTO> SignIn(UserLoginDTO userDTO);
        Task SignOut();
        Task<UserDTO> SignUp(UserRegisterDTO userDTO, string encodedUserIdAndToken);
    }
}