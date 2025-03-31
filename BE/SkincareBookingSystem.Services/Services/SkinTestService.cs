using SkincareBookingSystem.DataAccess.IRepositories;
using SkincareBookingSystem.Models.Domain;
using SkincareBookingSystem.Models.Dto.Response;
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
    public class SkinTestService : ISkinTestService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IAutoMapperService _autoMapperService;

        public SkinTestService(IUnitOfWork unitOfWork, IAutoMapperService autoMapperService)
        {
            _unitOfWork = unitOfWork;
            _autoMapperService = autoMapperService;
        }

        public async Task<ResponseDto> CreateSkinTest(ClaimsPrincipal User, CreateSkinTestDto createSkinTestDto)
        {
            if (User.FindFirstValue(ClaimTypes.NameIdentifier) is null)
            {
                return ErrorResponse.Build(
                    message: StaticResponseMessage.User.NotFound,
                    statusCode: StaticOperationStatus.StatusCode.NotFound);
            }

            var createSkinTest = _autoMapperService.Map<CreateSkinTestDto, SkinTest>(createSkinTestDto);
            createSkinTest.CreatedBy = User.FindFirstValue("Fullname");
            createSkinTest.CreatedTime = StaticOperationStatus.Timezone.Vietnam;
            createSkinTest.Status = StaticOperationStatus.BaseEntity.Active;

            try
            {
                await _unitOfWork.SkinTest.AddAsync(createSkinTest);
                await SaveChangesAsync();
            }
            catch (Exception ex)
            {
                return ErrorResponse.Build(
                    message: StaticResponseMessage.SkinTest.NotCreated,
                    statusCode: StaticOperationStatus.StatusCode.InternalServerError);
            }
            return SuccessResponse.Build(
                message: StaticResponseMessage.SkinTest.Created,
                statusCode: StaticOperationStatus.StatusCode.Ok,
                result: createSkinTest);
        }

        public async Task<ResponseDto> DeleteSkinTest(ClaimsPrincipal User, Guid skinTestId)
        {
            if (User.FindFirstValue(ClaimTypes.NameIdentifier) is null)
            {
                return ErrorResponse.Build(
                    message: StaticResponseMessage.User.NotFound,
                    statusCode: StaticOperationStatus.StatusCode.NotFound);
            }

            var deleteSkinTest = await _unitOfWork.SkinTest.GetAsync(s => s.SkinTestId == skinTestId);
            if (deleteSkinTest is null)
            {
                return ErrorResponse.Build(
                    message: StaticResponseMessage.SkinTest.NotFound,
                    statusCode: StaticOperationStatus.StatusCode.NotFound);
            }

            if (deleteSkinTest.Status == StaticOperationStatus.SkinTest.Deleted)
            {
                return ErrorResponse.Build(
                    message: StaticResponseMessage.SkinTest.AlreadyDeleted,
                    statusCode: StaticOperationStatus.StatusCode.BadRequest);
            }

            deleteSkinTest.Status = StaticOperationStatus.SkinTest.Deleted;
            deleteSkinTest.UpdatedTime = StaticOperationStatus.Timezone.Vietnam;
            deleteSkinTest.UpdatedBy = User.FindFirstValue("Fullname");

            return (await SaveChangesAsync()) ?
                SuccessResponse.Build(
                    message: StaticResponseMessage.SkinTest.Deleted,
                    statusCode: StaticOperationStatus.StatusCode.Ok,
                    result: deleteSkinTest)
                :
                ErrorResponse.Build(
                    message: StaticResponseMessage.SkinTest.NotDeleted,
                    statusCode: StaticOperationStatus.StatusCode.InternalServerError);

        }

        public async Task<ResponseDto> GetAllSkinTests()
        {
            var getAllSkinTests = await _unitOfWork.SkinTest.GetAllAsync(s => s.Status != StaticOperationStatus.SkinTest.Deleted);
            return (getAllSkinTests.Any()) ?
                SuccessResponse.Build(
                    message: StaticResponseMessage.SkinTest.RetrievedAll,
                    statusCode: StaticOperationStatus.StatusCode.Ok,
                    result: getAllSkinTests)
                :
                SuccessResponse.Build(
                    message: StaticResponseMessage.SkinTest.NotFound,
                    statusCode: StaticOperationStatus.StatusCode.Ok,
                    result: new List<SkinTest>());
        }

        public async Task<ResponseDto> GetSkinTestById(ClaimsPrincipal User, Guid skinTestId)
        {
            if (User.FindFirstValue(ClaimTypes.NameIdentifier) is null)
            {
                return ErrorResponse.Build(
                    message: StaticResponseMessage.SkinTest.NotFound,
                    statusCode: StaticOperationStatus.StatusCode.NotFound);
            }

            var getSkinTest = await _unitOfWork.SkinTest.GetAsync(s => s.SkinTestId == skinTestId 
            && s.Status != StaticOperationStatus.SkinTest.Deleted);
            return (getSkinTest is null) ?
                ErrorResponse.Build(
                    message: StaticResponseMessage.SkinTest.NotFound,
                    statusCode: StaticOperationStatus.StatusCode.NotFound)
                :
                SuccessResponse.Build(
                    message: StaticResponseMessage.SkinTest.Retrieved,
                    statusCode: StaticOperationStatus.StatusCode.Ok,
                    result: getSkinTest);
        }

        public async Task<ResponseDto> UpdateSkinTest(ClaimsPrincipal User, UpdateSkinTestDto updateSkinTestDto)
        {
            if (User.FindFirstValue(ClaimTypes.NameIdentifier) is null)
            {
                return ErrorResponse.Build(
                    message: StaticResponseMessage.User.NotFound,
                    statusCode: StaticOperationStatus.StatusCode.NotFound);
            }

            var updateSkinTest = await _unitOfWork.SkinTest.GetAsync(s => s.SkinTestId == updateSkinTestDto.SkinTestId);
            if (updateSkinTest is null)
            {
                return ErrorResponse.Build(
                    message: StaticResponseMessage.SkinTest.NotFound,
                    statusCode: StaticOperationStatus.StatusCode.NotFound);
            }

            var updateData = _autoMapperService.Map<UpdateSkinTestDto, SkinTest>(updateSkinTestDto);
            updateData.UpdatedBy = User.FindFirstValue("Fullname");
            updateData.UpdatedTime = StaticOperationStatus.Timezone.Vietnam;
            updateData.Status = StaticOperationStatus.BaseEntity.Active;
            updateData.CreatedBy = updateSkinTest.CreatedBy;
            updateData.CreatedTime = updateSkinTest.CreatedTime;

            _unitOfWork.SkinTest.Update(updateSkinTest, updateData);

            return (await SaveChangesAsync()) ?
                SuccessResponse.Build(
                    message: StaticResponseMessage.SkinTest.Updated,
                    statusCode: StaticOperationStatus.StatusCode.Ok,
                    result: updateData)
                :
                ErrorResponse.Build(
                    message: StaticResponseMessage.SkinTest.NotUpdated,
                    statusCode: StaticOperationStatus.StatusCode.InternalServerError);
        }

        private async Task<bool> SaveChangesAsync()
        {
            return await _unitOfWork.SaveAsync() == StaticOperationStatus.Database.Success;
        }
    }
}
