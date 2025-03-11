using SkincareBookingSystem.Models.Dto.Feedbacks;
using SkincareBookingSystem.Models.Dto.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace SkincareBookingSystem.Services.IServices
{
    public interface IFeedbackService
    {
        Task<ResponseDto> CreateFeedback(ClaimsPrincipal User, CreateFeedbackDto feedbackDto);
        Task<ResponseDto> UpdateFeedback(ClaimsPrincipal User, UpdateFeedbackDto feedbackDto);
        Task<ResponseDto> DeleteFeedback(ClaimsPrincipal User, Guid feedbackId);
        Task<ResponseDto> GetFeedbackById(ClaimsPrincipal User, Guid feedbackId);
        Task<ResponseDto> GetAllFeedbacks();
        Task<ResponseDto> GetFeedbackByAppointmentId(Guid appointmentId);
    }
}
