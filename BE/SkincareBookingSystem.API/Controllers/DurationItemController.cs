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

    [HttpPost]
    [Authorize]
    public async Task<IActionResult> CreateDurationItem(CreateDurationItemDto createDurationItemDto)
    {
        var result = await _durationItemService.AddDurationItemAsync(User, createDurationItemDto);
        return StatusCode(result.StatusCode, result);
    }
}