using Microsoft.AspNetCore.Mvc;
using SkincareBookingSystem.Models.Dto.Slot;
using SkincareBookingSystem.Services.IServices;

namespace SkincareBookingSystem.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class SlotController : Controller
{
    private readonly ISlotService _slotService;
    
    public SlotController(ISlotService slotService)
    {
        _slotService = slotService;
    }
    
    [HttpPost("create")]
    public async Task<IActionResult> CreateSlot([FromBody] CreateSlotDto createSlotDto)
    {
        var result = await _slotService.CreateSlot(User, createSlotDto);
        return StatusCode(result.StatusCode, result);
    }
}