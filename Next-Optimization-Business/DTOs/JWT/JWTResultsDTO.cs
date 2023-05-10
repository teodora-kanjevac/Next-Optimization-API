namespace NextOptimization.Business.DTOs
{
    public class JWTResultsDTO
    {
        public string Token { get; set; }
        public DateTime Expires { get; set; }
        public string UserId { get; set; }
        public List<string> RoleNames { get; set; }
    }
}