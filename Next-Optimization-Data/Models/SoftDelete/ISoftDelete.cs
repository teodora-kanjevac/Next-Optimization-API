namespace NextOptimization.Data.Models.SoftDelete
{
    public interface ISoftDelete
    {
        bool IsDeleted { get; set; }
    }
}