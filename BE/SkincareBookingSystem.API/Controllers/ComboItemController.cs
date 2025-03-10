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
    
    [HttpPost]
    public async Task<IActionResult> CreateComboItem([FromBody] CreateComboItemDto createComboItemDto)
    {
        var result = await _comboItemService.CreateComboItem(User, createComboItemDto);
        return StatusCode(result.StatusCode, result);
    }
}