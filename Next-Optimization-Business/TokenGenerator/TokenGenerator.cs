using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using NextOptimization.Business.DTOs;
using NextOptimization.Business.Middleware;
using NextOptimization.Data.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Security.Claims;
using System.Text;

namespace NextOptimization.Business.TokenGenerator
{
    public class TokenGenerator : ITokenGenerator
    {
        private readonly UserManager<User> _userManager;
        private readonly IConfiguration _configuration;

        public TokenGenerator(UserManager<User> userManager, IConfiguration configuration)
        {
            _userManager = userManager;
            _configuration = configuration;
        }

        public async Task<string> GenerateEmailToken(User user)
        {
            var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);

            return token;
        }

        public async Task<string> GeneratePasswordResetToken(User user)
        {
            if (!await _userManager.IsEmailConfirmedAsync(user))
            {
                ApiExceptionHandler.ThrowApiException(HttpStatusCode.BadRequest, "User is not signed up.");
            }

            var token = await _userManager.GeneratePasswordResetTokenAsync(user);

            return token;
        }

        public JWTResultsDTO GenerateJWTToken(JWTCreateDTO user, DateTime expires)
        {
            var authClaims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.UserName),
                new Claim(JwtRegisteredClaimNames.Jti, new Guid().ToString()),
            };

            foreach (var userRole in user.RoleNames)
            {
                authClaims.Add(new Claim(ClaimTypes.Role, userRole));
            }

            var authSignInKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Secret"]));

            var token = new JwtSecurityToken(
                _configuration["JWT:Issuer"],
                _configuration["JWT:Audience"],
                authClaims,
                expires: expires,
                signingCredentials: new SigningCredentials(authSignInKey, SecurityAlgorithms.HmacSha256)
            );

            JWTResultsDTO result = new()
            {
                Token = "Bearer " + new JwtSecurityTokenHandler().WriteToken(token),
                Expires = token.ValidTo,
                UserId = user.Id,
                RoleNames = user.RoleNames
            };

            return result;
        }
    }
}
