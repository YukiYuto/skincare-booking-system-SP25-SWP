using Microsoft.AspNetCore.Mvc;
using SkincareBookingSystem.Models.Dto.Slot;
using SkincareBookingSystem.Services.IServices;
using Swashbuckle.AspNetCore.Annotations;

namespace SkincareBookingSystem.API.Controllers;

[ApiController]
[Route("api/slot")]
public class SlotController : Controller
{
    private readonly ISlotService _slotService;

    public SlotController(ISlotService slotService)
    {
        _slotService = slotService;
    }

    [HttpPost]
    [SwaggerOperation(Summary = "API creates a slot", Description = "Requires Therapist's schedule")]
    public async Task<IActionResult> CreateSlot([FromBody] CreateSlotDto createSlotDto)
    {
        var result = await _slotService.CreateSlot(User, createSlotDto);
        return StatusCode(result.StatusCode, result);
    }

    [HttpGet("{slotId}")]
    [SwaggerOperation(Summary = "API gets a slot by id", Description = "Requires slotId")]
    public async Task<IActionResult> GetSlotById(Guid slotId)
    {
        var result = await _slotService.GetSlotById(slotId);
        return StatusCode(result.StatusCode, result);
    }

    [HttpGet]
    [SwaggerOperation(Summary = "API gets all slots", Description = "Requires admin role")]
    public async Task<IActionResult> GetAllSlots()
    {
        var result = await _slotService.GetAllSlots();
        return StatusCode(result.StatusCode, result);
    }

    [HttpPut]
    [SwaggerOperation(Summary = "API updates a slot", Description = "Requires admin role")]
    public async Task<IActionResult> UpdateSlot([FromBody] UpdateSlotDto updateSlotDto)
    {
        var result = await _slotService.UpdateSlot(User, updateSlotDto);
        return StatusCode(result.StatusCode, result);
    }

    [HttpDelete("{slotId}")]
    [SwaggerOperation(Summary = "API soft deletes a slot", Description = "Requires slotId")]
    public async Task<IActionResult> DeleteSlot(Guid slotId)
    {
        var result = await _slotService.DeleteSlot(User, slotId);
        return StatusCode(result.StatusCode, result);
    }
}