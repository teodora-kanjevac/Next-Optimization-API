namespace NextOptimization.Data.Models.SoftDelete
{
    public class SoftDelete : ISoftDelete
    {
        public bool IsDeleted { get; set; } = false;
    }
}
