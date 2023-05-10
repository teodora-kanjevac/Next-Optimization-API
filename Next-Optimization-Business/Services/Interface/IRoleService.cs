using NextOptimization.Business.DTOs;

namespace NextOptimization.Business.Services
{
    public interface IRoleService
    {
        Task<RoleDTO> Create(RoleCreateDTO roleCreateDTO);
        List<RoleDTO> GetAll();
        Task<RoleDTO> GetById(string id);
        Task<RoleDTO> GetByName(string name);
        Task<RoleDTO> Update(string id, RoleUpdateDTO roleUpdateDTO);
    }
}