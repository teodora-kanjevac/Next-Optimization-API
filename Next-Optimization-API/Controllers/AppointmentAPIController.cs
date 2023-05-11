using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NextOptimization.Business.DTOs;
using NextOptimization.Business.Services;

namespace NextOptimization.API.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class AppointmentAPIController : Controller
    {
        private readonly IAppointmentService _appointmentService;

        public AppointmentAPIController(IAppointmentService appointmentService)
        {
            _appointmentService = appointmentService;
        }

        [Authorize(Roles = "Admin")]
        [HttpGet]
        public async Task<IEnumerable<AppointmentDTO>> GetAll()
        {
            return await _appointmentService.GetAll();
        }

        [Authorize(Roles = "Admin")]
        [HttpGet("get-all-pending")]
        public async Task<IEnumerable<AppointmentDTO>> GetAllPending()
        {
            return await _appointmentService.GetAllPending();
        }

        [Authorize(Roles = "Admin")]
        [HttpGet("get-all-completed")]
        public async Task<IEnumerable<AppointmentDTO>> GetAllCompleted()
        {
            return await _appointmentService.GetAllCompleted();
        }

        [Authorize(Roles = "Admin")]
        [HttpGet("get-all-canceled")]
        public async Task<IEnumerable<AppointmentDTO>> GetAllCanceled()
        {
            return await _appointmentService.GetAllCanceled();
        }

        [HttpGet("id")]
        public async Task<AppointmentDTO> GetById(string id)
        {
            return await _appointmentService.GetById(id);
        }

        [HttpGet("all-by-buyer")]
        public async Task<IEnumerable<AppointmentDTO>> GetAllByBuyer(string id)
        {
            return await _appointmentService.GetAllByBuyer(id);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] AppointmentCreateDTO appointmentCreateDTO)
        {
            string username = User.Identity.Name;

            var result = await _appointmentService.Create(appointmentCreateDTO, username);

            return Ok(result);
        }

        [HttpPut]
        public async Task<IActionResult> Update(string id, [FromBody] AppointmentUpdateDTO appointmentUpdateDTO)
        {
            string username = User.Identity.Name;

            var result = await _appointmentService.Update(appointmentUpdateDTO, id, username);

            return Ok(result);
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete]
        public async Task<IActionResult> Delete(string id)
        {
            var result = await _appointmentService.Delete(id);

            return Ok(result);
        }
    }
}
