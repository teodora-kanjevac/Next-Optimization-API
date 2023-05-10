namespace NextOptimization.Business.DTOs
{
    public class UserDTO
    {
        public string Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string FullName => $"{FirstName} {LastName}";
        public string? PhoneNumber { get; set; }
        public string? Country { get; set; }
        public string Email { get; set; }
    }
}