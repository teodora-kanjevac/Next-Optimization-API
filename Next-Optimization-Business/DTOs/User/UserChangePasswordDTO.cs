using System.ComponentModel.DataAnnotations;

namespace NextOptimization.Business.DTOs
{
    public class UserChangePasswordDTO
    {
        [Required]
        public string CurrentPassword { get; set; }

        [Required]
        public string NewPassword { get; set; }
    }
}