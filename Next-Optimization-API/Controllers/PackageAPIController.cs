using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NextOptimization.Business.DTOs;
using NextOptimization.Business.Services;

namespace NextOptimization.API.Controllers
{
    [Authorize(Roles = "Admin")]
    [Route("api/[controller]")]
    [ApiController]
    public class PackageAPIController : Controller
    {
        private readonly IPackageService _packageService;

        public PackageAPIController(IPackageService packageService)
        {
            _packageService = packageService;
        }

        [HttpGet]
        public async Task<IEnumerable<PackageDTO>> GetAll()
        {
            return await _packageService.GetAll();
        }

        [HttpGet("id")]
        public async Task<PackageDTO> GetById(int id)
        {
            return await _packageService.GetById(id);
        }

        [HttpGet("name")]
        public async Task<PackageDTO> GetByName(string name)
        {
            return await _packageService.GetByName(name);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] PackageCreateDTO packageCreateDTO)
        {
            var result = await _packageService.Create(packageCreateDTO);

            return Ok(result);
        }

        [HttpPut]
        public async Task<IActionResult> Update(int id, [FromBody] PackageUpdateDTO packageUpdateDTO)
        {
            var result = await _packageService.Update(packageUpdateDTO, id);

            return Ok(result);
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _packageService.Delete(id);

            return Ok(result);
        }
    }
}
