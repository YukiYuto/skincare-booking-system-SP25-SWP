using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using SkincareBookingSystem.DataAccess.IRepositories;
using SkincareBookingSystem.Models.Domain;
using SkincareBookingSystem.Models.Dto.Response;
using SkincareBookingSystem.Models.Dto.BookingSchedule;
using SkincareBookingSystem.Services.IServices;
using System.Security.Claims;
using SkincareBookingSystem.Utilities.Constants;
using SkincareBookingSystem.Services.Helpers.Responses;
using SkincareBookingSystem.Models.Dto.TherapistSchedules;

namespace SkincareBookingSystem.Services.Services
{
    public class TherapistScheduleService : ITherapistScheduleService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IAutoMapperService _autoMapperService;

        public TherapistScheduleService(IUnitOfWork unitOfWork, IAutoMapperService autoMapperService)
        {
            _unitOfWork = unitOfWork;
            _autoMapperService = autoMapperService;
        }

        public async Task<ResponseDto> CreateTherapistSchedule(ClaimsPrincipal User, CreateTherapistScheduleDto createBookingScheduleDto)
        {
            if (User.FindFirstValue(ClaimTypes.NameIdentifier) is null)
            {
                return ErrorResponse.Build(
                    message: StaticResponseMessage.User.NotFound,
                    statusCode: StaticOperationStatus.StatusCode.NotFound);
            }

            var bookingScheduleToCreate = _autoMapperService.Map<CreateTherapistScheduleDto, TherapistSchedule>(createBookingScheduleDto);
            bookingScheduleToCreate.CreatedBy = User.FindFirstValue("Fullname");
            bookingScheduleToCreate.CreatedTime = StaticOperationStatus.Timezone.Vietnam;
            bookingScheduleToCreate.ScheduleStatus = ScheduleStatus.Pending;
            bookingScheduleToCreate.Status = StaticOperationStatus.BaseEntity.Active;

            try
            {
                await _unitOfWork.TherapistSchedule.AddAsync(bookingScheduleToCreate);
                await _unitOfWork.SaveAsync();
            }
            catch (Exception e)
            {
                return ErrorResponse.Build(
                    message: StaticResponseMessage.BookingSchedule.NotCreated,
                    statusCode: StaticOperationStatus.StatusCode.InternalServerError);
            }
            return SuccessResponse.Build(
                message: StaticResponseMessage.BookingSchedule.Created,
                statusCode: StaticOperationStatus.StatusCode.Ok,
                result: bookingScheduleToCreate);
        }

        public async Task<ResponseDto> GetAllTherapistSchedules()
        {
            var bookingSchedules = await _unitOfWork.TherapistSchedule.GetAllAsync(b => b.Status != StaticOperationStatus.BookingSchedule.Deleted);
            return (bookingSchedules.Any()) ?
                SuccessResponse.Build(
                    message: StaticResponseMessage.BookingSchedule.RetrievedAll,
                    statusCode: StaticOperationStatus.StatusCode.Ok,
                    result: bookingSchedules)
                :
                SuccessResponse.Build(
                    message: StaticResponseMessage.BookingSchedule.NotFound,
                    statusCode: StaticOperationStatus.StatusCode.Ok,
                    result: new List<TherapistSchedule>());
        }

        public async Task<ResponseDto> GetTherapistScheduleById(ClaimsPrincipal User, Guid scheduleId)
        {
            if (User.FindFirstValue(ClaimTypes.NameIdentifier) is null)
            {
                return ErrorResponse.Build(
                    message: StaticResponseMessage.User.NotFound,
                    statusCode: StaticOperationStatus.StatusCode.NotFound);
            }

            var bookingSchedule = await _unitOfWork.TherapistSchedule.GetAsync(b => b.TherapistScheduleId == scheduleId && b.Status != StaticOperationStatus.BookingSchedule.Deleted);
            return (bookingSchedule is null) ?
                ErrorResponse.Build(
                    message: StaticResponseMessage.BookingSchedule.NotFound,
                    statusCode: StaticOperationStatus.StatusCode.NotFound)
                :
                SuccessResponse.Build(
                    message: StaticResponseMessage.BookingSchedule.Retrieved,
                    statusCode: StaticOperationStatus.StatusCode.Ok,
                    result: bookingSchedule);
        }

