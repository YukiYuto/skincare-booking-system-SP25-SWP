using SkincareBookingSystem.Models.Dto.Appointment;
using SkincareBookingSystem.Models.Dto.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace SkincareBookingSystem.Services.IServices
{
    public interface IAppointmentService
    {
        Task<ResponseDto> CreateAppointment(ClaimsPrincipal user, CreateAppointmentDto appointmentDto);
        Task<ResponseDto> GetAllAppointments();
        Task<ResponseDto> GetAppointmentsByCustomer(ClaimsPrincipal User);
        Task<ResponseDto> GetAppointmentById(ClaimsPrincipal user, Guid appointmentId);
        Task<ResponseDto> UpdateAppointment(ClaimsPrincipal user, UpdateAppointmentDto appointmentDto);
        Task<ResponseDto> DeleteAppointment(ClaimsPrincipal user, Guid appointmentId);
    }
}
