using System.ComponentModel.DataAnnotations;

namespace NextOptimization.Business.DTOs
{
    public class UserCreateDTO
    {
        [Required]
        public string FirstName { get; set; }
        [Required] 
        public string LastName { get; set;}
        [Required]
        public string Email { get; set;}
        public string PhoneNumber { get; set; }
        public string Country { get; set; }
    }
}