using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SkincareBookingSystem.Models.Dto.Feedbacks;
using SkincareBookingSystem.Models.Dto.Response;
using SkincareBookingSystem.Services.IServices;
using SkincareBookingSystem.Utilities.Constants;
using Swashbuckle.AspNetCore.Annotations;
using System.Security.Claims;

namespace SkincareBookingSystem.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FeedbackController : ControllerBase
    {
        private readonly IFeedbackService _feedbackService;
        public FeedbackController(IFeedbackService feedbackService)
        {
            _feedbackService = feedbackService;
        }

        [HttpPost("create")]
        [SwaggerOperation(Summary = "API creates a new feedback", Description = "Requires customer, staff roles")]
        public async Task<ActionResult<ResponseDto>> CreateFeedback([FromBody] CreateFeedbackDto createFeedbackDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new ResponseDto
                {
                    IsSuccess = false,
                    Message = "Invalid input data.",
                    Result = ModelState.Values.SelectMany(v => v.Errors.Select(e => e.ErrorMessage))
                });
            }
            var result = await _feedbackService.CreateFeedback(User, createFeedbackDto);
            return StatusCode(result.StatusCode, result);
        }

        [HttpGet("all")]
        [SwaggerOperation(Summary = "API retrieves all feedbacks", Description = "Requires customer, staff roles")]
        public async Task<ActionResult<ResponseDto>> GetAllFeedbacks()
        {
            var result = await _feedbackService.GetAllFeedbacks();
            return StatusCode(result.StatusCode, result);
        }

        [HttpGet("appointment/{appointmentId}")]
        [SwaggerOperation(Summary = "API retrieves a feedback by Appointment ID", Description = "Requires customer, staff roles")]
        public async Task<ActionResult<ResponseDto>> GetFeedbackByAppointmentId(Guid appointmentId)
        {
            var result = await _feedbackService.GetFeedbackByAppointmentId(appointmentId);
            return StatusCode(result.StatusCode, result);
        }

        [HttpGet("{feedbackId}")]
        [SwaggerOperation(Summary = "API retrieves a feedback by Feedback ID", Description = "Requires customer, staff roles")]
        public async Task<ActionResult<ResponseDto>> GetFeedbackById(Guid feedbackId)
        {
            var result = await _feedbackService.GetFeedbackById(User, feedbackId);
            return StatusCode(result.StatusCode, result);
        }

        [HttpPut("update")]
        [SwaggerOperation(Summary = "API updates a feedback", Description = "Requires customer, staff roles")]
        public async Task<ActionResult<ResponseDto>> UpdateFeedback([FromBody] UpdateFeedbackDto updateFeedbackDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new ResponseDto
                {
                    IsSuccess = false,
                    Message = "Invalid input data.",
                    Result = ModelState.Values.SelectMany(v => v.Errors.Select(e => e.ErrorMessage))
                });
            }
            var result = await _feedbackService.UpdateFeedback(User, updateFeedbackDto);
            return StatusCode(result.StatusCode, result);
        }

        [HttpDelete("delete/{feedbackId}")]
        [Authorize(Roles = StaticUserRoles.ManagerStaff)]
        [SwaggerOperation(Summary = "API deletes a feedback", Description = "Requires manager, staff roles")]
        public async Task<ActionResult<ResponseDto>> DeleteFeedback(Guid feedbackId)
        {
            var result = await _feedbackService.DeleteFeedback(User, feedbackId);
            return StatusCode(result.StatusCode, result);
        }
    }
}
