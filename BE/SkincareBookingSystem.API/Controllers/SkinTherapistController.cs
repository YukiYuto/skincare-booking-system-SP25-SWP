using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SkincareBookingSystem.Models.Dto.Booking.ServiceType;
using SkincareBookingSystem.Services.IServices;

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
        public async Task<ActionResult> GetTherapistDetailsById(Guid therapistId)
        {
            var response = await _skinTherapistService.GetTherapistDetailsById(therapistId);
            return StatusCode(response.StatusCode, response);
        }

        [HttpGet]
        public async Task<ActionResult> GetAllTherapists()
        {
            var response = await _skinTherapistService.GetAllTherapists();
            return StatusCode(response.StatusCode, response);
        }

        [HttpGet("service/{serviceTypeId}")]
        public async Task<ActionResult> GetTherapistsByServiceType(Guid serviceTypeId)
        {
            var response = await _skinTherapistService.GetTherapistsByServiceTypeId(serviceTypeId);
            return StatusCode(response.StatusCode, response);
        }
    }
}
