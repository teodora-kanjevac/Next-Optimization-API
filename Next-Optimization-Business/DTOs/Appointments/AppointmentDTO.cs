using NextOptimization.Data.Models;

namespace NextOptimization.Business.DTOs
{
    public class AppointmentDTO
    {
        public string Id { get; set; }
        public UserDTO Buyer { get; set; }
        public DateTime PurchaseDate { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public PackageDTO Package { get; set; }
        public string Status { get; set; }
    }
}