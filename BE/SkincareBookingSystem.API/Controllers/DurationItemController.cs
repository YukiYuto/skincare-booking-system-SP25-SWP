using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SkincareBookingSystem.Models.Dto.DurationItem;
using SkincareBookingSystem.Services.IServices;

namespace SkincareBookingSystem.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class DurationItemController : ControllerBase
{
    private readonly IDurationItemService _durationItemService;

    public DurationItemController(IDurationItemService durationItemService)
    {
        _durationItemService = durationItemService;
    }


    [HttpGet]
    public async Task<IActionResult> GetAllDurationItems([FromQuery] int pageNumber, [FromQuery] int pageSize)
    {
        var result = await _durationItemService.GetAllDurationItem(User, pageNumber, pageSize);
        return StatusCode(result.StatusCode, result);
    }


    [HttpPost]
    [Authorize]
    public async Task<IActionResult> CreateDurationItem(CreateDurationItemDto createDurationItemDto)
    {
        var result = await _durationItemService.AddDurationItemAsync(User, createDurationItemDto);
        return StatusCode(result.StatusCode, result);
    }

    [HttpDelete]
    public async Task<IActionResult> DeleteDurationItem(CreateDurationItemDto deleteDurationItemDto)
    {
        var result = await _durationItemService.DeleteDuration(deleteDurationItemDto);
        return StatusCode(result.StatusCode, result);
    }
}