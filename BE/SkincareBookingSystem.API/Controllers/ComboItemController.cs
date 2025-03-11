using Microsoft.AspNetCore.Mvc;
using SkincareBookingSystem.Models.Dto.ComboItem;
using SkincareBookingSystem.Services.IServices;

namespace SkincareBookingSystem.API.Controllers;

[Route("api/combo-item")]
[ApiController]
public class ComboItemController : Controller
{
    private readonly IComboItemService _comboItemService;

    public ComboItemController(IComboItemService comboItemService)
    {
        _comboItemService = comboItemService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAllComboItems([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10,
        [FromQuery] string? filterOn = null, [FromQuery] string? filterQuery = null, [FromQuery] string? sortBy = null)
    {
        var result =
            await _comboItemService.GetAllComboItem(User, pageNumber, pageSize, filterOn, filterQuery, sortBy);
        return StatusCode(result.StatusCode, result);
    }

    [HttpPost]
    public async Task<IActionResult> CreateComboItem([FromBody] CreateComboItemDto createComboItemDto)
    {
        var result = await _comboItemService.CreateComboItem(User, createComboItemDto);
        return StatusCode(result.StatusCode, result);
    }
}