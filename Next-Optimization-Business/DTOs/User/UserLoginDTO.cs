using System.ComponentModel.DataAnnotations;

namespace NextOptimization.Business.DTOs
{
    public class UserLoginDTO
    {
        [EmailAddress]
        public string Email { get; set; }

        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}