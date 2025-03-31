using AutoMapper;
using SkincareBookingSystem.DataAccess.IRepositories;
using SkincareBookingSystem.Models.Dto.Response;
using SkincareBookingSystem.Models.Dto.Services;
using SkincareBookingSystem.Models.Domain;
using SkincareBookingSystem.Services.IServices;
using System.Security.Claims;
using SkincareBookingSystem.Utilities.Constants;
using SkincareBookingSystem.Services.Helpers.Responses;

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
                (pageNumber, pageSize, filterOn, filterQuery, sortBy, isManager,
                includeProperties: nameof(Models.Domain.Services.TypeItems));

            var activeServices = services.Where(s => s.Status == StaticOperationStatus.Service.Active).ToList();

            if (!activeServices.Any())
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
                _mapper.MapCollection<Models.Domain.Services, GetAllServicesDto>(activeServices);

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
            var service = await _unitOfWork.Services.GetAsync(s => s.ServiceId == id,
                includeProperties: $"{nameof(Models.Domain.Services.TypeItems)},{nameof(Models.Domain.Services.DurationItems)}.{nameof(DurationItem.ServiceDuration)}");

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

            var serviceResponseDto = _mapper.Map<Models.Domain.Services, GetAllServicesDto>(service);
            return new ResponseDto
            {
                Message = "Service retrieved successfully",
                IsSuccess = true,
                StatusCode = 200,
                Result = serviceResponseDto
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

            var updatedData = _mapper.Map<UpdateServiceDto, Models.Domain.Services>(updateServiceDto);
            service.UpdatedBy = User.FindFirstValue("FullName");
            service.Status = updatedData.Status;

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

        public async Task<ResponseDto> CreateBulkServices(ClaimsPrincipal User, List<CreateServiceDto> createServiceDtos)
        {
            if (createServiceDtos is null || createServiceDtos.Count == 0)
            {
                return ErrorResponse.Build(
                    message: "No services to create",
                    statusCode: StaticOperationStatus.StatusCode.BadRequest);
            }

            var servicesToCreate = _mapper.MapCollection<CreateServiceDto, Models.Domain.Services>(createServiceDtos);
            foreach (var service in servicesToCreate)
            {
                service.CreatedBy = User.Identity?.Name;
            }

            await using var transaction = await _unitOfWork.BeginTransactionAsync();
            try
            {
                await _unitOfWork.Services.AddRangeAsync(servicesToCreate);
                await _unitOfWork.SaveAsync();
                await transaction.CommitAsync();

                return SuccessResponse.Build(
                    message: "Services created successfully",
                    statusCode: StaticOperationStatus.StatusCode.Created,
                    result: servicesToCreate);
            }
            catch (Exception e)
            {
                await transaction.RollbackAsync();
                return ErrorResponse.Build(
                    message: "An error occurred while creating services: " + e.Message,
                    statusCode: StaticOperationStatus.StatusCode.InternalServerError);
            }
        }

        public async Task<ResponseDto> GetSimilarServices(Guid serviceId, int batch = 1, int itemPerBatch = 4)
        {
            var serviceFromDb = await _unitOfWork.Services.GetAsync(
                filter: s => s.ServiceId == serviceId,
                includeProperties: $"{nameof(Models.Domain.Services.TypeItems)}");

            if (serviceFromDb is null)
            {
                return ErrorResponse.Build(
                    message: StaticResponseMessage.Service.NotFound,
                    statusCode: StaticOperationStatus.StatusCode.NotFound);
            }

            var serviceTypeIds = serviceFromDb.TypeItems.Select(ti => ti.ServiceTypeId).ToList();

            var similarServices = await _unitOfWork.Services.GetAllAsync(
                filter: s => s.TypeItems.Any(ti => serviceTypeIds.Contains(ti.ServiceTypeId)),
                includeProperties: $"{nameof(Models.Domain.Services.TypeItems)}," +
                                   $"{nameof(Models.Domain.Services.DurationItems)}.{nameof(DurationItem.ServiceDuration)}");

            var similarServicesBatch = similarServices
                .Where(s => s.ServiceId != serviceId)
                .Skip((batch - 1) * itemPerBatch)
                .Take(itemPerBatch)
                .ToList();

            // If there are no more similar services, return an empty list
            if (!similarServicesBatch.Any())
            {
                return SuccessResponse.Build(
                    message: StaticResponseMessage.Service.NoSimilarServices,
                    statusCode: StaticOperationStatus.StatusCode.Ok,
                    result: new List<Models.Domain.Services>());
            }

            var similarServicesDto = _mapper.MapCollection<Models.Domain.Services, GetAllServicesDto>(similarServicesBatch);

            return SuccessResponse.Build(
                message: StaticResponseMessage.Service.SimilarServicesRetrieved,
                statusCode: StaticOperationStatus.StatusCode.Ok,
                result: similarServicesDto);
        }
    }
}