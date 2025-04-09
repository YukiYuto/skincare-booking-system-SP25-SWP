using System.Security.Claims;
using SkincareBookingSystem.Models.Dto.Response;
using SkincareBookingSystem.Models.Dto.SkinTherapist;

namespace SkincareBookingSystem.Services.IServices;

public interface ITherapistAdviceService
{
    Task<ResponseDto> CreateTherapistAdvice(ClaimsPrincipal User, CreateTherapistAdviceDto createTherapistAdviceDto);
    Task<ResponseDto> GetTherapistAdviceByAppointment(ClaimsPrincipal User, Guid appointmentId);
}