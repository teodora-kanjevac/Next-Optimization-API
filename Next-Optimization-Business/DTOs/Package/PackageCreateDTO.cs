using System.ComponentModel.DataAnnotations;

namespace NextOptimization.Business.DTOs
{
    public class PackageCreateDTO
    {
        [Required]
        public string Name { get; set; }
        public string? Description { get; set; }
        [Required]
        public int Price { get; set; }
    }
}