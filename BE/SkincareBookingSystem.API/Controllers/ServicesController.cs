using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SkincareBookingSystem.Models.Dto.Services;
using SkincareBookingSystem.Services.IServices;
using SkincareBookingSystem.Utilities.Constants;
using Swashbuckle.AspNetCore.Annotations;

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
        [SwaggerOperation(Summary = "API create a new Service", Description = "Requires admin role")]
        [Authorize(Roles = StaticUserRoles.Manager)]
        public async Task<IActionResult> CreateService([FromBody] CreateServiceDto createServiceDto)
        {
            var result = await _servicesService.CreateService(User, createServiceDto);
            return StatusCode(result.StatusCode, result);
        }

        [HttpPost("bulk")]
        [SwaggerOperation(Summary = "API create multiple Services", Description = "Requires admin role")]
        [Authorize(Roles = StaticUserRoles.Manager)]
        public async Task<IActionResult> CreateBulkServices([FromBody] List<CreateServiceDto> createServiceDtos)
        {
            var result = await _servicesService.CreateBulkServices(User, createServiceDtos);
            return StatusCode(result.StatusCode, result);
        }

        [HttpGet]
        [SwaggerOperation(Summary = "API get all Services", Description = "Requires admin role")]
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
        [SwaggerOperation(Summary = "API get a Service by id", Description = "Requires admin role")]
        public async Task<IActionResult> GetServiceById(Guid id)
        {
            var result = await _servicesService.GetServiceById(id);
            return StatusCode(result.StatusCode, result);
        }

        [HttpPut()]
        [Authorize(Roles = StaticUserRoles.Manager)]
        [SwaggerOperation(Summary = "API update a Service", Description = "Requires admin and manager roles")]
        public async Task<IActionResult> UpdateService([FromBody] UpdateServiceDto updateServiceDto)
        {
            var result = await _servicesService.UpdateService(User, updateServiceDto);
            return StatusCode(result.StatusCode, result);
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = StaticUserRoles.Manager)]
        [SwaggerOperation(Summary = "API soft delete a Service", Description = "Requires admin role")]
        public async Task<IActionResult> DeleteService(Guid id)
        {
            var result = await _servicesService.DeleteService(id);
            return StatusCode(result.StatusCode, result);
        }

        [HttpGet("similar")]
        [SwaggerOperation(Summary = "API get similar Services")]
        public async Task<IActionResult> GetSimilarServices(
            [FromQuery] Guid serviceId,
            int batch = 1,
            int itemPerBatch = 4)
        {
            var result = await _servicesService.GetSimilarServices(serviceId, batch, itemPerBatch);
            return StatusCode(result.StatusCode, result);
        }
    }
}