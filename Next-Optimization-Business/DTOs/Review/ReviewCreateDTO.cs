using System.ComponentModel.DataAnnotations;

namespace NextOptimization.Business.DTOs
{
    public class ReviewCreateDTO
    {
        [Required]
        public int Rating { get; set; }
        public string? Comment { get; set; }
    }
}