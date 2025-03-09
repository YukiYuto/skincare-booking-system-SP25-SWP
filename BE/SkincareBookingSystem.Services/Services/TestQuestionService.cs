using SkincareBookingSystem.DataAccess.IRepositories;
using SkincareBookingSystem.Models.Domain;
using SkincareBookingSystem.Models.Dto.Response;
using SkincareBookingSystem.Models.Dto.TestQuestion;
using SkincareBookingSystem.Services.Helpers.Responses;
using SkincareBookingSystem.Services.IServices;
using SkincareBookingSystem.Utilities.Constants;
using System.Security.Claims;
using static SkincareBookingSystem.Utilities.Constants.StaticOperationStatus;

namespace SkincareBookingSystem.Services.Services
{
    public class TestQuestionService : ITestQuestionService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IAutoMapperService _autoMapperService;

        public TestQuestionService(IUnitOfWork unitOfWork, IAutoMapperService autoMapperService)
        {
            _unitOfWork = unitOfWork;
            _autoMapperService = autoMapperService;
        }

        public async Task<ResponseDto> CreateTestQuestion(ClaimsPrincipal User, CreateTestQuestionDto testQuestionDto)
        {
            if (User.FindFirstValue(ClaimTypes.NameIdentifier) is null)
            {
                return ErrorResponse.Build(
                    message: StaticResponseMessage.User.NotFound,
                    statusCode: StaticOperationStatus.StatusCode.NotFound);
            }

            var testQuestionToCreate = _autoMapperService.Map<CreateTestQuestionDto, Models.Domain.TestQuestion>(testQuestionDto);
            //testQuestionToCreate.CreatedBy = user.FindFirstValue("FullName");

            try
            {
                await _unitOfWork.TestQuestion.AddAsync(testQuestionToCreate);
                await _unitOfWork.SaveAsync();
            }
            catch (Exception e)
            {
                return ErrorResponse.Build(
                    message: StaticResponseMessage.TestQuestion.NotCreated,
                    statusCode: StaticOperationStatus.StatusCode.InternalServerError);
            }
            return SuccessResponse.Build(
                message: StaticResponseMessage.TestQuestion.Created,
                statusCode: StaticOperationStatus.StatusCode.Ok,
                result: testQuestionToCreate);
        }

        public async Task<ResponseDto> GetAllTestQuestions()
        {
            var testQuestionsFromDb = await _unitOfWork.Appointments.GetAllAsync();

            return (testQuestionsFromDb.Any()) ?
                SuccessResponse.Build(
                    message: StaticResponseMessage.TestQuestion.RetrievedAll,
                    statusCode: StaticOperationStatus.StatusCode.Ok,
                    result: testQuestionsFromDb)
                :
                SuccessResponse.Build(
                    message: StaticResponseMessage.TestQuestion.NotFound,
                    statusCode: StaticOperationStatus.StatusCode.Ok,
                    result: new List<Models.Domain.TestQuestion>());
        }

        public async Task<ResponseDto> GetTestQuestionById(ClaimsPrincipal User, Guid testQuestionId)
        {
            if (User.FindFirstValue(ClaimTypes.NameIdentifier) is null)
            {
                return ErrorResponse.Build(
                    message: StaticResponseMessage.User.NotFound,
                    statusCode: StaticOperationStatus.StatusCode.NotFound);
            }

            var testQuestionFromDB = await _unitOfWork.TestQuestion.GetAsync(a => a.TestQuestionId == testQuestionId);
            return (testQuestionFromDB is null) ?
                ErrorResponse.Build(
                    message: StaticResponseMessage.TestQuestion.NotFound,
                    statusCode: StaticOperationStatus.StatusCode.NotFound)
                :
                SuccessResponse.Build(
                    message: StaticResponseMessage.TestQuestion.Retrieved,
                    statusCode: StaticOperationStatus.StatusCode.Ok,
                    result: testQuestionFromDB);
        }

