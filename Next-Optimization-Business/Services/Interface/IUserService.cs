using NextOptimization.Business.DTOs;

namespace NextOptimization.Business.Services
{
    public interface IUserService
    {
        Task<UserDTO> Create(UserCreateDTO userCreateDTO);
        Task<bool> Delete(string id);
        Task<List<UserDTO>> GetAll();
        Task<UserDTO> GetByEmail(string email);
        Task<UserDTO> GetById(string id);
        Task<UserDTO> GetByUsername(string username);
        Task<UserDTO> Update(string username, string id, UserUpdateDTO userUpdateDTO);
    }
}