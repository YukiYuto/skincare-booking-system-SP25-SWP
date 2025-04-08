using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using SkincareBookingSystem.DataAccess.IRepositories;
using SkincareBookingSystem.Models.Domain;
using SkincareBookingSystem.Models.Dto.Response;
using SkincareBookingSystem.Models.Dto.SkinTherapist;
using SkincareBookingSystem.Services.IServices;

namespace SkincareBookingSystem.Services.Services;

public class TherapistAdviceService : ITherapistAdviceService
{
    private readonly IAutoMapperService _autoMapperService;
    private readonly IUnitOfWork _unitOfWork;

    public TherapistAdviceService(IAutoMapperService autoMapperService, IUnitOfWork unitOfWork)
    {
        _autoMapperService = autoMapperService;
        _unitOfWork = unitOfWork;
    }

    public async Task<ResponseDto> CreateTherapistAdvice(ClaimsPrincipal User,
        CreateTherapistAdviceDto createTherapistAdviceDto)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        var skinTherapist = await _unitOfWork.SkinTherapist.GetAsync(st => st.UserId == userId);
        if (skinTherapist == null)
        {
            return new ResponseDto
            {
                StatusCode = StatusCodes.Status404NotFound,
                Message = "Skin therapist not found",
                IsSuccess = true,
                Result = null
            };
        }

        var therapistSchedules = await _unitOfWork.TherapistSchedule.GetAsync(ts =>
                ts.TherapistScheduleId == createTherapistAdviceDto.TherapistScheduleId &&
                ts.TherapistId == skinTherapist.SkinTherapistId &&
                ts.Appointment.CustomerId == createTherapistAdviceDto.CustomerId,
            "Appointment");

        if (therapistSchedules == null)
        {
            return new ResponseDto
            {
                StatusCode = StatusCodes.Status404NotFound,
                Message = "Therapist schedule not found",
                IsSuccess = true,
                Result = null
            };
        }
        
        var therapistAdvice = _autoMapperService.Map<CreateTherapistAdviceDto, TherapistAdvice>(createTherapistAdviceDto);
        therapistAdvice.CreatedBy = User.FindFirstValue("FullName");
        
        await _unitOfWork.TherapistAdvice.AddAsync(therapistAdvice);
        await _unitOfWork.SaveAsync();
        
        var therapistAdviceDto = _autoMapperService.Map<TherapistAdvice, TherapistAdviceDto>(therapistAdvice);

        return new ResponseDto()
        {
            StatusCode = StatusCodes.Status201Created,
            Message = "Therapist advice created successfully",
            IsSuccess = true,
            Result = therapistAdviceDto
        };
    }

public async Task<ResponseDto> GetTherapistAdviceByAppointment(ClaimsPrincipal User, Guid appointmentId)
{
    var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
    if (userId == null)
    {
        return new ResponseDto
        {
            StatusCode = StatusCodes.Status401Unauthorized,
            Message = "User not found",
            IsSuccess = false,
            Result = null
        };
    }
    
    var appointment = await _unitOfWork.Appointments.GetAsync(
        a => a.AppointmentId == appointmentId,
        "TherapistSchedules,Customer"
    );

    if (appointment == null)
    {
        return new ResponseDto
        {
            StatusCode = StatusCodes.Status404NotFound,
            Message = "Appointment not found",
            IsSuccess = false,
            Result = null
        };
    }
    
    var therapistScheduleIds = appointment.TherapistSchedules.Select(ts => ts.TherapistScheduleId).ToList();
    if (!therapistScheduleIds.Any())
    {
        return new ResponseDto
        {
            StatusCode = StatusCodes.Status404NotFound,
            Message = "No therapist schedules found for this appointment",
            IsSuccess = false,
            Result = null
        };
    }
    
    var therapistAdvices = await _unitOfWork.TherapistAdvice.GetAllAsync(
        ta => therapistScheduleIds.Contains(ta.TherapistScheduleId) && 
              ta.CustomerId == appointment.CustomerId 
    );
    
    if (!therapistAdvices.Any())
    {
        return new ResponseDto
        {
            StatusCode = StatusCodes.Status404NotFound,
            Message = "No therapist advice found for this appointment",
            IsSuccess = false,
            Result = null
        };
    }
    
    var therapistAdviceDtos = _autoMapperService.Map<IEnumerable<TherapistAdvice>, IEnumerable<TherapistAdviceDto>>(therapistAdvices);

    return new ResponseDto
    {
        StatusCode = StatusCodes.Status200OK,
        Message = "Therapist advice retrieved successfully",
        IsSuccess = true,
        Result = therapistAdviceDtos
    };
}
}