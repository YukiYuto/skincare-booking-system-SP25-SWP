using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SkincareBookingSystem.Models.Dto.ServiceTypeDto;
using SkincareBookingSystem.Services.IServices;

namespace SkincareBookingSystem.API.Controllers
{
    [Route("api/service-type")]
    [ApiController]
    public class ServiceTypeController : ControllerBase
    {
        private readonly IServiceTypeService _serviceTypeService;

        public ServiceTypeController(IServiceTypeService serviceTypeService)
        {
            _serviceTypeService = serviceTypeService;
        }

        [HttpPost]
        public async Task<IActionResult> CreateServiceType([FromBody] CreateServiceTypeDto createServiceTypeDto)
        {
            var result = await _serviceTypeService.CreateServiceType(User ,createServiceTypeDto);
            return StatusCode(result.StatusCode, result);
        }

        [HttpGet]
        public async Task<IActionResult> GetAllServiceTypes()
        {
            var result = await _serviceTypeService.GetAllServiceTypes();
            return StatusCode(result.StatusCode, result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetServiceTypeById([FromQuery] Guid id)
        {
            var result = await _serviceTypeService.GetServiceTypeById(id);
            return StatusCode(result.StatusCode, result);
        }

        [HttpPut]
        public async Task<IActionResult> UpdateServiceType([FromBody] UpdateServiceTypeDto updateServiceTypeDto)
        {
            var result = await _serviceTypeService.UpdateServiceType(User, updateServiceTypeDto);
            return StatusCode(result.StatusCode, result);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteServiceType([FromQuery] Guid id)
        {
            var result = await _serviceTypeService.DeleteServiceType(id);
            return StatusCode(result.StatusCode, result);
        }
    }
}
