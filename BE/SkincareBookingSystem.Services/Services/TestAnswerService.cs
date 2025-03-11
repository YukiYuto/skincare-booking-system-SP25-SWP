using SkincareBookingSystem.DataAccess.IRepositories;
using SkincareBookingSystem.Models.Domain;
using SkincareBookingSystem.Models.Dto.Response;
using SkincareBookingSystem.Models.Dto.TestAnswer;
using SkincareBookingSystem.Models.Dto.TestQuestion;
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
    public class TestAnswerService : ITestAnswerService
    {
        private readonly IAutoMapperService _autoMapperService;
        private readonly IUnitOfWork _unitOfWork;

        public TestAnswerService(IAutoMapperService autoMapperService, IUnitOfWork unitOfWork)
        {
            _autoMapperService = autoMapperService;
            _unitOfWork = unitOfWork;
        }

        public async Task<ResponseDto> CreateTestAnswer(ClaimsPrincipal User, CreateTestAnswerDto createTestAnswerDto)
        {
            if (User.FindFirstValue(ClaimTypes.NameIdentifier) is null)
            {
                return ErrorResponse.Build(
                    message: StaticResponseMessage.User.NotFound,
                    statusCode: StaticOperationStatus.StatusCode.NotFound);
            }

            var testAnswerToCreate = _autoMapperService.Map<CreateTestAnswerDto, TestAnswer>(createTestAnswerDto);
            testAnswerToCreate.CreatedBy = User.FindFirstValue("FullName");

            try
            {
                await _unitOfWork.TestAnswer.AddAsync(testAnswerToCreate);
                await _unitOfWork.SaveAsync();
            }
            catch (Exception e)
            {
                return ErrorResponse.Build(
                    message: StaticResponseMessage.TestAnswer.NotCreated,
                    statusCode: StaticOperationStatus.StatusCode.InternalServerError);
            }
            return SuccessResponse.Build(
                message: StaticResponseMessage.TestAnswer.Created,
                statusCode: StaticOperationStatus.StatusCode.Ok,
                result: testAnswerToCreate);
        }

        public async Task<ResponseDto> DeleteTestAnswer(ClaimsPrincipal User, Guid testAnswerId)
        {
            if (User.FindFirstValue(ClaimTypes.NameIdentifier) is null)
            {
                return ErrorResponse.Build(
                    message: StaticResponseMessage.User.NotFound,
                    statusCode: StaticOperationStatus.StatusCode.NotFound);
            }
            var testAnswerToDelete = await _unitOfWork.TestAnswer.GetAsync(a => a.TestAnswerId == testAnswerId);
            if (testAnswerToDelete is null)
            {
                return ErrorResponse.Build(
                    message: StaticResponseMessage.TestAnswer.NotFound,
                    statusCode: StaticOperationStatus.StatusCode.NotFound);
            }

            testAnswerToDelete.Status = StaticOperationStatus.TestAnswer.Deleted;
            testAnswerToDelete.UpdatedTime = StaticOperationStatus.Timezone.Vietnam;
            testAnswerToDelete.UpdatedBy = User.FindFirstValue("FullName");

            return (await SaveChangesAsync()) ?
                SuccessResponse.Build(
                    message: StaticResponseMessage.TestAnswer.Deleted,
                    statusCode: StaticOperationStatus.StatusCode.Ok,
                    result: testAnswerToDelete)
                :
                ErrorResponse.Build(
                    message: StaticResponseMessage.TestAnswer.NotDeleted,
                    statusCode: StaticOperationStatus.StatusCode.InternalServerError);
        }

        public async Task<ResponseDto> GetAllTestAnswers()
        {
            var testAnswerFromDb = await _unitOfWork.TestAnswer.GetAllAsync(a => a.Status != StaticOperationStatus.TestAnswer.Deleted);

            return (testAnswerFromDb.Any()) ?
                SuccessResponse.Build(
                    message: StaticResponseMessage.TestAnswer.RetrievedAll,
                    statusCode: StaticOperationStatus.StatusCode.Ok,
                    result: testAnswerFromDb)
                :
                SuccessResponse.Build(
                    message: StaticResponseMessage.TestAnswer.NotFound,
                    statusCode: StaticOperationStatus.StatusCode.Ok,
                    result: new List<TestAnswer>());
        }

        public async Task<ResponseDto> GetTestAnswerById(ClaimsPrincipal User, Guid testAnswerId)
        {
            if (User.FindFirstValue(ClaimTypes.NameIdentifier) is null)
            {
                return ErrorResponse.Build(
                    message: StaticResponseMessage.User.NotFound,
                    statusCode: StaticOperationStatus.StatusCode.NotFound);
            }

            var testAnswerFromDB = await _unitOfWork.TestAnswer.GetAsync(a => a.TestAnswerId == testAnswerId && a.Status != StaticOperationStatus.TestAnswer.Deleted);
            return (testAnswerFromDB is null) ?
                ErrorResponse.Build(
                    message: StaticResponseMessage.TestAnswer.NotFound,
                    statusCode: StaticOperationStatus.StatusCode.NotFound)
                :
                SuccessResponse.Build(
                    message: StaticResponseMessage.TestAnswer.Retrieved,
                    statusCode: StaticOperationStatus.StatusCode.Ok,
                    result: testAnswerFromDB);
        }

        public async Task<ResponseDto> GetTestAnswerByTestQuestionId(Guid testQuestionId)
        {
            if (await _unitOfWork.TestQuestion.GetAsync(c => c.TestQuestionId == testQuestionId) is null)
            {
                return ErrorResponse.Build(
                    message: StaticResponseMessage.User.NotFound,
                    statusCode: StaticOperationStatus.StatusCode.NotFound);
            }

            var testAnswerFromDb = await _unitOfWork.TestAnswer.GetAllAsync(a => a.TestQuestionId == testQuestionId && a.Status != StaticOperationStatus.TestAnswer.Deleted);

            return (testAnswerFromDb.Any()) ?
                SuccessResponse.Build(
                    message: StaticResponseMessage.TestAnswer.RetrievedAll,
                    statusCode: StaticOperationStatus.StatusCode.Ok,
                    result: testAnswerFromDb)
                :
                SuccessResponse.Build(
                    message: StaticResponseMessage.TestAnswer.NotFound,
                    statusCode: StaticOperationStatus.StatusCode.Ok,
                    result: new List<TestAnswer>());
        }

        public async Task<ResponseDto> UpdateTestAnswer(ClaimsPrincipal User, UpdateTestAnswerDto updateTestAnswerDto)
        {
            if (User.FindFirstValue(ClaimTypes.NameIdentifier) is null)
            {
                return ErrorResponse.Build(
                    message: StaticResponseMessage.User.NotFound,
                    statusCode: StaticOperationStatus.StatusCode.NotFound);
            }

            var testAnswerFromDb = await _unitOfWork.TestAnswer.GetAsync(a => a.TestAnswerId == updateTestAnswerDto.TestAnswerId);
            if (testAnswerFromDb is null)
            {
                return ErrorResponse.Build(
                    message: StaticResponseMessage.TestAnswer.NotFound,
                    statusCode: StaticOperationStatus.StatusCode.NotFound);
            }

            var testAnswerToUpdate = _autoMapperService.Map<UpdateTestAnswerDto, TestAnswer>(updateTestAnswerDto);
            _unitOfWork.TestAnswer.Update(testAnswerFromDb, testAnswerToUpdate);

            return (!await SaveChangesAsync()) ?
                ErrorResponse.Build(
                    message: StaticResponseMessage.TestAnswer.NotUpdated,
                    statusCode: StaticOperationStatus.StatusCode.InternalServerError)
                :
                SuccessResponse.Build(
                    message: StaticResponseMessage.TestAnswer.Updated,
                    statusCode: StaticOperationStatus.StatusCode.Ok,
                    result: testAnswerToUpdate);
        }

        private async Task<bool> SaveChangesAsync()
        {
            return await _unitOfWork.SaveAsync() == StaticOperationStatus.Database.Success;
        }
    }
}
