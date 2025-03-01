using Microsoft.AspNetCore.Mvc;
using SkincareBookingSystem.Models.Dto.BookingSchedule;
using SkincareBookingSystem.Services.IServices;

namespace SkincareBookingSystem.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TherapistScheduleController : ControllerBase
    {
        private readonly ITherapistScheduleService _bookingScheduleService;

        public TherapistScheduleController(ITherapistScheduleService bookingScheduleService)
        {
            _bookingScheduleService = bookingScheduleService;
        }

        [HttpPost("create")]
        public async Task<IActionResult> CreateBookingSchedule([FromBody] CreateTherapistScheduleDto createBookingScheduleDto)
        {
            var result = await _bookingScheduleService.CreateTherapistSchedule(User, createBookingScheduleDto);
            return StatusCode(result.StatusCode, result);
        }

        [HttpGet("get/{id}")]
        public async Task<IActionResult> GetBookingScheduleById(Guid id)
        {
            var result = await _bookingScheduleService.GetTherapistScheduleById(id);
            return StatusCode(result.StatusCode, result);
        }

        [HttpGet("all")]
        public async Task<IActionResult> GetAllBookingSchedules()
        {
            var result = await _bookingScheduleService.GetAllTherapistSchedules();
            return StatusCode(result.StatusCode, result);
        }

        [HttpPut("update")]
        public async Task<IActionResult> UpdateBookingSchedule([FromBody] UpdateTherapistScheduleDto updateBookingScheduleDto)
        {
            var result = await _bookingScheduleService.UpdateTherapistSchedule(User, updateBookingScheduleDto);
            return StatusCode(result.StatusCode, result);
        }

        [HttpDelete("delete/{id}")]
        public async Task<IActionResult> DeleteBookingSchedule(Guid id)
        {
            var result = await _bookingScheduleService.DeleteTherapistSchedule(User, id);
            return StatusCode(result.StatusCode, result);
        }
    }
}
