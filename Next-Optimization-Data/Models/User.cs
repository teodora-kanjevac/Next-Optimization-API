using Microsoft.AspNetCore.Identity;
using NextOptimization.Data.Models.SoftDelete;

namespace NextOptimization.Data.Models
{
    public class User : IdentityUser, ISoftDelete
    {
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string FullName => $"{FirstName} {LastName}";
        public string? PhoneNumber { get; set; }
        public string? Country { get; set; }
        public List<Appointment>? AppointmentHistory { get; set; }
        public List<Review>? Reviews { get; set; }
        public bool IsDeleted { get; set; } = false;
    }
}
