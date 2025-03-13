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
    
    [HttpPost]
    public async Task<IActionResult> CreateService([FromBody] CreateServiceComboDto createServiceComboDtoDto)
    {
        var result = await _serviceComboService.CreateServiceCombo(User, createServiceComboDtoDto);
        return StatusCode(result.StatusCode, result);
    }
}