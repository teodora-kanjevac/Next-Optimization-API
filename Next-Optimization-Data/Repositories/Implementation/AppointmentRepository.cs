using Microsoft.EntityFrameworkCore;
using NextOptimization.Data.Models;
using NextOptimization.Shared.Enums;

namespace NextOptimization.Data.Repositories
{
    public class AppointmentRepository : IAppointmentRepository
    {
        private readonly NextOptimizationContext _nextOptimizationContext;

        public AppointmentRepository(NextOptimizationContext nextOptimizationContext)
        {
            _nextOptimizationContext = nextOptimizationContext;
        }

        public async Task<List<Appointment>> GetAll()
        {
            return await _nextOptimizationContext.Appointments.ToListAsync();
        }

        public async Task<List<Appointment>> GetAllPending()
        {
            return await _nextOptimizationContext.Appointments.Where(a => a.Status == Status.Pending.ToString()).ToListAsync();
        }

        public async Task<List<Appointment>> GetAllCompleted()
        {
            return await _nextOptimizationContext.Appointments.Where(a => a.Status == Status.Completed.ToString()).ToListAsync();
        }

        public async Task<List<Appointment>> GetAllCanceled()
        {
            return await _nextOptimizationContext.Appointments.Where(a => a.Status == Status.Canceled.ToString()).ToListAsync();
        }

        public async Task<Appointment> GetById(string id)
        {
            return await _nextOptimizationContext.Appointments.FirstOrDefaultAsync(a => a.Id == id);
        }

        public async Task<List<Appointment>> GetAllByBuyer(string buyerId)
        {
            return await _nextOptimizationContext.Appointments.Where(a => a.BuyerId == buyerId).ToListAsync();
        }

        public async Task<Appointment> Create(Appointment appointment)
        {
            await _nextOptimizationContext.Appointments.AddAsync(appointment);

            await _nextOptimizationContext.SaveChangesAsync();

            return appointment;
        }

        public async Task<Appointment> Update(Appointment appointment)
        {
            _nextOptimizationContext.Appointments.Update(appointment);

            await _nextOptimizationContext.SaveChangesAsync();

            return appointment;
        }

        public async Task<bool> Delete(Appointment appointment)
        {
            _nextOptimizationContext.Appointments.Remove(appointment);

            return await _nextOptimizationContext.SaveChangesAsync() > 0;
        }
    }
}
