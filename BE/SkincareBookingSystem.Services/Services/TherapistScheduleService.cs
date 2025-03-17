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

namespace SkincareBookingSystem.Services.Services
{
    public class TherapistScheduleService : ITherapistScheduleService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public TherapistScheduleService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<ResponseDto> CreateTherapistSchedule(ClaimsPrincipal User, CreateTherapistScheduleDto createBookingScheduleDto)
        {
            if (User.FindFirstValue(ClaimTypes.NameIdentifier) is null)
            {
                return ErrorResponse.Build(
                    message: StaticResponseMessage.User.NotFound,
                    statusCode: StaticOperationStatus.StatusCode.NotFound);
            }

            var bookingScheduleToCreate = _mapper.Map<CreateTherapistScheduleDto, TherapistSchedule>(createBookingScheduleDto);
            bookingScheduleToCreate.CreatedBy = User.Identity?.Name;

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

        public async Task<ResponseDto> GetTherapistScheduleByTherapistId(Guid therapistId)
        {
            if (await _unitOfWork.SkinTherapist.GetAsync(t => t.SkinTherapistId == therapistId) is null)
            {
                return ErrorResponse.Build(
                    message: StaticResponseMessage.BookingSchedule.NotFound,
                    statusCode: StaticOperationStatus.StatusCode.NotFound);
            }

            var bookingSchedules = await _unitOfWork.TherapistSchedule.GetAllAsync(b => b.TherapistId == therapistId && b.Status != StaticOperationStatus.BookingSchedule.Deleted);

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

            var updatedData = _mapper.Map<UpdateTherapistScheduleDto, TherapistSchedule>(updateBookingScheduleDto);
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
