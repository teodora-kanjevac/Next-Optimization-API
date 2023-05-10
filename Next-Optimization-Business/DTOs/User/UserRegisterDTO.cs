using System.ComponentModel.DataAnnotations;

namespace NextOptimization.Business.DTOs
{
    public class UserRegisterDTO
    {
        [Required]
        public string Password { get; set; }

        [Required]
        public string ConfirmPassword { get; set; }
    }
}