        //public async Task<ResponseDto> GetTestQuestionByStaffId(Guid staffId)
        //{
        //    if (await _unitOfWork.Staff.GetAsync(c => c.StaffId == staffId) is null)
        //    {
        //        return ErrorResponse.Build(
        //            message: StaticResponseMessage.User.NotFound,
        //        statusCode: StaticOperationStatus.StatusCode.NotFound);
        //    }

        //    var testQuestionsFromDb = await _unitOfWork.TestQuestion.GetAllAsync(a => a.SkinTestId == staffId); 

        //    return (testQuestionsFromDb.Any()) ?
        //        SuccessResponse.Build(
        //            message: StaticResponseMessage.TestQuestion.RetrievedAll,
        //            statusCode: StaticOperationStatus.StatusCode.Ok,
        //            result: testQuestionsFromDb)
        //        :
        //        SuccessResponse.Build(
        //            message: StaticResponseMessage.TestQuestion.NotFound,
        //            statusCode: StaticOperationStatus.StatusCode.Ok,
        //            result: new List<Models.Domain.TestQuestion>());
        //}

        public async Task<ResponseDto> UpdateTestQuestion(ClaimsPrincipal User, UpdateTestQuestionDto testQuestionDto)
        {
            if (User.FindFirstValue(ClaimTypes.NameIdentifier) is null)
            {
                return ErrorResponse.Build(
                    message: StaticResponseMessage.User.NotFound,
                    statusCode: StaticOperationStatus.StatusCode.NotFound);
            }

            var testQuestionFromDb = await _unitOfWork.TestQuestion.GetAsync(a => a.TestQuestionId == testQuestionDto.TestQuestionId);
            if (testQuestionFromDb is null)
            {
                return ErrorResponse.Build(
                    message: StaticResponseMessage.Appointment.NotFound,
                    statusCode: StaticOperationStatus.StatusCode.NotFound);
            }

            var testQuestionToUpdate = _autoMapperService.Map<UpdateTestQuestionDto, Models.Domain.TestQuestion>(testQuestionDto);
            _unitOfWork.TestQuestion.Update(testQuestionFromDb, testQuestionToUpdate);

            return (!await SaveChangesAsync()) ?
                ErrorResponse.Build(
                    message: StaticResponseMessage.TestQuestion.NotUpdated,
                    statusCode: StaticOperationStatus.StatusCode.InternalServerError)
                :
                SuccessResponse.Build(
                    message: StaticResponseMessage.TestQuestion.Updated,
                    statusCode: StaticOperationStatus.StatusCode.Ok,
                    result: testQuestionToUpdate);
        }

        public async Task<ResponseDto> DeleteTestQuestion(ClaimsPrincipal User, Guid testQuestionId)
        {
            if (User.FindFirstValue(ClaimTypes.NameIdentifier) is null)
            {
                return ErrorResponse.Build(
                    message: StaticResponseMessage.User.NotFound,
                    statusCode: StaticOperationStatus.StatusCode.NotFound);
            }
            var testQuestionToDelete = await _unitOfWork.TestQuestion.GetAsync(a => a.TestQuestionId == testQuestionId);
            if (testQuestionToDelete is null)
            {
                return ErrorResponse.Build(
                    message: StaticResponseMessage.Appointment.NotFound,
                    statusCode: StaticOperationStatus.StatusCode.NotFound);
            }

            //testQuestionToDelete.Status = StaticOperationStatus.Appointment.Deleted;
            //testQuestionToDelete.UpdatedTime = StaticOperationStatus.Timezone.Vietnam;
            //testQuestionToDelete.UpdatedBy = User.FindFirstValue("FullName");

            return (await SaveChangesAsync()) ?
                SuccessResponse.Build(
                    message: StaticResponseMessage.TestQuestion.Deleted,
                    statusCode: StaticOperationStatus.StatusCode.Ok,
                    result: testQuestionToDelete)
                :
                ErrorResponse.Build(
                    message: StaticResponseMessage.TestQuestion.NotDeleted,
                    statusCode: StaticOperationStatus.StatusCode.InternalServerError);
        }

        private async Task<bool> SaveChangesAsync()
        {
            return await _unitOfWork.SaveAsync() == StaticOperationStatus.Database.Success;
        }
    }
}