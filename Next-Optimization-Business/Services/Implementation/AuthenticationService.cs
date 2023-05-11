using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using NextOptimization.Business.DTOs;
using NextOptimization.Business.Middleware;
using NextOptimization.Business.TokenGenerator;
using NextOptimization.Data.Models;
using NextOptimization.Data.Repositories;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Transactions;
using System.Web;

namespace NextOptimization.Business.Services
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly SignInManager<User> _signInManager;
        private readonly UserManager<User> _userManager;
        private readonly IUserRepository _userRepository;
        private readonly IUserService _userService;
        private readonly IEmailService _emailService;
        private readonly ITokenGenerator _tokenGenerator;
        private readonly IConfiguration _configuration;
        private readonly IMapper _mapper;

        public AuthenticationService(
            IUserRepository userRepository,
            IMapper mapper, UserManager<User> userManager,
            SignInManager<User> signInManager,
            IUserService userService,
            ITokenGenerator tokenGenerator,
            IConfiguration configuration, 
            IEmailService emailService)
        {
            _userRepository = userRepository;
            _mapper = mapper;
            _userManager = userManager;
            _userService = userService;
            _tokenGenerator = tokenGenerator;
            _signInManager = signInManager;
            _configuration = configuration;
            _emailService = emailService;
        }

        public async Task<UserDTO> SignUp(UserRegisterDTO userDTO, string encodedUserIdAndToken)
        {
            UserDTO result = new();

            if (!userDTO.Password.Equals(userDTO.ConfirmPassword))
            {
                ApiExceptionHandler.ThrowApiException(HttpStatusCode.BadRequest, "Password and confirm password don't match.");
            }

            encodedUserIdAndToken = HttpUtility.UrlDecode(encodedUserIdAndToken);

            byte[] bytePassword = Convert.FromBase64String(userDTO.Password);
            string decodedPassword = Encoding.UTF8.GetString(bytePassword);

            try
            {
                string decodedUserIdAndToken = Encoding.Unicode.GetString(Convert.FromBase64String(encodedUserIdAndToken));

                var userIdAndTokenForEmailDTO = JsonConvert.DeserializeObject<UserIdAndTokenForEmailDTO>(decodedUserIdAndToken);

                User user = await _userRepository.GetById(userIdAndTokenForEmailDTO.UserId);

                if (await _userManager.IsEmailConfirmedAsync(user))
                {
                    ApiExceptionHandler.ThrowApiException(HttpStatusCode.BadRequest, "User is already signed up.");
                }

                _mapper.Map(userDTO, user);

                using (var scope = new TransactionScope(
                          TransactionScopeOption.Required,
                          new TransactionOptions { IsolationLevel = IsolationLevel.ReadCommitted },
                          TransactionScopeAsyncFlowOption.Enabled))
                    try
                    {
                        await ConfirmEmailAndAddPassword(decodedPassword, userIdAndTokenForEmailDTO, user);
                        scope.Complete();
                    }
                    catch (Exception ex)
                    {
                        ApiExceptionHandler.ThrowApiException(HttpStatusCode.BadRequest, ex.Message);
                    }


                result = await _userService.GetById(userIdAndTokenForEmailDTO.UserId);

                return result;
            }
            catch (Exception ex)
            {
                ApiExceptionHandler.ThrowApiException(HttpStatusCode.BadRequest, ex.Message);
            }

            return result;
        }

        private async Task ConfirmEmailAndAddPassword(string decodedPassword, UserIdAndTokenForEmailDTO userIdAndTokenDTO, User user)
        {
            (bool confirmEmailSuccess, string confirmEmailError) = await ConfirmEmail(user, userIdAndTokenDTO.Token);

            if (!confirmEmailSuccess && !string.IsNullOrEmpty(confirmEmailError) && confirmEmailError.Split(new char[] { ' ', '.' }, StringSplitOptions.RemoveEmptyEntries).Any(x => x == "expired"))
            {
                var token = await _tokenGenerator.GenerateEmailToken(user);

                _emailService.SendMail(new EmailDTO { Token = token, To = user.Email, Subject = "Register to Next Optimization" }, "RegistrationTemplate");
            }

            (bool addPasswordSuccess, string addPasswordError) = await AddPassword(user, decodedPassword);

            if (!addPasswordSuccess && !string.IsNullOrEmpty(addPasswordError))
            {
                ApiExceptionHandler.ThrowApiException(HttpStatusCode.BadRequest, addPasswordError);
            }
        }

        public async Task<bool> ForgotPassword(string email)
        {
            User user = await _userRepository.GetByEmail(email);

            ApiExceptionHandler.ObjectNotNull(user, $"User with email {email}.");

            var token = await _tokenGenerator.GeneratePasswordResetToken(user);

            var token2 = EncodeUrl(token, user);

            //await _emailService.SendMail(new List<string> { user.Email }, "Resetovanje šifre", _emailService.EncodeUrl(token, user));

            return true;
        }

        public string EncodeUrl(string token, User user)
        {
            UserIdAndTokenForEmailDTO userIdAndToken = new()
            {
                UserId = user.Id,
                Token = token
            };

            string text = JsonConvert.SerializeObject(userIdAndToken);
            byte[] encodedBytes = Encoding.Unicode.GetBytes(text);
            string encodedText = Convert.ToBase64String(encodedBytes);

            return encodedText;
        }

        public async Task<bool> ResetPassword(UserResetPasswordDTO userDTO, string encodedUserIdAndToken)
        {
            if (!userDTO.Password.Equals(userDTO.ConfirmPassword))
            {
                ApiExceptionHandler.ThrowApiException(HttpStatusCode.BadRequest, "Password and confirm password don't match.");
            }

            encodedUserIdAndToken = HttpUtility.UrlDecode(encodedUserIdAndToken);
            string decodedUserIdAndToken = Encoding.Unicode.GetString(Convert.FromBase64String(encodedUserIdAndToken));

            var userIdAndTokenDTO = JsonConvert.DeserializeObject<UserIdAndTokenForEmailDTO>(decodedUserIdAndToken);
            string decodedPassword = Encoding.UTF8.GetString(Convert.FromBase64String(userDTO.Password));

            Regex regex = new(@"^(?=.*[A-Z])(?=.*[0-9])[A-Za-z0-9!@#$%^&*]{6,}$");

            if (!regex.Match(decodedPassword).Success)
            {
                ApiExceptionHandler.ThrowApiException(HttpStatusCode.BadRequest, "Password must contain at least 6 characters, including at least one uppercase letter and at least one number.");
            }

            User user = await _userRepository.GetById(userIdAndTokenDTO.UserId);

            if (user == null || userIdAndTokenDTO.Token == null)
            {
                ApiExceptionHandler.ThrowApiException(HttpStatusCode.BadRequest, $@"User doesn't exist or token is not valid.");
            }

            return await ResetPassword(user, userIdAndTokenDTO.Token, decodedPassword);
        }

        public async Task<bool> ChangePassword(string id, UserChangePasswordDTO userDTO)
        {
            if (userDTO.CurrentPassword.Equals(userDTO.NewPassword))
            {
                ApiExceptionHandler.ThrowApiException(HttpStatusCode.BadRequest, "New password can't be the same as the current password.");
            }

            string decodedCurrentPassword = Encoding.UTF8.GetString(Convert.FromBase64String(userDTO.CurrentPassword));
            string decodedNewPassword = Encoding.UTF8.GetString(Convert.FromBase64String(userDTO.NewPassword));

            User user = await _userRepository.GetById(id);
            ApiExceptionHandler.ObjectNotNull(user, $"User with id {id}");

            Regex regex = new(@"^(?=.*[A-Z])(?=.*[0-9])[A-Za-z0-9!@#$%^&*]{6,}$");

            if (!regex.Match(decodedNewPassword).Success)
            {
                ApiExceptionHandler.ThrowApiException(HttpStatusCode.BadRequest, "Password must contain at least 6 characters, including at least one uppercase letter and at least one number.");
            }

            bool passwordChanged = await ChangePassword(user, decodedCurrentPassword, decodedNewPassword);

            if (!passwordChanged)
            {
                ApiExceptionHandler.ThrowApiException(HttpStatusCode.BadRequest, $@"Failed to change password.");
            }

            return passwordChanged;
        }

        public async Task<JWTResultsDTO> SignIn(UserLoginDTO userDTO)
        {
            var user = await _userRepository.GetByEmail(userDTO.Email);

            if (user == null)
            {
                ApiExceptionHandler.ThrowApiException(HttpStatusCode.BadRequest, "Email or password is incorrect.");
            }

            byte[] bytePassword = Convert.FromBase64String(userDTO.Password);
            string decodedPassword = Encoding.UTF8.GetString(bytePassword);

            user = await SignInWithPassword(user, decodedPassword);

            JWTCreateDTO userToken = new();
            _mapper.Map(user, userToken);
            userToken.RoleNames = (List<string>)await _userManager.GetRolesAsync(user);
            return _tokenGenerator.GenerateJWTToken(userToken, DateTime.UtcNow.AddDays(int.Parse(_configuration.GetSection("JWT:Duration").Value)));
        }

        public async Task SignOut()
        {
            await _signInManager.SignOutAsync();
        }

        public async Task<bool> ResetPassword(User user, string token, string newPassword)
        {
            return (await _userManager.ResetPasswordAsync(user, token, newPassword)).Succeeded;
        }

        private async Task<bool> ChangePassword(User user, string currentPassword, string newPassword)
        {
            return (await _userManager.ChangePasswordAsync(user, currentPassword, newPassword)).Succeeded;
        }

        public async Task<(bool, string)> ConfirmEmail(User user, string token)
        {
            var identityResult = await _userManager.ConfirmEmailAsync(user, token);

            if (!identityResult.Succeeded)
            {
                return (false, string.Join(", ", identityResult.Errors.Select(x => x.Description).ToList()));
            }

            return (true, "");
        }

        public async Task<(bool, string)> AddPassword(User user, string password)
        {
            var identityResultPassword = await _userManager.AddPasswordAsync(user, password);

            if (!identityResultPassword.Succeeded)
            {
                return (false, string.Join(", ", identityResultPassword.Errors.Select(x => x.Description).ToList()));
            }

            var identityResultUpdate = await _userManager.UpdateAsync(user);

            if (!identityResultUpdate.Succeeded)
            {
                return (false, string.Join(", ", identityResultPassword.Errors.Select(x => x.Description).ToList()));
            }

            return (true, "");
        }

        private async Task<User> SignInWithPassword(User user, string password)
        {
            if (await _userManager.IsEmailConfirmedAsync(user))
            {
                var login = await _signInManager.PasswordSignInAsync(user.Email, password, isPersistent: false, lockoutOnFailure: false);

                if (!login.Succeeded)
                {
                    ApiExceptionHandler.ThrowApiException(HttpStatusCode.BadRequest, "Email or password is incorrect.");
                }

                return user;
            }

            ApiExceptionHandler.ThrowApiException(HttpStatusCode.BadRequest, $"User with email {user.Email} is not registered.");

            return null;
        }
    }
}
