using System.ComponentModel.DataAnnotations;

namespace NextOptimization.Business.DTOs
{
    public class RoleCreateDTO
    {
        [Required]
        public string Name { get; set; }
    }
}