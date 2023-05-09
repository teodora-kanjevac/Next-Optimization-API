using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using NextOptimization.Data.Models;

namespace NextOptimization.Data.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly UserManager<User> _userManager;

        public UserRepository(UserManager<User> userManager)
        {
            _userManager = userManager;
        }

        public async Task<List<User>> GetAll()
        {
            return await _userManager.Users.ToListAsync();
        }

        public async Task<User> GetById(string id)
        {
            return await _userManager.Users.FirstOrDefaultAsync(u => u.Id == id);
        }

        public async Task<User> GetByEmail(string email)
        {
            return await _userManager.Users.FirstOrDefaultAsync(u => u.Email == email);
        }

        public async Task<User> GetByUsername(string username)
        {
            return await _userManager.Users.FirstOrDefaultAsync(u => u.UserName == username);
        }

        public async Task<User> Create(User user)
        {
            await _userManager.CreateAsync(user);

            return user;
        }

        public async Task<User> Update(User user)
        {
            await _userManager.UpdateAsync(user);

            return user;
        }

        public async Task<bool> Delete(User user)
        {
            return (await _userManager.DeleteAsync(user)).Succeeded;
        }

        public async Task<bool> ConfirmEmail(User user, string token)
        {
            return (await _userManager.ConfirmEmailAsync(user, token)).Succeeded;
        }

        public async Task<bool> AddToRole(User user, string roleName)
        {
            return (await _userManager.AddToRoleAsync(user, roleName)).Succeeded;
        }

        public async Task<string> GetRoleName(User user)
        {
            var roles = await _userManager.GetRolesAsync(user);

            return roles.FirstOrDefault();
        }

        public async Task<bool> IsUserInRole(User user, string roleName)
        {
            return await _userManager.IsInRoleAsync(user, roleName);
        }
    }
}
