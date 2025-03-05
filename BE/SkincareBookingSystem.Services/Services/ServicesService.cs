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
using SkincareBookingSystem.Utilities.Constants;

namespace SkincareBookingSystem.Services.Services
{
    public class ServicesService : IServicesService
    {
        private readonly IAutoMapperService _mapper;
        private readonly IUnitOfWork _unitOfWork;

        public ServicesService(IUnitOfWork unitOfWork, IAutoMapperService mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<ResponseDto> GetAllServices
        (
            ClaimsPrincipal User,
            int pageNumber = 1,
            int pageSize = 10,
            string? filterOn = null,
            string? filterQuery = null,
            string? sortBy = null
        )
        {
            var userRole = User.FindFirstValue(ClaimTypes.Role);

            bool isManager = userRole == StaticUserRoles.Manager;

            var (services, totalServices) = await _unitOfWork.Services.GetServicesAsync
                (pageNumber, pageSize, filterOn, filterQuery, sortBy, isManager);

            if (!services.Any())
            {
                return new ResponseDto
                {
                    Result = services,
                    Message = "No services found",
                    IsSuccess = false,
                    StatusCode = 200
                };
            }

            int totalPages = (int)Math.Ceiling((double)totalServices / pageSize);

            var serviceDtos = 
                _mapper.MapCollection<Models.Domain.Services, GetAllServicesDto>(services);

            return new ResponseDto
            {
                Result = new
                {
                    TotalServices = totalServices,
                    TotalPages = totalPages,
                    PageSize = pageSize,
                    CurrentPage = pageNumber,
                    Services = serviceDtos
                },
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
                    IsSuccess = true,
                    Result = null,
                    StatusCode = 200
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

        public async Task<ResponseDto> CreateService(ClaimsPrincipal User, CreateServiceDto createServiceDto)
        {
            try
            {
                var service = _mapper.Map<CreateServiceDto, Models.Domain.Services>(createServiceDto);
                service.CreatedBy = User.FindFirstValue("FullName");

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
                    Message = ex.Message,
                    IsSuccess = false,
                    StatusCode = 500
                };
            }
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
                    Result = null,
                    StatusCode = 200
                };
            }

            // using AutoMapper to map the DTO to Services model
            var updatedData = _mapper.Map<UpdateServiceDto, Models.Domain.Services>(updateServiceDto);
            if (updatedData.ServiceTypeId == Guid.Empty)
            {
                //updatedData.ServiceTypeId = service.ServiceTypeId;
                return new ResponseDto
                {
                    Message = "Service Type Id is required",
                    IsSuccess = false,
                    StatusCode = 400,
                    Result = updatedData.ServiceTypeId
                };
            }

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
    }
}