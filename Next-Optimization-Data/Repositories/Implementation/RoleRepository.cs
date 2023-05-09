using Microsoft.AspNetCore.Identity;

namespace NextOptimization.Data.Repositories
{
    public class RoleRepository : IRoleRepository
    {
        private readonly RoleManager<IdentityRole> _roleManager;

        public RoleRepository(RoleManager<IdentityRole> roleManager)
        {
            _roleManager = roleManager;
        }

        public List<IdentityRole> GetAll()
        {
            return _roleManager.Roles.ToList();
        }

        public async Task<IdentityRole> GetById(string id)
        {
            return await _roleManager.FindByIdAsync(id);
        }

        public async Task<IdentityRole> GetByName(string name)
        {
            return await _roleManager.FindByNameAsync(name);
        }

        public async Task<IdentityRole> Create(IdentityRole role)
        {
            await _roleManager.CreateAsync(role);

            return role;
        }

        public async Task<IdentityRole> Update(IdentityRole role)
        {
            await _roleManager.UpdateAsync(role);

            return role;
        }

        public async Task<bool> Exists(string name)
        {
            return await _roleManager.RoleExistsAsync(name);
        }
    }
}
