using Microsoft.AspNetCore.Identity;

namespace NextOptimization.Data.Repositories
{
    public interface IRoleRepository
    {
        Task<IdentityRole> Create(IdentityRole role);
        Task<bool> Exists(string name);
        List<IdentityRole> GetAll();
        Task<IdentityRole> GetById(string id);
        Task<IdentityRole> GetByName(string name);
        Task<IdentityRole> Update(IdentityRole role);
    }
}