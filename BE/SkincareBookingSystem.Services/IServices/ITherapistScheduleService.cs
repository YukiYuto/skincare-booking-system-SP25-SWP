using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SkincareBookingSystem.Models.Dto.Response;
using SkincareBookingSystem.Models.Dto.BookingSchedule;
using System.Security.Claims;

namespace SkincareBookingSystem.Services.IServices
{
    public interface ITherapistScheduleService
    {
        Task<ResponseDto> CreateTherapistSchedule(ClaimsPrincipal User, CreateTherapistScheduleDto bookingScheduleDto);
        Task<ResponseDto> GetTherapistScheduleById(Guid id);
        Task<ResponseDto> GetAllTherapistSchedules();
        Task<ResponseDto> UpdateTherapistSchedule(ClaimsPrincipal User, UpdateTherapistScheduleDto bookingScheduleDto);
        Task<ResponseDto> DeleteTherapistSchedule(ClaimsPrincipal User, Guid id);
    }
}
