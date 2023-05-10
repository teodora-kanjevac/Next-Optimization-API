using NextOptimization.Business.DTOs;

namespace NextOptimization.Business.Services
{
    public interface IPackageService
    {
        Task<PackageDTO> Create(PackageCreateDTO packageCreateDTO);
        Task<bool> Delete(int id);
        Task<List<PackageDTO>> GetAll();
        Task<PackageDTO> GetById(int id);
        Task<PackageDTO> GetByName(string name);
        Task<PackageDTO> Update(PackageUpdateDTO packageUpdateDTO, int id);
    }
}