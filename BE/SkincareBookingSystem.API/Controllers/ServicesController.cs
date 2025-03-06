using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SkincareBookingSystem.Models.Dto.Services;
using SkincareBookingSystem.Services.IServices;
using Swashbuckle.AspNetCore.Annotations;

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
        [SwaggerOperation(Summary = "API tạo Service mới")]
        public async Task<IActionResult> CreateService([FromBody] CreateServiceDto createServiceDto)
        {
            var result = await _servicesService.CreateService(User, createServiceDto);
            return StatusCode(result.StatusCode, result);
        }

        [HttpGet("all")]
        [SwaggerOperation(Summary = "API lấy tất cả Services có sẵn")]
        public async Task<IActionResult> GetAllServices()
        {
            var result = await _servicesService.GetAllServices();
            return StatusCode(result.StatusCode, result);
        }

        [HttpGet("get/{id}")]
        [SwaggerOperation(Summary = "API lấy một Service bằng ServiceID")]
        public async Task<IActionResult> GetServiceById(Guid id)
        {
            var result = await _servicesService.GetServiceById(id);
            return StatusCode(result.StatusCode, result);
        }

        [HttpPut("update")]
        [SwaggerOperation(Summary = "API cập nhật một Service")]
        public async Task<IActionResult> UpdateService([FromBody] UpdateServiceDto updateServiceDto)
        {
            var result = await _servicesService.UpdateService(User, updateServiceDto);
            return StatusCode(result.StatusCode, result);
        }

        [HttpDelete("delete/{id}")]
        [SwaggerOperation(Summary = "API soft delete một Service bằng ServiceID")]
        public async Task<IActionResult> DeleteService(Guid id)
        {
            var result = await _servicesService.DeleteService(id);
            return StatusCode(result.StatusCode, result);
        }
    }
}
