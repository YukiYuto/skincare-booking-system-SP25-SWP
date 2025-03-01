using SkincareBookingSystem.DataAccess.IRepositories;
using SkincareBookingSystem.Models.Domain;
using SkincareBookingSystem.Models.Dto.Booking.ServiceType;
using SkincareBookingSystem.Models.Dto.Response;
using SkincareBookingSystem.Models.Dto.SkinTherapist;
using SkincareBookingSystem.Services.Helpers.Responses;
using SkincareBookingSystem.Services.IServices;
using SkincareBookingSystem.Utilities.Constants;
using System;
using System.Collections.Generic;
using System.Linq;
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
    }
}
