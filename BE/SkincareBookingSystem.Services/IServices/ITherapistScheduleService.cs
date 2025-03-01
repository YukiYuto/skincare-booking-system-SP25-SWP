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
        Task<ResponseDto> CreateBookingSchedule(ClaimsPrincipal User, CreateTherapistScheduleDto bookingScheduleDto);
        Task<ResponseDto> GetBookingScheduleById(Guid id);
        Task<ResponseDto> GetAllBookingSchedules();
        Task<ResponseDto> UpdateBookingSchedule(ClaimsPrincipal User, UpdateTherapistScheduleDto bookingScheduleDto);
        Task<ResponseDto> DeleteBookingSchedule(Guid id);
    }
}
