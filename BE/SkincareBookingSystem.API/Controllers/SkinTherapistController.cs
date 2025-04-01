using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SkincareBookingSystem.Models.Dto.Booking.ServiceType;
using SkincareBookingSystem.Services.IServices;
using Swashbuckle.AspNetCore.Annotations;

namespace SkincareBookingSystem.API.Controllers
{
    [Route("api/therapists")]
    [ApiController]
    public class SkinTherapistController : ControllerBase
    {
        private readonly ISkinTherapistService _skinTherapistService;

        public SkinTherapistController(ISkinTherapistService skinTherapistService)
        {
            _skinTherapistService = skinTherapistService;
        }

        [HttpGet("details/{therapistId}")]
        [SwaggerOperation(Summary = "API gets a therapist detail by id", Description = "Requires admin role")]
        public async Task<ActionResult> GetTherapistDetailsById(Guid therapistId)
        {
            var response = await _skinTherapistService.GetTherapistDetailsById(therapistId);
            return StatusCode(response.StatusCode, response);
        }

        [HttpGet]
        [SwaggerOperation(Summary = "API gets all therapists", Description = "Requires admin role")]
        public async Task<ActionResult> GetAllTherapists()
        {
            var response = await _skinTherapistService.GetAllTherapists();
            return StatusCode(response.StatusCode, response);
        }

        [HttpGet("service-type/{serviceTypeId}")]
        [SwaggerOperation(Summary = "API gets therapists by service type", Description = "Requires serviceTypeId")]
        public async Task<ActionResult> GetTherapistsByServiceType(Guid serviceTypeId)
        {
            var response = await _skinTherapistService.GetTherapistsByServiceTypeId(serviceTypeId);
            return StatusCode(response.StatusCode, response);
        }

        [HttpGet("service/{serviceId}")]
        [SwaggerOperation(Summary = "API gets therapists by service", Description = "Requires serviceId")]
        public async Task<ActionResult> GetTherapistsByServiceId(Guid serviceId)
        {
            var response = await _skinTherapistService.GetTherapistsByServiceId(serviceId);
            return StatusCode(response.StatusCode, response);
        }

        [HttpGet("today-appointments")]
        [SwaggerOperation(Summary = "API gets therapists appointments for today", Description = "Requires therapist role")]
        public async Task<ActionResult> GetTherapistTodayAppointments()
        {
            var response = await _skinTherapistService.GetTherapistTodayAppointments(User);
            return StatusCode(response.StatusCode, response);
        }

    }
}
