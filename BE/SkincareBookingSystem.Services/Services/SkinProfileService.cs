using SkincareBookingSystem.DataAccess.IRepositories;
using SkincareBookingSystem.Models.Domain;
using SkincareBookingSystem.Models.Dto.Response;
using SkincareBookingSystem.Models.Dto.SkinProfile;
using SkincareBookingSystem.Models.Dto.SkinTest;
using SkincareBookingSystem.Services.Helpers.Responses;
using SkincareBookingSystem.Services.IServices;
using SkincareBookingSystem.Utilities.Constants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace SkincareBookingSystem.Services.Services
{
    public class SkinProfileService : ISkinProfileService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IAutoMapperService _autoMapperService;

        public SkinProfileService(IUnitOfWork unitOfWork, IAutoMapperService autoMapperService)
        {
            _unitOfWork = unitOfWork;
            _autoMapperService = autoMapperService;
        }

        public async Task<ResponseDto> CreateSkinProfile(ClaimsPrincipal User, CreateSkinProfileDto createSkinProfileDto)
        {
            if (User.FindFirstValue(ClaimTypes.NameIdentifier) is null)
            {
                return ErrorResponse.Build(
                    message: StaticResponseMessage.User.NotFound,
                    statusCode: StaticOperationStatus.StatusCode.NotFound);
            }

            var createSkinProfile = _autoMapperService.Map<CreateSkinProfileDto, SkinProfile>(createSkinProfileDto);
            createSkinProfile.CreatedBy = User.FindFirstValue("Fullname");
            createSkinProfile.CreatedTime = StaticOperationStatus.Timezone.Vietnam;
            createSkinProfile.Status = StaticOperationStatus.BaseEntity.Active;

            try
            {
                await _unitOfWork.SkinProfile.AddAsync(createSkinProfile);
                await _unitOfWork.SaveAsync();
            }
            catch (Exception e)
            {
                return ErrorResponse.Build(
                    message: StaticResponseMessage.SkinProfile.NotCreated,
                    statusCode: StaticOperationStatus.StatusCode.InternalServerError);
            }
            return SuccessResponse.Build(
                message: StaticResponseMessage.SkinProfile.Created,
                statusCode: StaticOperationStatus.StatusCode.Ok,
                result: createSkinProfile);
        }

        public async Task<ResponseDto> DeleteSkinProfile(ClaimsPrincipal User, Guid skinProfileId)
        {
            if (User.FindFirstValue(ClaimTypes.NameIdentifier) is null)
            {
                return ErrorResponse.Build(
                    message: StaticResponseMessage.User.NotFound,
                    statusCode: StaticOperationStatus.StatusCode.NotFound);
            }

            var deleteSkinProfile = await _unitOfWork.SkinProfile.GetAsync(s => s.SkinProfileId == skinProfileId);
            if (deleteSkinProfile is null)
            {
                return ErrorResponse.Build(
                    message: StaticResponseMessage.SkinProfile.NotFound,
                    statusCode: StaticOperationStatus.StatusCode.NotFound);
            }

            if (deleteSkinProfile.Status == StaticOperationStatus.SkinProfile.Deleted)
            {
                return ErrorResponse.Build(
                    message: StaticResponseMessage.SkinProfile.AlreadyDeleted,
                    statusCode: StaticOperationStatus.StatusCode.BadRequest);
            }

            deleteSkinProfile.Status = StaticOperationStatus.SkinProfile.Deleted;
            deleteSkinProfile.UpdatedTime = StaticOperationStatus.Timezone.Vietnam;
            deleteSkinProfile.UpdatedBy = User.FindFirstValue("Fullname");

            return (await SaveChangesAsync()) ?
                SuccessResponse.Build(
                    message: StaticResponseMessage.SkinProfile.Deleted,
                    statusCode: StaticOperationStatus.StatusCode.Ok,
                    result: deleteSkinProfile)
                :
                ErrorResponse.Build(
                    message: StaticResponseMessage.SkinProfile.NotDeleted,
                    statusCode: StaticOperationStatus.StatusCode.InternalServerError);
        }

        public async Task<ResponseDto> GetAllSkinProfiles()
        {
            var getAllSkinProfile = await _unitOfWork.SkinProfile.GetAllAsync(s => s.Status != StaticOperationStatus.SkinProfile.Deleted);
            return (getAllSkinProfile.Any()) ?
                SuccessResponse.Build(
                    message: StaticResponseMessage.SkinProfile.RetrievedAll,
                    statusCode: StaticOperationStatus.StatusCode.Ok,
                    result: getAllSkinProfile)
                :
                SuccessResponse.Build(
                    message: StaticResponseMessage.SkinProfile.NotFound,
                    statusCode: StaticOperationStatus.StatusCode.Ok,
                    result: new List<SkinProfile>());
        }

        public async Task<ResponseDto> GetSkinProfileById(ClaimsPrincipal User, Guid skinProfileId)
        {
            if (User.FindFirstValue(ClaimTypes.NameIdentifier) is null)
            {
                return ErrorResponse.Build(
                    message: StaticResponseMessage.SkinProfile.NotFound,
                    statusCode: StaticOperationStatus.StatusCode.NotFound);
            }

            var getSkinProfile = await _unitOfWork.SkinProfile.GetAsync(s => s.SkinProfileId == skinProfileId
                && s.Status != StaticOperationStatus.SkinProfile.Deleted);
            return (getSkinProfile is null) ?
                ErrorResponse.Build(
                    message: StaticResponseMessage.SkinProfile.NotFound,
                    statusCode: StaticOperationStatus.StatusCode.NotFound)
                :
                SuccessResponse.Build(
                    message: StaticResponseMessage.SkinProfile.Retrieved,
                    statusCode: StaticOperationStatus.StatusCode.Ok,
                    result: getSkinProfile);
        }

        public async Task<ResponseDto> UpdateSkinProfile(ClaimsPrincipal User, UpdateSkinProfileDto updateSkinProfileDto)
        {
            if (User.FindFirstValue(ClaimTypes.NameIdentifier) is null)
            {
                return ErrorResponse.Build(
                    message: StaticResponseMessage.User.NotFound,
                    statusCode: StaticOperationStatus.StatusCode.NotFound);
            }

            var updateSkinProfile = await _unitOfWork.SkinProfile.GetAsync(s => s.SkinProfileId == updateSkinProfileDto.SkinProfileId);

            if (updateSkinProfile is null)
            {
                return ErrorResponse.Build(
                    message: StaticResponseMessage.SkinProfile.NotFound,
                    statusCode: StaticOperationStatus.StatusCode.NotFound);
            }

            var updateData = _autoMapperService.Map<UpdateSkinProfileDto, SkinProfile>(updateSkinProfileDto);
            updateData.UpdatedBy = User.FindFirstValue("Fullname");
            updateData.UpdatedTime = StaticOperationStatus.Timezone.Vietnam;
            updateData.Status = StaticOperationStatus.BaseEntity.Active;
            updateData.CreatedBy = updateSkinProfile.CreatedBy;
            updateData.CreatedTime = updateSkinProfile.CreatedTime;

            _unitOfWork.SkinProfile.Update(updateSkinProfile, updateData);

            return (await SaveChangesAsync()) ?
                SuccessResponse.Build(
                    message: StaticResponseMessage.SkinProfile.Updated,
                    statusCode: StaticOperationStatus.StatusCode.Ok,
                    result: updateData)
                :
                ErrorResponse.Build(
                    message: StaticResponseMessage.SkinProfile.NotUpdated,
                    statusCode: StaticOperationStatus.StatusCode.InternalServerError);
        }

        private async Task<bool> SaveChangesAsync()
        {
            return await _unitOfWork.SaveAsync() == StaticOperationStatus.Database.Success;
        }
    }
}
