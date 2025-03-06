using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SkincareBookingSystem.Models.Dto.TherapistServiceTypes;
using SkincareBookingSystem.Services.IServices;

namespace SkincareBookingSystem.API.Controllers
{
    [Route("api/therapist-service-type")]
    [ApiController]
    public class TherapistServiceTypeController : ControllerBase
    {
        private readonly ITherapistServiceTypeService _therapistServiceTypeService;
        public TherapistServiceTypeController(ITherapistServiceTypeService therapistServiceTypeService)
        {
            _therapistServiceTypeService = therapistServiceTypeService;
        }

        [HttpPost]
        public async Task<IActionResult> AssignServiceTypeToTherapist([FromBody] TherapistServiceTypesDto therapistServiceTypesDto)
        {
            var response = await _therapistServiceTypeService.AssignServiceTypesToTherapist(User, therapistServiceTypesDto);
            return StatusCode(response.StatusCode, response);
        }

        [HttpDelete]
        public async Task<IActionResult> RemoveServiceTypeFromTherapist([FromBody] TherapistServiceTypesDto therapistServiceTypesDto)
        {
            var response = await _therapistServiceTypeService.RemoveServiceTypesForTherapist(User, therapistServiceTypesDto);
            return StatusCode(response.StatusCode, response);
        }
    }
}
