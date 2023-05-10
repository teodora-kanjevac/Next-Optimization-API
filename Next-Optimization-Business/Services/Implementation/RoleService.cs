using AutoMapper;
using Microsoft.AspNetCore.Identity;
using NextOptimization.Business.DTOs;
using NextOptimization.Business.Middleware;
using NextOptimization.Data.Repositories;
using System.Net;

namespace NextOptimization.Business.Services
{
    public class RoleService : IRoleService
    {
        private readonly IRoleRepository _roleRepository;
        private readonly IMapper _mapper;

        public RoleService(IRoleRepository roleRepository, IMapper mapper)
        {
            _roleRepository = roleRepository;
            _mapper = mapper;
        }

        public List<RoleDTO> GetAll()
        {
            var roles = _roleRepository.GetAll();

            var rolesDTO = _mapper.Map<List<RoleDTO>>(roles);

            return rolesDTO;
        }

        public async Task<RoleDTO> GetById(string id)
        {
            var role = await _roleRepository.GetById(id);

            ApiExceptionHandler.ObjectNotNull(role, $"Role with the id '{id}'");

            var roleDTO = _mapper.Map<RoleDTO>(role);

            return roleDTO;
        }

        public async Task<RoleDTO> GetByName(string name)
        {
            var role = await _roleRepository.GetByName(name);

            ApiExceptionHandler.ObjectNotNull(role, $"Role with name '{name}'");

            var roleDTO = _mapper.Map<RoleDTO>(role);

            return roleDTO;
        }

        public async Task<RoleDTO> Create(RoleCreateDTO roleCreateDTO)
        {
            if (await _roleRepository.Exists(roleCreateDTO.Name))
            {
                ApiExceptionHandler.ThrowApiException(HttpStatusCode.BadRequest, $"Role name '{roleCreateDTO.Name}' already exists.");
            }

            return _mapper.Map<RoleDTO>(await _roleRepository.Create(_mapper.Map<IdentityRole>(roleCreateDTO)));
        }

        public async Task<RoleDTO> Update(string id, RoleUpdateDTO roleUpdateDTO)
        {
            var role = await _roleRepository.GetById(id);

            ApiExceptionHandler.ObjectNotNull(role, $"Role with the id '{id}'");

            role = _mapper.Map(roleUpdateDTO, role);

            await _roleRepository.Update(role);

            return _mapper.Map<RoleDTO>(role);
        }
    }
}
