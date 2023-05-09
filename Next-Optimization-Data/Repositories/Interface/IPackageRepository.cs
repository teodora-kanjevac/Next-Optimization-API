using NextOptimization.Data.Models;

namespace NextOptimization.Data.Repositories
{
    public interface IPackageRepository
    {
        Task<Package> Create(Package package);
        Task<bool> Delete(Package package);
        Task<List<Package>> GetAll();
        Task<Package> GetById(int id);
        Task<Package> GetByName(string name);
        Task<Package> Update(Package package);
    }
}