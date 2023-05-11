using AutoMapper;
using NextOptimization.Business.DTOs;
using NextOptimization.Business.Middleware;
using NextOptimization.Data.Models;
using NextOptimization.Data.Repositories;

namespace NextOptimization.Business.Services
{
    public class PackageService : IPackageService
    {
        private readonly IPackageRepository _packageRepository;
        private readonly IMapper _mapper;

        public PackageService(IPackageRepository packageRepository, IMapper mapper)
        {
            _packageRepository = packageRepository;
            _mapper = mapper;
        }

        public async Task<List<PackageDTO>> GetAll()
        {
            var packages = await _packageRepository.GetAll();

            return _mapper.Map<List<PackageDTO>>(packages);
        }

        public async Task<PackageDTO> GetById(int id)
        {
            Package package = await _packageRepository.GetById(id);

            ApiExceptionHandler.ObjectNotNull(package, $"Package with id '{id}'");

            return _mapper.Map<PackageDTO>(package);
        }

        public async Task<PackageDTO> GetByName(string name)
        {
            Package package = await _packageRepository.GetByName(name);

            ApiExceptionHandler.ObjectNotNull(package, $"Package with name '{name}'");

            return _mapper.Map<PackageDTO>(package);
        }

        public async Task<PackageDTO> Create(PackageCreateDTO packageCreateDTO)
        {
            Package package = _mapper.Map<Package>(packageCreateDTO);

            await _packageRepository.Create(package);

            return _mapper.Map<PackageDTO>(package);
        }

        public async Task<PackageDTO> Update(PackageUpdateDTO packageUpdateDTO, int id)
        {
            Package package = await _packageRepository.GetById(id);

            ApiExceptionHandler.ObjectNotNull(package, $"Package with id '{id}'");

            package = _mapper.Map(packageUpdateDTO, package);

            await _packageRepository.Update(package);

            return _mapper.Map<PackageDTO>(package);
        }

        public async Task<bool> Delete(int id)
        {
            Package package = await _packageRepository.GetById(id);

            ApiExceptionHandler.ObjectNotNull(package, $"Package with id '{id}'");

            return await _packageRepository.Delete(package);
        }
    }
}
