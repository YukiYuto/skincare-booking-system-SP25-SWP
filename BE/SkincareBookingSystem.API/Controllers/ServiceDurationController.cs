using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SkincareBookingSystem.Models.Dto.ServiceDuration;
using SkincareBookingSystem.Services.IServices;

namespace SkincareBookingSystem.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ServiceDurationController : ControllerBase
{
    private readonly IServiceDurationService _serviceDurationService;

    public ServiceDurationController(IServiceDurationService serviceDurationService)
    {
        _serviceDurationService = serviceDurationService;
    }


    [HttpGet]
    public async Task<IActionResult> GetAllServiceDurations
    (
        int pageNumber = 1,
        int pageSize = 10,
        string? filterOn = null,
        string? filterQuery = null,
        string? sortBy = null
    )
    {
        var result = await _serviceDurationService.GetAllServiceDurations
            (User, pageNumber, pageSize, filterOn, filterQuery, sortBy);
        return StatusCode(result.StatusCode, result);
    }


    [HttpPost]
    [Authorize]
    public async Task<IActionResult> CreateServiceDuration(CreateServiceDurationDto createServiceDurationDto)
    {
        var result = await _serviceDurationService.CreateServiceDuration(User, createServiceDurationDto);
        return StatusCode(result.StatusCode, result);
    }
}