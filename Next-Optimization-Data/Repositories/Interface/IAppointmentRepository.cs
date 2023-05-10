using NextOptimization.Data.Models;

namespace NextOptimization.Data.Repositories
{
    public interface IAppointmentRepository
    {
        Task<Appointment> Create(Appointment appointment);
        Task<bool> Delete(Appointment appointment);
        Task<List<Appointment>> GetAll();
        Task<List<Appointment>> GetAllCanceled();
        Task<List<Appointment>> GetAllCompleted();
        Task<List<Appointment>> GetAllPending();
        Task<Appointment> GetById(string id);
        Task<Appointment> Update(Appointment appointment);
        Task<List<Appointment>> GetAllByBuyer(string buyerId);
    }
}