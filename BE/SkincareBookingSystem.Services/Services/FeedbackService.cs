using SkincareBookingSystem.DataAccess.IRepositories;
using SkincareBookingSystem.Models.Domain;
using SkincareBookingSystem.Models.Dto.Feedbacks;
using SkincareBookingSystem.Models.Dto.Response;
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
    public class FeedbackService : IFeedbackService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IAutoMapperService _autoMapperService;

        public FeedbackService(IUnitOfWork unitOfWork, IAutoMapperService autoMapperService)
        {
            _unitOfWork = unitOfWork;
            _autoMapperService = autoMapperService;
        }

        public async Task<ResponseDto> CreateFeedback(ClaimsPrincipal User, CreateFeedbackDto feedbackDto)
        {
            if (User.FindFirstValue(ClaimTypes.NameIdentifier) is null)
            {
                return ErrorResponse.Build(
                    message: StaticResponseMessage.User.NotFound,
                    statusCode: StaticOperationStatus.StatusCode.NotFound);
            }
            var feedbackToCreate = _autoMapperService.Map<CreateFeedbackDto, Feedbacks>(feedbackDto);
            feedbackToCreate.CreatedBy = User.FindFirstValue("FullName");
            try
            {
                await _unitOfWork.Feedbacks.AddAsync(feedbackToCreate);
                await _unitOfWork.SaveAsync();
            }
            catch (Exception e)
            {
                return ErrorResponse.Build(
                    message: StaticResponseMessage.Feedback.NotCreated,
                    statusCode: StaticOperationStatus.StatusCode.InternalServerError);
            }
            return SuccessResponse.Build(
                message: StaticResponseMessage.Feedback.Created,
                statusCode: StaticOperationStatus.StatusCode.Ok,
                result: feedbackToCreate);
        }

        public async Task<ResponseDto> GetAllFeedbacks()
        {
            var feedbacks = await _unitOfWork.Feedbacks.GetAllAsync();
            return (feedbacks.Any()) ?
                SuccessResponse.Build(
                    message: StaticResponseMessage.Feedback.RetrievedAll,
                    statusCode: StaticOperationStatus.StatusCode.Ok,
                    result: feedbacks)
                :
                SuccessResponse.Build(
                    message: StaticResponseMessage.Feedback.NotFound,
                    statusCode: StaticOperationStatus.StatusCode.NotFound,
                    result: new List<Feedbacks>());
        }

        public async Task<ResponseDto> GetFeedbackById(ClaimsPrincipal User, Guid feedbackId)
        {
            if (User.FindFirstValue(ClaimTypes.NameIdentifier) is null)
            {
                return ErrorResponse.Build(
                    message: StaticResponseMessage.Feedback.NotFound,
                    statusCode: StaticOperationStatus.StatusCode.NotFound);
            }

            var feedback = await _unitOfWork.Feedbacks.GetAsync(f => f.FeedbackId == feedbackId);
            return (feedback is null) ? 
                ErrorResponse.Build(
                    message: StaticResponseMessage.Feedback.NotFound,
                    statusCode: StaticOperationStatus.StatusCode.NotFound)
                :
                SuccessResponse.Build(
                    message: StaticResponseMessage.Feedback.Retrieved,
                    statusCode: StaticOperationStatus.StatusCode.Ok,
                    result: feedback);
        }

        public async Task<ResponseDto> GetFeedbackByAppointmentId(Guid appointmentId)
        {
            if (await _unitOfWork.Feedbacks.GetAsync(f => f.AppointmentId == appointmentId) is null)
            {
                return ErrorResponse.Build(
                    message: StaticResponseMessage.Feedback.NotFound,
                    statusCode: StaticOperationStatus.StatusCode.NotFound);
            }

            var feedbacks = await _unitOfWork.Feedbacks.GetAllAsync(f => f.AppointmentId == appointmentId);

            return (feedbacks.Any()) ?
                SuccessResponse.Build(
                    message: StaticResponseMessage.Feedback.RetrievedAll,
                    statusCode: StaticOperationStatus.StatusCode.Ok,
                    result: feedbacks)
                :
                SuccessResponse.Build(
                    message: StaticResponseMessage.Feedback.NotFound,
                    statusCode: StaticOperationStatus.StatusCode.Ok,
                    result: new List<Feedbacks>());
        }

        public async Task<ResponseDto> UpdateFeedback(ClaimsPrincipal User, UpdateFeedbackDto updateFeedbackDto)
        {
            if (User.FindFirstValue(ClaimTypes.NameIdentifier) is null)
            {
                return ErrorResponse.Build(
                    message: StaticResponseMessage.User.NotFound,
                    statusCode: StaticOperationStatus.StatusCode.NotFound);
            }

            var feedbackFromDb = await _unitOfWork.Feedbacks.GetAsync(f => f.FeedbackId == updateFeedbackDto.FeedbackId);
            if (feedbackFromDb is null)
            {
                return ErrorResponse.Build(
                    message: StaticResponseMessage.Feedback.NotFound,
                    statusCode: StaticOperationStatus.StatusCode.NotFound);
            }

            var feedbackToUpdate = _autoMapperService.Map<UpdateFeedbackDto, Feedbacks>(updateFeedbackDto);
            _unitOfWork.Feedbacks.Update(feedbackFromDb, feedbackToUpdate);

            return (!await SaveChangesAsync()) ?
                ErrorResponse.Build(
                    message: StaticResponseMessage.Feedback.NotUpdated,
                    statusCode: StaticOperationStatus.StatusCode.InternalServerError)
                :
                SuccessResponse.Build(
                message: StaticResponseMessage.Feedback.Updated,
                statusCode: StaticOperationStatus.StatusCode.Ok,
                result: feedbackToUpdate);
        }

        public async Task<ResponseDto> DeleteFeedback (ClaimsPrincipal User, Guid feedbackId)
        {
            //if (User.FindFirstValue(ClaimTypes.NameIdentifier) is null)
            //{
            //    return ErrorResponse.Build(
            //        message: StaticResponseMessage.User.NotFound,
            //        statusCode: StaticOperationStatus.StatusCode.NotFound);
            //}
            //var userRole = User.FindFirst(ClaimTypes.Role)?.Value;
            ////if(userRole is not "Manager" or "Staff")
            //{
            //    return ErrorResponse.Build(
            //        message: StaticResponseMessage.User.Unauthorized,
            //        statusCode: StaticOperationStatus.StatusCode.Unauthorized);
            //}
            var feedbackFromDb = await _unitOfWork.Feedbacks.GetAsync(f => f.FeedbackId == feedbackId);
            if (feedbackFromDb is null)
            {
                return ErrorResponse.Build(
                    message: StaticResponseMessage.Feedback.NotFound,
                    statusCode: StaticOperationStatus.StatusCode.NotFound);
            }

            feedbackFromDb.Status = StaticOperationStatus.Feedback.Deleted;
            feedbackFromDb.UpdatedTime = StaticOperationStatus.Timezone.Vietnam;
            feedbackFromDb.UpdatedBy = User.FindFirstValue("FullName");

            _unitOfWork.Feedbacks.Remove(feedbackFromDb);
            return (await SaveChangesAsync()) ?
                ErrorResponse.Build(
                    message: StaticResponseMessage.Feedback.NotDeleted,
                    statusCode: StaticOperationStatus.StatusCode.InternalServerError)
                :
                SuccessResponse.Build(
                    message: StaticResponseMessage.Feedback.Deleted,
                    statusCode: StaticOperationStatus.StatusCode.Ok,
                    result: feedbackFromDb);
        }

        private async Task<bool> SaveChangesAsync()
        {
            return await _unitOfWork.SaveAsync() == StaticOperationStatus.Database.Success;
        }

    }
}
