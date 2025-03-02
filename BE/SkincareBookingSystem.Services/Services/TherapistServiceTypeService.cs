using SkincareBookingSystem.DataAccess.IRepositories;
using SkincareBookingSystem.Models.Dto.Response;
using SkincareBookingSystem.Models.Dto.TherapistServiceTypes;
using SkincareBookingSystem.Services.IServices;
using SkincareBookingSystem.Services.Helpers.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using SkincareBookingSystem.Services.Helpers.Responses;
using SkincareBookingSystem.Utilities.Constants;
using SkincareBookingSystem.Models.Domain;

namespace SkincareBookingSystem.Services.Services
{
    public class TherapistServiceTypeService : ITherapistServiceTypeService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IAutoMapperService _autoMapperService;

        public TherapistServiceTypeService(IUnitOfWork unitOfWork, IAutoMapperService autoMapperService)
        {
            _unitOfWork = unitOfWork;
            _autoMapperService = autoMapperService;
        }
        public async Task<ResponseDto> AssignServiceTypesToTherapist(ClaimsPrincipal User, TherapistServiceTypesDto therapistServiceTypesDto)
        {
            if (UserError.NotExists(User))
                return ErrorResponse.Build(message: StaticOperationStatus.User.UserNotFound, statusCode: StaticOperationStatus.StatusCode.NotFound);

            if (therapistServiceTypesDto.ServiceTypeIdList is null || !therapistServiceTypesDto.ServiceTypeIdList.Any())
                return ErrorResponse.Build(message: StaticOperationStatus.ServiceType.EmptyList, statusCode: StaticOperationStatus.StatusCode.BadRequest);

            var therapistFromDb = await _unitOfWork.SkinTherapist.GetAsync(
                filter: t => t.SkinTherapistId == therapistServiceTypesDto.TherapistId,
                includeProperties: nameof(SkinTherapist.TherapistServiceTypes));

            if (therapistFromDb is null)
                return ErrorResponse.Build(message: StaticOperationStatus.SkinTherapist.NotFound, statusCode: StaticOperationStatus.StatusCode.NotFound);

            // 1. Fetch only relevant service types from DB
            var serviceTypeIds = therapistServiceTypesDto.ServiceTypeIdList.Distinct().ToList();
            var validServiceTypes = await _unitOfWork.ServiceType
                .GetAllAsync(st => serviceTypeIds.Contains(st.ServiceTypeId))
                .ConfigureAwait(false);

            // 2. Validate all service types to be added (handles unexpected changes / malicious data)
            var validIdSet = validServiceTypes.Select(st => st.ServiceTypeId).ToHashSet();
            var invalidIds = serviceTypeIds.Except(validIdSet).ToList();
            if (invalidIds.Any())
            {
                return ErrorResponse.Build(
                    message: $"{StaticOperationStatus.ServiceType.Invalid}: {string.Join(", ", invalidIds)}",
                    statusCode: StaticOperationStatus.StatusCode.BadRequest);
            }

            // 3. Create and add only valid service types to the therapist
            // TODO: Change to use AutoMapper when TherapistServiceType is updated to include BaseEntity
            var therapistServiceTypes = _autoMapperService.MapCollection<Guid, TherapistServiceType>(validIdSet);
            foreach (var tst in therapistServiceTypes)
            {
                tst.TherapistId = therapistFromDb.SkinTherapistId;
                tst.CreatedBy = User.FindFirstValue("FullName");

            }

            // 4. Add service types to the therapist using transaction
            await using var transaction = await _unitOfWork.BeginTransactionAsync();
            try
            {
                await _unitOfWork.TherapistServiceType.AddRangeAsync(therapistServiceTypes).ConfigureAwait(false);
                await _unitOfWork.SaveAsync().ConfigureAwait(false);
                await transaction.CommitAsync().ConfigureAwait(false);
            }
            catch
            {
                await transaction.RollbackAsync().ConfigureAwait(false);
                return ErrorResponse.Build(
                    message: StaticOperationStatus.TherapistServiceType.NotAdded,
                    statusCode: StaticOperationStatus.StatusCode.InternalServerError);
            }

            return SuccessResponse.Build(
                message: StaticOperationStatus.TherapistServiceType.Added,
                statusCode: StaticOperationStatus.StatusCode.Created,
                result: therapistFromDb.TherapistServiceTypes);
        }

        public async Task<ResponseDto> RemoveServiceTypesForTherapist(ClaimsPrincipal User, TherapistServiceTypesDto therapistServiceTypesDto)
        {
            if (UserError.NotExists(User))
                return ErrorResponse.Build(
                    message: StaticOperationStatus.User.UserNotFound,
                    statusCode: StaticOperationStatus.StatusCode.NotFound);

            if (therapistServiceTypesDto.ServiceTypeIdList is null || !therapistServiceTypesDto.ServiceTypeIdList.Any())
                return ErrorResponse.Build(
                    message: StaticOperationStatus.ServiceType.EmptyList,
                    statusCode: StaticOperationStatus.StatusCode.BadRequest);

            var therapistFromDb = await _unitOfWork.SkinTherapist
                .GetAsync(
                    filter: t => t.SkinTherapistId == therapistServiceTypesDto.TherapistId,
                    includeProperties: nameof(SkinTherapist.TherapistSchedules))
                .ConfigureAwait(false);

            if (therapistFromDb is null)
                return ErrorResponse.Build(
                    message: StaticOperationStatus.SkinTherapist.NotFound,
                    statusCode: StaticOperationStatus.StatusCode.NotFound);

            // Fetch only existing service types from DB
            var serviceTypeIds = therapistServiceTypesDto.ServiceTypeIdList.Distinct().ToList();
            var existingServiceTypes = await _unitOfWork.TherapistServiceType
                .GetAllAsync(tst => tst.TherapistId == therapistFromDb.SkinTherapistId && serviceTypeIds.Contains(tst.ServiceTypeId))
                .ConfigureAwait(false);

            // Validate all service types to be removed (handles unexpected changes / malicious data)
            if (!existingServiceTypes.Any())
                return ErrorResponse.Build(
                    message: StaticOperationStatus.ServiceType.NotFound,
                    statusCode: StaticOperationStatus.StatusCode.NotFound);

            // Remove service types from the therapist using transaction
            await using var transaction = await _unitOfWork.BeginTransactionAsync();
            try
            {
                _unitOfWork.TherapistServiceType.RemoveRange(existingServiceTypes);
                await _unitOfWork.SaveAsync().ConfigureAwait(false);
                await transaction.CommitAsync().ConfigureAwait(false);
            }
            catch
            {
                await transaction.RollbackAsync().ConfigureAwait(false);
                return ErrorResponse.Build(
                    message: StaticOperationStatus.TherapistServiceType.NotRemoved,
                    statusCode: StaticOperationStatus.StatusCode.InternalServerError);
            }

            return SuccessResponse.Build(
                message: StaticOperationStatus.TherapistServiceType.Removed,
                statusCode: StaticOperationStatus.StatusCode.Ok,
                result: therapistFromDb.TherapistServiceTypes);
        }
    }
}
