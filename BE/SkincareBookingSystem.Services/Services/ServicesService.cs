using AutoMapper;
using SkincareBookingSystem.DataAccess.IRepositories;
using SkincareBookingSystem.Models.Dto.Response;
using SkincareBookingSystem.Models.Dto.Services;
using SkincareBookingSystem.Models.Domain;
using SkincareBookingSystem.Services.IServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace SkincareBookingSystem.Services.Services
{
    public class ServicesService : IServicesService
    {
        private readonly IAutoMapperService _mapper;
        private readonly IUnitOfWork _unitOfWork;

        public ServicesService(IUnitOfWork unitOfWork, IAutoMapperService mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper; // Inject IMapper
        }
        public async Task<ResponseDto> CreateService(ClaimsPrincipal User, CreateServiceDto createServiceDto)
        {
            try
            {
                // Check user authorization (if needed)
                //if (!User.IsInRole("Admin"))
                //{
                //    return new ResponseDto
                //    {
                //        Message = "You are not authorized to perform this action",
                //        IsSuccess = false,
                //        StatusCode = 403
                //    };
                //}

                // Using AutoMapper to map the DTO to Services model
                var service = _mapper.Map<CreateServiceDto, Models.Domain.Services>(createServiceDto);

                // Set the created by and created time (if needed)
                service.ServiceId = Guid.NewGuid();
                service.CreatedBy = User.Identity?.Name;

                // Add the service to the database
                await _unitOfWork.Services.AddAsync(service);
                await _unitOfWork.SaveAsync();

                return new ResponseDto
                {
                    Result = service,
                    Message = "Service(s) created successfully",
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


        public async Task<ResponseDto> DeleteService(Guid id)
        {
            var service = await _unitOfWork.Services.GetAsync(s => s.ServiceId == id);
            if (service == null)
            {
                return new ResponseDto
                {
                    Message = "Service not found",
                    IsSuccess = false,
                    StatusCode = 404
                };
            }
            service.Status = "1"; // Soft delete, 1 is deleted, 0 is active
            service.UpdatedTime = DateTime.UtcNow;

            await _unitOfWork.SaveAsync();

            return new ResponseDto
            {
                Message = "Service(s) deleted successfully",
                IsSuccess = true,
                StatusCode = 200
            };
        }

        public async Task<ResponseDto> GetAllServices()
        {
            var services = await _unitOfWork.Services.GetAllAsync(s => s.Status != "1");
            return new ResponseDto
            {
                Result = services,
                Message = "Service(s) retrieved successfully",
                IsSuccess = true,
                StatusCode = 200
            };
        }

        public async Task<ResponseDto> GetServiceById(Guid id)
        {
            var service = await _unitOfWork.Services.GetAsync(s => s.ServiceId == id);
            if (service == null)
            {
                return new ResponseDto
                {
                    Message = "Service not found",
                    IsSuccess = false,
                    StatusCode = 404
                };
            }
            return new ResponseDto
            {
                Result = service,
                Message = "Service retrieved successfully",
                IsSuccess = true,
                StatusCode = 200
            };
        }

        public async Task<ResponseDto> UpdateService(ClaimsPrincipal User, UpdateServiceDto updateServiceDto)
        {
            var service = await _unitOfWork.Services.GetAsync(s => s.ServiceId == updateServiceDto.ServiceId);
            if (service == null)
            {
                return new ResponseDto
                {
                    Message = "Service not found",
                    IsSuccess = false,
                    StatusCode = 404
                };
            }

            // using AutoMapper to map the DTO to Services model
            var updatedData = _mapper.Map<UpdateServiceDto, Models.Domain.Services>(updateServiceDto);
            service.UpdatedBy = User.Identity?.Name;

            // Update the service
            _unitOfWork.Services.Update(service, updatedData);
            await _unitOfWork.SaveAsync();

            return new ResponseDto
            {
                Result = updatedData,
                Message = "Service updated successfully",
                IsSuccess = true,
                StatusCode = 200
            };
        }
    }
}
