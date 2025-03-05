using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SkincareBookingSystem.Models.Dto.Services;
using SkincareBookingSystem.Services.IServices;
using SkincareBookingSystem.Utilities.Constants;

namespace SkincareBookingSystem.API.Controllers
{
    [Route("api/services")]
    [ApiController]
    public class ServicesController : ControllerBase
    {
        private readonly IServicesService _servicesService;

        public ServicesController(IServicesService servicesService)
        {
            _servicesService = servicesService;
        }

        [HttpPost]
        [Authorize(Roles = StaticUserRoles.Manager)]
        public async Task<IActionResult> CreateService([FromBody] CreateServiceDto createServiceDto)
        {
            var result = await _servicesService.CreateService(User, createServiceDto);
            return StatusCode(result.StatusCode, result);
        }

        [HttpGet]
        public async Task<IActionResult> GetAllServices
        (
            [FromQuery] int pageNumber = 1,
            int pageSize = 10,
            string? filterOn = null,
            string? filterQuery = null,
            string? sortBy = null
        )
        {
            var result = await _servicesService.GetAllServices
                (User, pageNumber, pageSize, filterOn, filterQuery, sortBy);
            return StatusCode(result.StatusCode, result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetServiceById(Guid id)
        {
            var result = await _servicesService.GetServiceById(id);
            return StatusCode(result.StatusCode, result);
        }

        [HttpPut()]
        [Authorize(Roles = StaticUserRoles.Manager)]
        public async Task<IActionResult> UpdateService([FromBody] UpdateServiceDto updateServiceDto)
        {
            var result = await _servicesService.UpdateService(User, updateServiceDto);
            return StatusCode(result.StatusCode, result);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteService(Guid id)
        {
            var result = await _servicesService.DeleteService(id);
            return StatusCode(result.StatusCode, result);
        }
    }
}