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

        public async Task<ResponseDto> CreateBookingSchedule(ClaimsPrincipal User, CreateTherapistScheduleDto createBookingScheduleDto)
        {
            try
            {

                var bookingSchedule = _mapper.Map<CreateTherapistScheduleDto, TherapistSchedule>(createBookingScheduleDto);

                bookingSchedule.TherapistScheduleId = Guid.NewGuid();
                bookingSchedule.CreatedBy = User.Identity?.Name;

                await _unitOfWork.TherapistSchedule.AddAsync(bookingSchedule);
                await _unitOfWork.SaveAsync();

                return new ResponseDto
                {
                    IsSuccess = true,
                    Message = "Booking schedule created successfully",
                    StatusCode = 201,
                    Result = bookingSchedule
                };
            }
            catch (Exception ex)
            {
                return new ResponseDto
                {
                    IsSuccess = false,
                    Message = $"Booking schedule creation failed: {ex.Message}",
                    StatusCode = 500,
                };
            }
        }

        public async Task<ResponseDto> GetAllBookingSchedules()
        {
            var bookingSchedules = await _unitOfWork.TherapistSchedule.GetAllAsync();
            return new ResponseDto
            {
                IsSuccess = true,
                Message = "All booking schedules retrieved successfully",
                StatusCode = 200,
                Result = bookingSchedules
            };
        }

        public async Task<ResponseDto> GetBookingScheduleById(Guid id)
        {
            var bookingSchedule = await _unitOfWork.TherapistSchedule.GetAsync(b => b.TherapistScheduleId == id);
            if (bookingSchedule == null)
            {
                return new ResponseDto
                {
                    IsSuccess = false,
                    Message = "Booking schedule not found",
                    StatusCode = 404
                };
            }

            return new ResponseDto
            {
                IsSuccess = true,
                Message = "Booking schedule retrieved successfully",
                StatusCode = 200,
                Result = bookingSchedule
            };
        }

        public async Task<ResponseDto> UpdateBookingSchedule(ClaimsPrincipal User, UpdateTherapistScheduleDto updateBookingScheduleDto)
        {
            var bookingSchedule = await _unitOfWork.TherapistSchedule.GetAsync(b => b.TherapistScheduleId == updateBookingScheduleDto.BookingScheduleId);
            if (bookingSchedule == null)
            {
                return new ResponseDto
                {
                    IsSuccess = false,
                    Message = "Booking schedule not found",
                    StatusCode = 404
                };
            }

            var updatedData = _mapper.Map<UpdateTherapistScheduleDto, TherapistSchedule>(updateBookingScheduleDto);
            bookingSchedule.UpdatedBy = User.Identity?.Name;

            _unitOfWork.TherapistSchedule.Update(bookingSchedule, updatedData);
            await _unitOfWork.SaveAsync();

            return new ResponseDto
            {
                IsSuccess = true,
                Message = "Booking schedule updated successfully",
                StatusCode = 200,
                Result = bookingSchedule
            };
        }

        public async Task<ResponseDto> DeleteBookingSchedule(Guid id)
        {
            var booking = await _unitOfWork.TherapistSchedule.GetAsync(s => s.TherapistScheduleId == id);
            if (booking == null)
            {
                return new ResponseDto
                {
                    Message = "Service not found",
                    IsSuccess = false,
                    StatusCode = 404
                };
            }
            booking.Status = "1"; // Soft delete, 1 is deleted, 0 is active
            booking.UpdatedTime = DateTime.UtcNow;

            await _unitOfWork.SaveAsync();

            return new ResponseDto
            {
                Message = "Service(s) deleted successfully",
                IsSuccess = true,
                StatusCode = 200
            };
        }
    }
}
