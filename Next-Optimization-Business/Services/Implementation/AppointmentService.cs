using AutoMapper;
using NextOptimization.Business.DTOs;
using NextOptimization.Business.Middleware;
using NextOptimization.Data.Models;
using NextOptimization.Data.Repositories;
using NextOptimization.Shared.Enums;
using System.Net;

namespace NextOptimization.Business.Services.Implementation
{
    public class AppointmentService : IAppointmentService
    {
        private readonly IAppointmentRepository _appointmentRepository;
        private readonly IPackageRepository _packageRepository;
        private readonly IUserRepository _userRepository;
        private readonly IUserService _userService;
        private readonly IMapper _mapper;

        public AppointmentService(IAppointmentRepository appointmentRepository, IUserRepository userRepository, IUserService userService, IMapper mapper, IPackageRepository packageRepository)
        {
            _appointmentRepository = appointmentRepository;
            _userRepository = userRepository;
            _userService = userService;
            _mapper = mapper;
            _packageRepository = packageRepository;
        }

        public async Task<List<AppointmentDTO>> GetAll()
        {
            var appointments = await _appointmentRepository.GetAll();

            return await MapAppointmentsWithBuyersAndPackages(appointments);
        }

        public async Task<List<AppointmentDTO>> GetAllPending()
        {
            var appointments = await _appointmentRepository.GetAllPending();

            return await MapAppointmentsWithBuyersAndPackages(appointments);
        }

        public async Task<List<AppointmentDTO>> GetAllCompleted()
        {
            var appointments = await _appointmentRepository.GetAllCompleted();

            return await MapAppointmentsWithBuyersAndPackages(appointments);
        }

        public async Task<List<AppointmentDTO>> GetAllCanceled()
        {
            var appointments = await _appointmentRepository.GetAllCanceled();

            return await MapAppointmentsWithBuyersAndPackages(appointments);
        }

        public async Task<List<AppointmentDTO>> GetAllByBuyer(string buyerId)
        {
            var appointments = await _appointmentRepository.GetAllByBuyer(buyerId);

            return await MapAppointmentsWithBuyersAndPackages(appointments);
        }

        public async Task<AppointmentDTO> GetById(string id)
        {
            Appointment appointment = await _appointmentRepository.GetById(id);

            ApiExceptionHandler.ObjectNotNull(appointment, $"Appointment with id '{id}'");

            return await MapAppointmentWithBuyerAndPackage(appointment);
        }

        public async Task<AppointmentDTO> Create(AppointmentCreateDTO appointmentCreateDTO, string username)
        {
            UserDTO userDTO = await _userService.GetByUsername(username);

            Appointment appointment = _mapper.Map<Appointment>(appointmentCreateDTO);

            Package package = await _packageRepository.GetById(appointmentCreateDTO.PackageId);

            ApiExceptionHandler.ObjectNotNull(package, $"Package with id '{appointmentCreateDTO.PackageId}'");

            appointment.BuyerId = userDTO.Id;
            appointment.Status = Status.Pending.ToString();
            appointment.PurchaseDate = DateTime.Now;
            appointment.EndDate = appointment.StartDate.AddHours(2);
            appointment.Package = package;

            await _appointmentRepository.Create(appointment);

            return await MapAppointmentWithBuyerAndPackage(appointment);
        }

        public async Task<AppointmentDTO> Update(AppointmentUpdateDTO appointmentUpdateDTO, string id, string username)
        {
            UserDTO userDTO = await _userService.GetByUsername(username);

            Appointment appointment = await _appointmentRepository.GetById(id);

            ApiExceptionHandler.ObjectNotNull(appointment, $"Appointment with {id}");

            if (userDTO.Id != appointment.BuyerId)
            {
                ApiExceptionHandler.ThrowApiException(HttpStatusCode.BadRequest, "You can't update this appointment.");
            }

            appointment.EndDate = appointmentUpdateDTO.StartDate.AddHours(2);
            appointment.Package = await _packageRepository.GetById(appointmentUpdateDTO.PackageId);

            appointment = _mapper.Map(appointmentUpdateDTO, appointment);

            await _appointmentRepository.Update(appointment);

            return await MapAppointmentWithBuyerAndPackage(appointment);
        }

        public async Task<bool> Delete(string id)
        {
            Appointment appointment = await _appointmentRepository.GetById(id);

            ApiExceptionHandler.ObjectNotNull(appointment, $"Appointment with {id}");

            return await _appointmentRepository.Delete(appointment);
        }

        private async Task<AppointmentDTO> MapAppointmentWithBuyerAndPackage(Appointment appointment)
        {
            AppointmentDTO appointmentDTO = _mapper.Map<AppointmentDTO>(appointment);

            User buyer = await _userRepository.GetById(appointment.BuyerId);
            appointmentDTO.Buyer = _mapper.Map<UserDTO>(buyer);

            Package package = await _packageRepository.GetById(appointment.PackageId);
            appointmentDTO.Package = _mapper.Map<PackageDTO>(package);

            return appointmentDTO;
        }

        private async Task<List<AppointmentDTO>> MapAppointmentsWithBuyersAndPackages(List<Appointment> appointments)
        {
            List<AppointmentDTO> appointmentDTO = new();

            foreach (var appointment in appointments)
            {
                appointmentDTO.Add(await MapAppointmentWithBuyerAndPackage(appointment));
            }

            return appointmentDTO;
        }
    }
}
