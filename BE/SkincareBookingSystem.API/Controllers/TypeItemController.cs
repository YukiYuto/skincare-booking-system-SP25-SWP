using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SkincareBookingSystem.Models.Dto.TypeItem;
using SkincareBookingSystem.Services.IServices;
using SkincareBookingSystem.Utilities.Constants;

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

    [HttpGet]
    public async Task<IActionResult> GetAllTypeItems([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10,
        [FromQuery] string? filterOn = null, [FromQuery] string? filterQuery = null, [FromQuery] string? sortBy = null)
    {
        var response =
            await _typeItemService.GetAllTypeItem(User, pageNumber, pageSize, filterOn, filterQuery, sortBy);

        return StatusCode(response.StatusCode, response);
    }

    [HttpPost]
    public async Task<IActionResult> CreateTypeItem([FromBody] CreateTypeItemDto createTypeItemDto)
    {
        var response = await _typeItemService.CreateTypeItem(User, createTypeItemDto);

        return StatusCode(response.StatusCode, response);
    }

    [Authorize(Roles = StaticUserRoles.Manager)]
    [HttpPut]
    public async Task<IActionResult> UpdateTypeItem([FromBody] UpdateTypeItemDto updateTypeItemDto)
    {
        var response = await _typeItemService.UpdateTypeItem(User, updateTypeItemDto);
        return StatusCode(response.StatusCode, response);
    }
}