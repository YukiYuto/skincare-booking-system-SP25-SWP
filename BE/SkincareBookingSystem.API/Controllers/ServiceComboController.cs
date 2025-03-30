using Microsoft.AspNetCore.Mvc;
using SkincareBookingSystem.Models.Dto.ServiceCombo;
using SkincareBookingSystem.Services.IServices;

namespace SkincareBookingSystem.API.Controllers;

[Route("api/servicecombo")]
[ApiController]
public class ServiceComboController : Controller
{
    private readonly IServiceComboService _serviceComboService;

    public ServiceComboController(IServiceComboService serviceComboService)
    {
        _serviceComboService = serviceComboService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAllServiceCombos
    (
        [FromQuery] int pageNumber = 1,
        [FromQuery] int pageSize = 10,
        [FromQuery] string? filterOn = null,
        [FromQuery] string? filterQuery = null,
        [FromQuery] string? sortBy = null
    )
    {
        var result =
            await _serviceComboService.GetAllServiceCombos(User, pageNumber, pageSize, filterOn, filterQuery, sortBy);
        return StatusCode(result.StatusCode, result);
    }
    
    [HttpGet("{serviceComboId}")]
    public async Task<IActionResult> GetServiceComboById(Guid serviceComboId)
    {
        var result = await _serviceComboService.GetServiceComboById(User, serviceComboId);
        return StatusCode(result.StatusCode, result);
    }

    [HttpPost]
    public async Task<IActionResult> CreateService([FromBody] CreateServiceComboDto createServiceComboDtoDto)
    {
        var result = await _serviceComboService.CreateServiceCombo(User, createServiceComboDtoDto);
        return StatusCode(result.StatusCode, result);
    }
}