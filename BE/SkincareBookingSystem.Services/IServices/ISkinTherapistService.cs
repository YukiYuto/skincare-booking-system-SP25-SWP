using SkincareBookingSystem.Models.Dto.Booking.ServiceType;
using SkincareBookingSystem.Models.Dto.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace SkincareBookingSystem.Services.IServices
{
    public interface ISkinTherapistService
    {
        Task<ResponseDto> GetTherapistDetailsById(Guid therapistId);
        Task<ResponseDto> GetTherapistsByServiceTypeId(Guid serviceTypeId);
        Task<ResponseDto> GetTherapistsByServiceId(Guid serviceId);
        Task<ResponseDto> GetAllTherapists();
        Task<ResponseDto> GetTherapistTodayAppointments(ClaimsPrincipal User);
    }
}
