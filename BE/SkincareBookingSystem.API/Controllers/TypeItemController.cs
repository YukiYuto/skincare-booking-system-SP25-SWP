using Microsoft.AspNetCore.Mvc;
using SkincareBookingSystem.Models.Dto.TypeItem;
using SkincareBookingSystem.Services.IServices;

namespace SkincareBookingSystem.API.Controllers;

[Route("api/type-item")]
[ApiController]
public class TypeItemController : Controller
{
    private readonly ITypeItemService _typeItemService;
    
    public TypeItemController(ITypeItemService typeItemService)
    {
        _typeItemService = typeItemService;
    }
    
    [HttpPost]
    public async Task<IActionResult> CreateTypeItem([FromBody] CreateTypeItemDto createTypeItemDto)
    {
        var response = await _typeItemService.CreateTypeItem(User, createTypeItemDto);
        
        return StatusCode(response.StatusCode, response);
    }
}