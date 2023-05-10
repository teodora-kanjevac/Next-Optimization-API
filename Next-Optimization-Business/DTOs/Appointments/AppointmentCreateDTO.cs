using System.ComponentModel.DataAnnotations;

namespace NextOptimization.Business.DTOs
{
    public class AppointmentCreateDTO
    {
        [Required]
        public DateTime StartDate { get; set; }
        [Required]
        public int PackageId { get; set; }
    }
}