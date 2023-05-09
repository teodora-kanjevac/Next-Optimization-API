using Microsoft.EntityFrameworkCore;
using NextOptimization.Data.Models;

namespace NextOptimization.Data.Repositories
{
    public class PackageRepository : IPackageRepository
    {
        private readonly NextOptimizationContext _nextOptimizationContext;

        public PackageRepository(NextOptimizationContext nextOptimizationContext)
        {
            _nextOptimizationContext = nextOptimizationContext;
        }

        public async Task<List<Package>> GetAll()
        {
            return await _nextOptimizationContext.Packages.ToListAsync();
        }

        public async Task<Package> GetById(int id)
        {
            return await _nextOptimizationContext.Packages.FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task<Package> GetByName(string name)
        {
            return await _nextOptimizationContext.Packages.FirstOrDefaultAsync(p => p.Name == name);
        }

        public async Task<Package> Create(Package package)
        {
            await _nextOptimizationContext.Packages.AddAsync(package);

            await _nextOptimizationContext.SaveChangesAsync();

            return package;
        }

        public async Task<Package> Update(Package package)
        {
            _nextOptimizationContext.Packages.Update(package);

            await _nextOptimizationContext.SaveChangesAsync();

            return package;
        }

        public async Task<bool> Delete(Package package)
        {
            _nextOptimizationContext.Packages.Remove(package);

            return await _nextOptimizationContext.SaveChangesAsync() > 0;
        }
    }
}
