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
            var result = await _bookingScheduleService.CreateBookingSchedule(User, createBookingScheduleDto);
            return StatusCode(result.StatusCode, result);
        }

        [HttpGet("get/{id}")]
        public async Task<IActionResult> GetBookingScheduleById(Guid id)
        {
            var result = await _bookingScheduleService.GetBookingScheduleById(id);
            return StatusCode(result.StatusCode, result);
        }

        [HttpGet("all")]
        public async Task<IActionResult> GetAllBookingSchedules()
        {
            var result = await _bookingScheduleService.GetAllBookingSchedules();
            return StatusCode(result.StatusCode, result);
        }

        [HttpPut("update")]
        public async Task<IActionResult> UpdateBookingSchedule([FromBody] UpdateTherapistScheduleDto updateBookingScheduleDto)
        {
            var result = await _bookingScheduleService.UpdateBookingSchedule(User, updateBookingScheduleDto);
            return StatusCode(result.StatusCode, result);
        }

        [HttpDelete("delete/{id}")]
        public async Task<IActionResult> DeleteBookingSchedule(Guid id)
        {
            var result = await _bookingScheduleService.DeleteBookingSchedule(id);
            return StatusCode(result.StatusCode, result);
        }
    }
}
