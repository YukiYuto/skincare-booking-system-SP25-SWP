using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SkincareBookingSystem.Models.Dto.Booking.ServiceType;
using SkincareBookingSystem.Services.IServices;

namespace SkincareBookingSystem.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SkinTherapistController : ControllerBase
    {
        private readonly ISkinTherapistService _skinTherapistService;

        public SkinTherapistController(ISkinTherapistService skinTherapistService)
        {
            _skinTherapistService = skinTherapistService;
        }

        [HttpGet("skin-therapists/{therapistId}")]
        public async Task<ActionResult> GetTherapistDetailsById(Guid therapistId)
        {
            var response = await _skinTherapistService.GetTherapistDetailsById(therapistId);
            return StatusCode(response.StatusCode, response);
        }

        [HttpGet("skin-therapists")]
        public async Task<ActionResult> GetAllTherapists()
        {
            var response = await _skinTherapistService.GetAllTherapists();
            return StatusCode(response.StatusCode, response);
        }

        [HttpGet("skin-therapists/service")]
        public async Task<ActionResult> GetTherapistsByServiceType([FromBody] ServiceTypeDto serviceTypeDto)
        {
            var response = await _skinTherapistService.GetTherapistsByServiceType(serviceTypeDto);
            return StatusCode(response.StatusCode, response);
        }
    }
}
