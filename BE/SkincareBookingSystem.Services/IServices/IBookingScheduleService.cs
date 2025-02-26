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
    public interface IBookingScheduleService
    {
        Task<ResponseDto> CreateBookingSchedule(ClaimsPrincipal User, CreateBookingScheduleDto bookingScheduleDto);
        Task<ResponseDto> GetBookingScheduleById(Guid id);
        Task<ResponseDto> GetAllBookingSchedules();
        Task<ResponseDto> UpdateBookingSchedule(ClaimsPrincipal User, UpdateBookingScheduleDto bookingScheduleDto);
        Task<ResponseDto> DeleteBookingSchedule(Guid id);
    }
}
