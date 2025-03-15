using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SkincareBookingSystem.Models.Dto.ServiceTypeDto;
using SkincareBookingSystem.Services.IServices;
using Swashbuckle.AspNetCore.Annotations;

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
        [SwaggerOperation(Summary = "API creates a ServiceType", Description = "Requires admin role")]
        public async Task<IActionResult> CreateServiceType([FromBody] CreateServiceTypeDto createServiceTypeDto)
        {
            var result = await _serviceTypeService.CreateServiceType(User, createServiceTypeDto);
            return StatusCode(result.StatusCode, result);
        }

        [HttpPost("bulk")]
        [SwaggerOperation(Summary = "API creates multiple ServiceTypes", Description = "Requires admin role")]
        public async Task<IActionResult> CreateBulkServiceTypes([FromBody] List<CreateServiceTypeDto> createServiceTypeDtos)
        {
            var result = await _serviceTypeService.CreateBulkServiceTypes(User, createServiceTypeDtos);
            return StatusCode(result.StatusCode, result);
        }

        [HttpGet]
        [SwaggerOperation(Summary = "API gets all ServiceTypes", Description = "Requires admin role")]
        public async Task<IActionResult> GetAllServiceTypes()
        {
            var result = await _serviceTypeService.GetAllServiceTypes();
            return StatusCode(result.StatusCode, result);
        }

        [HttpGet("{id}")]
        [SwaggerOperation(Summary = "API gets a ServiceType by id", Description = "Requires admin role")]
        public async Task<IActionResult> GetServiceTypeById([FromQuery] Guid id)
        {
            var result = await _serviceTypeService.GetServiceTypeById(id);
            return StatusCode(result.StatusCode, result);
        }

        [HttpPut]
        [SwaggerOperation(Summary = "API updates a ServiceType", Description = "Requires admin role")]
        public async Task<IActionResult> UpdateServiceType([FromBody] UpdateServiceTypeDto updateServiceTypeDto)
        {
            var result = await _serviceTypeService.UpdateServiceType(User, updateServiceTypeDto);
            return StatusCode(result.StatusCode, result);
        }

        [HttpDelete("{id}")]
        [SwaggerOperation(Summary = "API soft deletes a ServiceType", Description = "Requires admin role")]
        public async Task<IActionResult> DeleteServiceType([FromQuery] Guid id)
        {
            var result = await _serviceTypeService.DeleteServiceType(id);
            return StatusCode(result.StatusCode, result);
        }
    }
}
