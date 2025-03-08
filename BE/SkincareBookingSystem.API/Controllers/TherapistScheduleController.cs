using Microsoft.AspNetCore.Mvc;
using SkincareBookingSystem.Models.Dto.BookingSchedule;
using SkincareBookingSystem.Models.Dto.Response;
using SkincareBookingSystem.Services.IServices;

namespace SkincareBookingSystem.API.Controllers
{
    [ApiController]
    [Route("api/therapist-schedules")]
    public class TherapistScheduleController : ControllerBase
    {
        private readonly ITherapistScheduleService _bookingScheduleService;

        public TherapistScheduleController(ITherapistScheduleService bookingScheduleService)
        {
            _bookingScheduleService = bookingScheduleService;
        }

        [HttpPost]
        public async Task<ActionResult<ResponseDto>> CreateTherapistSchedule([FromBody] CreateTherapistScheduleDto bookingScheduleDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new ResponseDto
                {
                    IsSuccess = false,
                    Message = "Invalid input data.",
                    Result = ModelState.Values
                        .SelectMany(v => v.Errors.Select(e => e.ErrorMessage))
                });
            }

            var result = await _bookingScheduleService.CreateTherapistSchedule(User, bookingScheduleDto);
            return StatusCode(result.StatusCode, result);
        }

        [HttpGet]
        public async Task<ActionResult<ResponseDto>> GetAllTherapistSchedules()
        {
            var result = await _bookingScheduleService.GetAllTherapistSchedules();
            return StatusCode(result.StatusCode, result);
        }

        [HttpGet("{scheduleId:guid}")]
        public async Task<ActionResult<ResponseDto>> GetTherapistScheduleById(Guid scheduleId)
        {
            var result = await _bookingScheduleService.GetTherapistScheduleById(User, scheduleId);
            return StatusCode(result.StatusCode, result);
        }

        [HttpGet("therapist/{therapistId:guid}")]
        public async Task<ActionResult<ResponseDto>> GetTherapistScheduleByTherapistId(Guid therapistId)
        {
            var result = await _bookingScheduleService.GetTherapistScheduleByTherapistId(therapistId);
            return StatusCode(result.StatusCode, result);
        }

        [HttpPut]
        public async Task<ActionResult<ResponseDto>> UpdateTherapistSchedule([FromBody] UpdateTherapistScheduleDto bookingScheduleDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new ResponseDto
                {
                    IsSuccess = false,
                    Message = "Invalid input data.",
                    Result = ModelState.Values
                        .SelectMany(v => v.Errors.Select(e => e.ErrorMessage))
                });
            }
            var result = await _bookingScheduleService.UpdateTherapistSchedule(User, bookingScheduleDto);
            return StatusCode(result.StatusCode, result);
        }

        [HttpDelete("{scheduleId:guid}")]
        public async Task<ActionResult<ResponseDto>> DeleteTherapistSchedule(Guid scheduleId)
        {
            var result = await _bookingScheduleService.DeleteTherapistSchedule(User, scheduleId);
            return StatusCode(result.StatusCode, result);
        }
    }
}