        public async Task<ResponseDto> GetTherapistScheduleByTherapistId(ClaimsPrincipal User, Guid therapistId)
        {
            if (User.FindFirstValue(ClaimTypes.NameIdentifier) is null)
            {
                return ErrorResponse.Build(
                    message: StaticResponseMessage.User.NotFound,
                    statusCode: StaticOperationStatus.StatusCode.NotFound);
            }

            var therapistExists = await _unitOfWork.SkinTherapist.GetAsync(t => t.SkinTherapistId == therapistId);
            if (therapistExists == null)
            {
                return ErrorResponse.Build(
                    message: StaticResponseMessage.User.NotFound,
                    statusCode: StaticOperationStatus.StatusCode.NotFound);
            }

            var schedules = await _unitOfWork.TherapistSchedule.GetAllAsync(
                filter: b => b.TherapistId == therapistId && b.Status != StaticOperationStatus.BookingSchedule.Deleted,
                includeProperties: $"{nameof(TherapistSchedule.Appointment)},{nameof(TherapistSchedule.Slot)}");

            var scheduleDtos = schedules.Select(s => new GetTherapistScheduleDto
            {
                TherapistScheduleId = s.TherapistScheduleId,
                AppointmentId = s.Appointment?.AppointmentId ?? Guid.Empty, // Handle null Appointment
                SlotId = s.Slot?.SlotId ?? Guid.Empty, // Handle null Slot
                AppointmentDate = s.Appointment?.AppointmentDate.ToDateTime(TimeOnly.MinValue) // Convert DateOnly to DateTime
            }).ToList();

            return scheduleDtos.Any() ?
                SuccessResponse.Build(
                    message: StaticResponseMessage.BookingSchedule.RetrievedAll,
                    statusCode: StaticOperationStatus.StatusCode.Ok,
                    result: scheduleDtos)
                :
                SuccessResponse.Build(
                    message: StaticResponseMessage.BookingSchedule.NotFound,
                    statusCode: StaticOperationStatus.StatusCode.Ok,
                    result: new List<GetTherapistScheduleDto>());

        }

        public async Task<ResponseDto> UpdateTherapistSchedule(ClaimsPrincipal User, UpdateTherapistScheduleDto updateBookingScheduleDto)
        {
            if (User.FindFirstValue(ClaimTypes.NameIdentifier) is null)
            {
                return ErrorResponse.Build(
                    message: StaticResponseMessage.User.NotFound,
                    statusCode: StaticOperationStatus.StatusCode.NotFound);
            }

            var bookingSchedule = await _unitOfWork.TherapistSchedule.GetAsync(b => b.TherapistScheduleId == updateBookingScheduleDto.TherapistScheduleId);
            if (bookingSchedule == null)
            {
                return ErrorResponse.Build(
                    message: StaticResponseMessage.BookingSchedule.NotFound,
                    statusCode: StaticOperationStatus.StatusCode.NotFound);
            }

            var updatedData = _autoMapperService.Map<UpdateTherapistScheduleDto, TherapistSchedule>(updateBookingScheduleDto);
            _unitOfWork.TherapistSchedule.Update(bookingSchedule, updatedData);

            return (await SaveChangesAsync()) ?
                SuccessResponse.Build(
                    message: StaticResponseMessage.BookingSchedule.Updated,
                    statusCode: StaticOperationStatus.StatusCode.Ok,
                    result: updatedData)
                :
                ErrorResponse.Build(
                    message: StaticResponseMessage.BookingSchedule.NotUpdated,
                    statusCode: StaticOperationStatus.StatusCode.InternalServerError);
        }

        public async Task<ResponseDto> DeleteTherapistSchedule(ClaimsPrincipal User, Guid scheduleId)
        {
            if (User.FindFirstValue(ClaimTypes.NameIdentifier) is null)
            {
                return ErrorResponse.Build(
                    message: StaticResponseMessage.User.NotFound,
                    statusCode: StaticOperationStatus.StatusCode.NotFound);
            }

            var booking = await _unitOfWork.TherapistSchedule.GetAsync(s => s.TherapistScheduleId == scheduleId);
            if (booking == null)
            {
                return ErrorResponse.Build(
                    message: StaticResponseMessage.BookingSchedule.NotFound,
                    statusCode: StaticOperationStatus.StatusCode.NotFound);
            }
            if (booking.Status == StaticOperationStatus.BookingSchedule.Deleted)
            {
                return ErrorResponse.Build(
                    message: StaticResponseMessage.BookingSchedule.AlreadyDeleted,
                    statusCode: StaticOperationStatus.StatusCode.BadRequest);
            }

            booking.Status = StaticOperationStatus.BookingSchedule.Deleted;
            booking.UpdatedTime = StaticOperationStatus.Timezone.Vietnam;   
            booking.UpdatedBy = User.FindFirstValue("FullName");

            return (await SaveChangesAsync()) ?
                SuccessResponse.Build(
                    message: StaticResponseMessage.BookingSchedule.Deleted,
                    statusCode: StaticOperationStatus.StatusCode.Ok,
                    result: booking)
                :
                ErrorResponse.Build(
                    message: StaticResponseMessage.BookingSchedule.NotDeleted,
                    statusCode: StaticOperationStatus.StatusCode.InternalServerError);
        }

        private async Task<bool> SaveChangesAsync()
            {
                return await _unitOfWork.SaveAsync() == StaticOperationStatus.Database.Success;
            }
    }
}
