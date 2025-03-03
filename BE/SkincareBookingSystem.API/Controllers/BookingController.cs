using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SkincareBookingSystem.Models.Dto.Booking.Order;
using SkincareBookingSystem.Services.IServices;

namespace SkincareBookingSystem.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BookingController : ControllerBase
    {
        private readonly IBookingService _bookingService;
        public BookingController(IBookingService bookingService)
        {
            _bookingService = bookingService;
        }

        [HttpPost("order/bundle")]
        public async Task<IActionResult> BundleOrder([FromBody] BundleOrderDto bundleOrderDto)
        {
            var result = await _bookingService.BundleOrder(bundleOrderDto, User);
            return StatusCode(result.StatusCode, result);
        }

        [HttpGet("therapists")]
        public async Task<IActionResult> GetTherapistsForServiceType(Guid serviceTypeId)
        {
            var result = await _bookingService.GetTherapistsForServiceType(serviceTypeId);
            return StatusCode(result.StatusCode, result);
        }

        [HttpGet("occupied-slots")]
        public async Task<IActionResult> GetOccupiedSlotsFromTherapist(Guid therapistId, DateTime date)
        {
            var result = await _bookingService.GetOccupiedSlotsFromTherapist(therapistId, date);
            return StatusCode(result.StatusCode, result);
        }
    }
}
