using NextOptimization.Business.DTOs;

namespace NextOptimization.Business.Services
{
    public interface IAppointmentService
    {
        Task<AppointmentDTO> Create(AppointmentCreateDTO appointmentCreateDTO, string username);
        Task<bool> Delete(string id);
        Task<List<AppointmentDTO>> GetAll();
        Task<List<AppointmentDTO>> GetAllByBuyer(string buyerId);
        Task<List<AppointmentDTO>> GetAllCanceled();
        Task<List<AppointmentDTO>> GetAllCompleted();
        Task<List<AppointmentDTO>> GetAllPending();
        Task<AppointmentDTO> GetById(string id);
        Task<AppointmentDTO> Update(AppointmentUpdateDTO appointmentUpdateDTO, string id, string username);
    }
}