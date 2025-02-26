using AutoMapper;
using SkincareBookingSystem.DataAccess.IRepositories;
using SkincareBookingSystem.Models.Domain;
using SkincareBookingSystem.Models.Dto.Response;
using SkincareBookingSystem.Models.Dto.Slots;
using SkincareBookingSystem.Services.IServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace SkincareBookingSystem.Services.Services
{
    public class SlotService : ISlotService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public SlotService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<ResponseDto> CreateSlot(ClaimsPrincipal User, CreateSlotDto createSlotDto)
        {
            try
            {
                if (createSlotDto.EndTime <= createSlotDto.StartTime)
                {
                    return new ResponseDto
                    {
                        Message = "End time must be greater than start time",
                        IsSuccess = false,
                        StatusCode = 400
                    };
                }

                var slot = _mapper.Map<CreateSlotDto, Slot>(createSlotDto);

                slot.SlotId = Guid.NewGuid();
                slot.CreatedBy = User.Identity?.Name;

                await _unitOfWork.Slot.AddAsync(slot);
                await _unitOfWork.SaveAsync();

                return new ResponseDto
                {
                    Result = slot,
                    Message = "Slot created successfully",
                    IsSuccess = true,
                    StatusCode = 201
                };
            }
            catch (Exception ex)
            {
                return new ResponseDto
                {
                    Message = $"Error when creating Service(s): {ex.Message}",
                    IsSuccess = false,
                    StatusCode = 500
                };
            }
        }

        public async Task<ResponseDto> DeleteSlot(Guid id)
        {
            var slot = await _unitOfWork.Slot.GetAsync(s => s.SlotId == id);
            if (slot == null)
            {
                return new ResponseDto
                {
                    Message = "Slot not found",
                    IsSuccess = false,
                    StatusCode = 404
                };
            }

            slot.Status = "1"; // Soft delete, 1 is deleted, 0 is active
            slot.UpdatedTime = DateTime.UtcNow;

            await _unitOfWork.SaveAsync();

            return new ResponseDto
            {
                Message = "Slot(s) deleted successfully",
                IsSuccess = true,
                StatusCode = 200
            };
        }

        public async Task<ResponseDto> GetAllSlots()
        {
            var slots = await _unitOfWork.Slot.GetAllAsync(s => s.Status != "1");
            return new ResponseDto
            {
                Result = slots,
                Message = "Slot(s) retrieved successfully",
                IsSuccess = true,
                StatusCode = 200
            };
        }

        public async Task<ResponseDto> GetSlotById(Guid id)
        {
            var slot = await _unitOfWork.Slot.GetAsync(s => s.SlotId == id);
            if (slot == null)
            {
                return new ResponseDto
                {
                    Message = "Slot not found",
                    IsSuccess = false,
                    StatusCode = 404
                };
            }
            return new ResponseDto
            {
                Result = slot,
                Message = "Slot retrieved successfully",
                IsSuccess = true,
                StatusCode = 200
            };
        }

        public async Task<ResponseDto> UpdateSlot(ClaimsPrincipal User, UpdateSlotDto updateSlotDto)
        {
            var slot = await _unitOfWork.Slot.GetAsync(s => s.SlotId == updateSlotDto.SlotId);
            if (slot == null)
            {
                return new ResponseDto
                {
                    Message = "Slot not found",
                    IsSuccess = false,
                    StatusCode = 404
                };
            }

            var updatedData = _mapper.Map<UpdateSlotDto, Slot>(updateSlotDto);
            slot.UpdatedBy = User.Identity?.Name;

            _unitOfWork.Slot.Update(slot, updatedData);
            await _unitOfWork.SaveAsync();

            return new ResponseDto
            {
                Message = "Slot updated successfully",
                IsSuccess = true,
                StatusCode = 200
            };

        }
    }
}
