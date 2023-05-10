namespace NextOptimization.Business.DTOs
{
    public class ReviewDTO
    {
        public string Id { get; set; }
        public UserDTO Reviewer { get; set; }
        public int Rating { get; set; }
        public string? Comment { get; set; }
    }
}