using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SkincareBookingSystem.Models.Dto.Booking.Appointment;
using SkincareBookingSystem.Models.Dto.Booking.Appointment.RescheduleAppointment;
using SkincareBookingSystem.Models.Dto.Booking.Order;
using SkincareBookingSystem.Services.IServices;
using Swashbuckle.AspNetCore.Annotations;

namespace SkincareBookingSystem.API.Controllers
{
    [Route("api/bookings")]
    [ApiController]
    public class BookingController : ControllerBase
    {
        private readonly IBookingService _bookingService;
        public BookingController(IBookingService bookingService)
        {
            _bookingService = bookingService;
        }

        [HttpPost("orders-bundles")]
        [SwaggerOperation(Summary = "API creates a new bundle order", Description = "Requires user role")]
        public async Task<IActionResult> BundleOrder([FromBody] BundleOrderDto bundleOrderDto)
        {
            var result = await _bookingService.BundleOrder(bundleOrderDto, User);
            return StatusCode(result.StatusCode, result);
        }

        [HttpPost("appointment-schedule")]
        public async Task<ActionResult> FinalizeAppointment([FromBody] BookAppointmentDto bookingRequest)
        {
            var result = await _bookingService.FinalizeAppointment(bookingRequest, User);
            return StatusCode(result.StatusCode, result);
        }

        [HttpPut("rescheduling")]
        public async Task<IActionResult> RescheduleAppointment([FromBody] RescheduleAppointmentDto rescheduleRequest)
        {
            var result = await _bookingService.RescheduleAppointment(rescheduleRequest, User);
            return StatusCode(result.StatusCode, result);
        }

        [HttpDelete("cancellation/{appointmentId}")]
        public async Task<ActionResult> CancelAppointment(Guid appointmentId)
        {
            var result = await _bookingService.CancelAppointment(appointmentId, User);
            return StatusCode(result.StatusCode, result);
        }

        [HttpGet("therapists")]
        [SwaggerOperation(Summary = "API gets therapists for a service type", Description = "Requires serviceTypeId")]
        public async Task<IActionResult> GetTherapistsForServiceType(Guid serviceTypeId)
        {
            var result = await _bookingService.GetTherapistsForServiceType(serviceTypeId);
            return StatusCode(result.StatusCode, result);
        }

        [HttpGet("occupied-slots")]
        [SwaggerOperation(Summary = "API gets occupied slots from a therapist", Description = "Requires therapistId and date")]
        public async Task<IActionResult> GetOccupiedSlotsFromTherapist(Guid therapistId, DateOnly date)
        {
            var result = await _bookingService.GetOccupiedSlotsFromTherapist(therapistId, date);
            return StatusCode(result.StatusCode, result);
        }

        [HttpPost("auto-assign")]
        public async Task<IActionResult> HandleTherapistAutoAssignment([FromBody] AutoAssignmentDto autoAssignmentDto)
        {
            var result = await _bookingService.HandleTherapistAutoAssignment(autoAssignmentDto);
            return StatusCode(result.StatusCode, result);
        }
    }
}
