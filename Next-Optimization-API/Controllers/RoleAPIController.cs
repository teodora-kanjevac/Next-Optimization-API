using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NextOptimization.Business.DTOs;
using NextOptimization.Business.Services;

namespace NextOptimization.API.Controllers
{
    [Authorize(Roles = "Admin")]
    [Route("api/[controller]")]
    [ApiController]
    public class RoleAPIController : ControllerBase
    {
        private readonly IRoleService _roleService;

        public RoleAPIController(IRoleService roleService)
        {
            _roleService = roleService;
        }

        [HttpGet]
        public IEnumerable<RoleDTO> GetAll()
        {
            return _roleService.GetAll();
        }

        [HttpGet("id")]
        public async Task<RoleDTO> GetById(string id)
        {
            return await _roleService.GetById(id);
        }

        [HttpGet("name")]
        public async Task<RoleDTO> GetByName(string name)
        {
            return await _roleService.GetByName(name);
        }

        [HttpPost]
        public async Task<IActionResult> Create(RoleCreateDTO roleCreateDTO)
        {
            var result = await _roleService.Create(roleCreateDTO);

            return Ok(result);
        }

        [HttpPut("id")]
        public async Task<IActionResult> Update(string id, [FromBody] RoleUpdateDTO roleUpdateDTO)
        {
            var result = await _roleService.Update(id, roleUpdateDTO);

            return Ok(result);
        }
    }
}
