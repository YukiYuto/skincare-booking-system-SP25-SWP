using SkincareBookingSystem.DataAccess.IRepositories;
using SkincareBookingSystem.Models.Domain;
using SkincareBookingSystem.Models.Dto.Booking.Appointment.FinalizeAppointment;
using SkincareBookingSystem.Models.Dto.Booking.ServiceType;
using SkincareBookingSystem.Models.Dto.Response;
using SkincareBookingSystem.Models.Dto.SkinTherapist;
using SkincareBookingSystem.Models.Dto.Staff;
using SkincareBookingSystem.Services.Helpers.Responses;
using SkincareBookingSystem.Services.Helpers.Users;
using SkincareBookingSystem.Services.IServices;
using SkincareBookingSystem.Utilities.Constants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace SkincareBookingSystem.Services.Services
{
    public class SkinTherapistService : ISkinTherapistService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IAutoMapperService _autoMapperService;

        public SkinTherapistService(IUnitOfWork unitOfWork, IAutoMapperService autoMapperService)
        {
            _unitOfWork = unitOfWork;
            _autoMapperService = autoMapperService;
        }

        public async Task<ResponseDto> GetAllTherapists()
        {
            var therapistsFromDb = await _unitOfWork.SkinTherapist.GetAllAsync(
                includeProperties: nameof(ApplicationUser));

            var therapistListDto = _autoMapperService.MapCollection<SkinTherapist, GetSkinTherapistDto>(therapistsFromDb);

            return (therapistsFromDb.Any()) ?
                SuccessResponse.Build(
                message: StaticOperationStatus.SkinTherapist.Found,
                statusCode: StaticOperationStatus.StatusCode.Ok,
                result: therapistListDto) :
                ErrorResponse.Build(
                message: StaticOperationStatus.SkinTherapist.NotFound,
                statusCode: StaticOperationStatus.StatusCode.NotFound);
        }

        public async Task<ResponseDto> GetTherapistDetailsById(Guid therapistId)
        {
            var therapistFromDb = await _unitOfWork.SkinTherapist.GetAsync(
                filter: t => t.SkinTherapistId == therapistId,
                includeProperties: nameof(ApplicationUser));

            if (therapistFromDb is null)
            {
                return ErrorResponse.Build(
                    message: StaticOperationStatus.SkinTherapist.NotFound,
                    statusCode: StaticOperationStatus.StatusCode.NotFound);
            }

            var therapistDto = _autoMapperService.Map<SkinTherapist, GetSkinTherapistDto>(therapistFromDb);

            return SuccessResponse.Build(
                message: StaticOperationStatus.SkinTherapist.Found,
                statusCode: StaticOperationStatus.StatusCode.Ok,
                result: therapistDto);
        }

        public async Task<ResponseDto> GetTherapistsByServiceId(Guid serviceId)
        {
            var serviceFromDb = await _unitOfWork.Services.GetAsync(
                filter: s => s.ServiceId == serviceId,
                includeProperties: nameof(Models.Domain.Services.TypeItems));

            if (serviceFromDb is null)
            {
                return ErrorResponse.Build(
                    message: StaticResponseMessage.Service.NotFound,
                    statusCode: StaticOperationStatus.StatusCode.NotFound);
            }

            var serviceTypeIds = serviceFromDb.TypeItems.Select(ti => ti.ServiceTypeId).ToList();

            var therapistsFromDb = await _unitOfWork.SkinTherapist.GetAllAsync(
                filter: s => s.TherapistServiceTypes.Any(
                    tst => serviceTypeIds.Contains(tst.ServiceTypeId)),
                includeProperties: $"{nameof(SkinTherapist.TherapistServiceTypes)},{nameof(ApplicationUser)}");

            if (therapistsFromDb.Any())
            {
                var therapistListDto = _autoMapperService.MapCollection<SkinTherapist, GetSkinTherapistDto>(therapistsFromDb);

                return SuccessResponse.Build(
                    message: StaticOperationStatus.SkinTherapist.Found,
                    statusCode: StaticOperationStatus.StatusCode.Ok,
                    result: therapistListDto);
            }

            return SuccessResponse.Build(
                message: StaticOperationStatus.SkinTherapist.NotFound,
                statusCode: StaticOperationStatus.StatusCode.Ok,
                result: new List<SkinTherapist>());
        }

        public async Task<ResponseDto> GetTherapistsByServiceTypeId(Guid serviceTypeId)
        {
            var therapistsFromDb = await _unitOfWork.SkinTherapist.GetAllAsync(
                filter: s => s.TherapistServiceTypes.Any(
                    tst => tst.ServiceTypeId == serviceTypeId),
                includeProperties: $"{nameof(SkinTherapist.TherapistServiceTypes)},{nameof(ApplicationUser)}");

            if (therapistsFromDb.Any())
            {
                var therapistListDto = _autoMapperService.MapCollection<SkinTherapist, GetSkinTherapistDto>(therapistsFromDb);

                return SuccessResponse.Build(
                    message: StaticOperationStatus.SkinTherapist.Found,
                    statusCode: StaticOperationStatus.StatusCode.Ok,
                    result: therapistListDto);
            }

            return ErrorResponse.Build(
                message: StaticOperationStatus.SkinTherapist.NotFound,
                statusCode: StaticOperationStatus.StatusCode.NotFound);
        }

        public async Task<ResponseDto> GetTherapistTodayAppointments(ClaimsPrincipal User)
        {
            if (UserError.NotExists(User))
                return ErrorResponse.Build(
                    message: StaticResponseMessage.User.NotFound,
                    statusCode: StaticOperationStatus.StatusCode.NotFound);

            try
            {
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                var therapistFromDb = await _unitOfWork.SkinTherapist.GetAsync(st => st.UserId == userId);

                if (therapistFromDb is null)
                    return ErrorResponse.Build(
                        message: "Insufficient permission!",
                        statusCode: StaticOperationStatus.StatusCode.BadRequest);

                var today = DateOnly.FromDateTime(DateTime.UtcNow.AddHours(7));

                var appointmentsFromDb = await _unitOfWork.Appointments.GetAllAsync(
                    filter: a => a.AppointmentDate == today && a.TherapistSchedules.Any(ts => ts.TherapistId == therapistFromDb.SkinTherapistId),
                    includeProperties: $"{nameof(Appointments.TherapistSchedules)}," +
                       $"{nameof(Appointments.TherapistSchedules)}.{nameof(TherapistSchedule.SkinTherapist)}," +
                       $"{nameof(Appointments.TherapistSchedules)}.{nameof(TherapistSchedule.SkinTherapist)}.{nameof(SkinTherapist.ApplicationUser)}," +
                       $"{nameof(Appointments.Customer)}," +
                       $"{nameof(Appointments.Customer)}.{nameof(Customer.ApplicationUser)}");

                var appointmentListDto = appointmentsFromDb.Select(a =>
                {
                    var activeSchedule = a.TherapistSchedules?.FirstOrDefault
                    (
                        ts => ts.ScheduleStatus != ScheduleStatus.Cancelled &&
                              ts.ScheduleStatus != ScheduleStatus.Rescheduled
                    );

                    return new GetTodayAppointmentDto
                    {
                        Therapist = activeSchedule?.SkinTherapist?.ApplicationUser?.FullName ?? "Not Assigned",
                        Customer = a.Customer?.ApplicationUser?.FullName ?? "Unknown Customer",
                        Time = activeSchedule?.Slot?.StartTime.ToString("HH:mm") ?? "Not Scheduled",
                        Status = activeSchedule?.ScheduleStatus,
                        AppointmentId = a.AppointmentId,
                        CustomerId = a.CustomerId
                    };
                }).ToList();

                return new ResponseDto()
                {
                    Result = new
                    {
                        Appointments = appointmentListDto
                    },
                    Message = "Today's appointments retrieved successfully",
                    IsSuccess = true,
                    StatusCode = 200
                };
            }
            catch (Exception ex)
            {
                return ErrorResponse.Build(
                    message: ex.Message,
                    statusCode: StaticOperationStatus.StatusCode.InternalServerError);
            }
        }
    }
}
