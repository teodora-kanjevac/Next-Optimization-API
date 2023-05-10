namespace NextOptimization.Business.DTOs
{
    public class JWTCreateDTO
    {
        public string Id { get; set; }
        public string UserName { get; set; }
        public List<string> RoleNames { get; set; }
    }
}