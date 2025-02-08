using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SkincareBookingSystem.Models.Dto.Services;
using SkincareBookingSystem.Services.IServices;

namespace SkincareBookingSystem.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ServicesController : ControllerBase
    {
        private readonly IServicesService _servicesService;
        public ServicesController(IServicesService servicesService)
        {
            _servicesService = servicesService;
        }

        [HttpPost("create")]
        public async Task<IActionResult> CreateService([FromBody] CreateServiceDto createServiceDto)
        {
            var result = await _servicesService.CreateService(User, createServiceDto);
            return StatusCode(result.StatusCode, result);
        }

        [HttpGet("all")]
        public async Task<IActionResult> GetAllServices()
        {
            var result = await _servicesService.GetAllServices();
            return StatusCode(result.StatusCode, result);
        }

        [HttpGet("get/{id}")]
        public async Task<IActionResult> GetServiceById(Guid id)
        {
            var result = await _servicesService.GetServiceById(id);
            return StatusCode(result.StatusCode, result);
        }

        [HttpPut("update/{id}")]
        public async Task<IActionResult> UpdateService(Guid id, [FromBody] UpdateServiceDto updateServiceDto)
        {
            updateServiceDto.ServiceId = id ; // Ensure the ID is set
            var result = await _servicesService.UpdateService(User, updateServiceDto);
            return StatusCode(result.StatusCode, result);
        }

        [HttpDelete("delete/{id}")]
        public async Task<IActionResult> DeleteService(Guid id)
        {
            var result = await _servicesService.DeleteService(id);
            return StatusCode(result.StatusCode, result);
        }
    }
}
