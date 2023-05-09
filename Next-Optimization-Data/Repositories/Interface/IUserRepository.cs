using NextOptimization.Data.Models;

namespace NextOptimization.Data.Repositories
{
    public interface IUserRepository
    {
        Task<bool> AddToRole(User user, string roleName);
        Task<bool> ConfirmEmail(User user, string token);
        Task<User> Create(User user);
        Task<bool> Delete(User user);
        Task<List<User>> GetAll();
        Task<User> GetByEmail(string email);
        Task<User> GetById(string id);
        Task<User> GetByUsername(string username);
        Task<string> GetRoleName(User user);
        Task<bool> IsUserInRole(User user, string roleName);
        Task<User> Update(User user);
    }
}