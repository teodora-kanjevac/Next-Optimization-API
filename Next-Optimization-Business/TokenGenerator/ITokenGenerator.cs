using NextOptimization.Business.DTOs;
using NextOptimization.Data.Models;

namespace NextOptimization.Business.TokenGenerator
{
    public interface ITokenGenerator
    {
        Task<string> GenerateEmailToken(User user);
        JWTResultsDTO GenerateJWTToken(JWTCreateDTO user, DateTime expires);
        Task<string> GeneratePasswordResetToken(User user);
    }
}