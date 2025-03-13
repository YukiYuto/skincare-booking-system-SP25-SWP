using AutoMapper;
using SkincareBookingSystem.DataAccess.IRepositories;
using SkincareBookingSystem.Models.Dto.Response;
using SkincareBookingSystem.Models.Dto.ServiceTypeDto;
using SkincareBookingSystem.Models.Domain;
using SkincareBookingSystem.Services.IServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using SkincareBookingSystem.Services.Helpers.Responses;
using SkincareBookingSystem.Utilities.Constants;

namespace SkincareBookingSystem.Services.Services
{
    public class ServiceTypeService : IServiceTypeService
    {
        private readonly IAutoMapperService _mapper;
        private readonly IUnitOfWork _unitOfWork;

        public ServiceTypeService(IUnitOfWork unitOfWork, IAutoMapperService mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper; // Inject IMapper
        }

        public async Task<ResponseDto> CreateBulkServiceTypes(ClaimsPrincipal User, List<CreateServiceTypeDto> createServiceTypeDtos)
        {
            var serviceTypes = _mapper.MapCollection<CreateServiceTypeDto, ServiceType>(createServiceTypeDtos);
            foreach (var serviceType in serviceTypes)
            {
                serviceType.CreatedBy = User.Identity?.Name;
            }

            await using var transaction = await _unitOfWork.BeginTransactionAsync();
            try
            {
                await _unitOfWork.ServiceType.AddRangeAsync(serviceTypes);
                await _unitOfWork.SaveAsync();
                await transaction.CommitAsync();

                return SuccessResponse.Build(
                    message: "Service type(s) created successfully",
                    statusCode: StaticOperationStatus.StatusCode.Created,
                    result: serviceTypes);
            }
            catch (Exception e)
            {
                await transaction.RollbackAsync();
                return ErrorResponse.Build(
                    message: $"Error when creating service type(s): {e.Message}",
                    statusCode: StaticOperationStatus.StatusCode.InternalServerError);
            }
        }

        public async Task<ResponseDto> CreateServiceType(ClaimsPrincipal User, CreateServiceTypeDto createServiceTypeDto)
        {
            try
            {
                // Using AutoMapper to map the DTO to the ServiceType model
                var serviceType = _mapper.Map<CreateServiceTypeDto, ServiceType>(createServiceTypeDto);

                // Set the created by and created time (if needed)
                serviceType.ServiceTypeId = Guid.NewGuid();
                serviceType.CreatedBy = User.Identity?.Name;

                // Add the serviceType to the database
                await _unitOfWork.ServiceType.AddAsync(serviceType);
                await _unitOfWork.SaveAsync();

                return new ResponseDto
                {
                    Result = serviceType,
                    Message = "Service Type(s) created successfully",
                    IsSuccess = true,
                    StatusCode = 201
                };
            }
            catch (Exception ex)
            {
                return new ResponseDto
                {
                    Message = $"Error when creating Service Type(s): {ex.Message}",
                    IsSuccess = false,
                    StatusCode = 500
                };
            }
        }

        public async Task<ResponseDto> DeleteServiceType(Guid id)
        {
            var serviceType = await _unitOfWork.ServiceType.GetAsync(s => s.ServiceTypeId == id);
            if (serviceType == null)
            {
                return new ResponseDto
                {
                    Message = "Service Type not found",
                    IsSuccess = false,
                    StatusCode = 404
                };
            }
            serviceType.Status = "1"; // Soft delete, 1 is deleted, 0 is active
            serviceType.UpdatedTime = DateTime.UtcNow.AddHours(7.0); // Update to UTC+7 (Vietnam timezone)

            await _unitOfWork.SaveAsync();

            return new ResponseDto
            {
                Message = "Service Type(s) deleted successfully",
                IsSuccess = true,
                StatusCode = 200
            };

        }

        public async Task<ResponseDto> GetAllServiceTypes()
        {
            var serviceType = await _unitOfWork.ServiceType.GetAllAsync(s => s.Status != "1");
            return new ResponseDto
            {
                Result = serviceType,
                Message = "Service Type(s) retrieved successfully",
                IsSuccess = true,
                StatusCode = 200
            };
        }

        public async Task<ResponseDto> GetServiceTypeById(Guid id)
        {
            var serviceType = await _unitOfWork.ServiceType.GetAsync(s => s.ServiceTypeId == id && s.Status != "1");
            if (serviceType == null)
            {
                return new ResponseDto
                {
                    Message = "Service Type not found",
                    IsSuccess = false,
                    StatusCode = 404
                };
            }
            return new ResponseDto
            {
                Result = serviceType,
                Message = "Service Type retrieved successfully",
                IsSuccess = true,
                StatusCode = 200
            };
        }

        public async Task<ResponseDto> UpdateServiceType(ClaimsPrincipal User, UpdateServiceTypeDto updateServiceTypeDto)
        {
            var serviceType = await _unitOfWork.ServiceType.GetAsync(s => s.ServiceTypeId == updateServiceTypeDto.ServiceTypeId && s.Status != "1");
            if (serviceType == null)
            {
                return new ResponseDto
                {
                    Message = "Service Type not found",
                    IsSuccess = false,
                    StatusCode = 404
                };
            }

            // Using AutoMapper to map the DTO to the ServiceType model
            var serviceTypeUpdate = _mapper.Map<UpdateServiceTypeDto, ServiceType>(updateServiceTypeDto);
            serviceType.UpdatedBy = User.Identity?.Name;

            // Update the serviceType in the database
            _unitOfWork.ServiceType.Update(serviceType, serviceTypeUpdate);
            await _unitOfWork.SaveAsync();

            return new ResponseDto
            {
                Result = serviceType,
                Message = "Service Type(s) updated successfully",
                IsSuccess = true,
                StatusCode = 200
            };

        }
    }
}
