namespace NextOptimization.Data.Models
{
    public class Review
    {
        public string Id { get; set; }
        public string ReviewerId { get; set; }
        public User Reviewer { get; set; }
        public int Rating { get; set; }
        public string? Comment { get; set; }
    }
}
