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

    [HttpGet("{slotId}")]
    public async Task<IActionResult> GetSlotById(Guid slotId)
    {
        var result = await _slotService.GetSlotById(slotId);
        return StatusCode(result.StatusCode, result);
    }

    [HttpGet("list")]
    public async Task<IActionResult> GetAllSlots()
    {
        var result = await _slotService.GetAllSlots();
        return StatusCode(result.StatusCode, result);
    }

    [HttpPut("update")]
    public async Task<IActionResult> UpdateSlot([FromBody] UpdateSlotDto updateSlotDto)
    {
        var result = await _slotService.UpdateSlot(User, updateSlotDto);
        return StatusCode(result.StatusCode, result);
    }

    [HttpDelete("{slotId}")]
    public async Task<IActionResult> DeleteSlot(Guid slotId)
    {
        var result = await _slotService.DeleteSlot(User, slotId);
        return StatusCode(result.StatusCode, result);
    }
}