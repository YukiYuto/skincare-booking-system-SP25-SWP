using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SkincareBookingSystem.DataAccess.IRepositories;
using SkincareBookingSystem.Models.Dto.LockUser;
using SkincareBookingSystem.Services.IServices;
using SkincareBookingSystem.Utilities.Constants;
using Swashbuckle.AspNetCore.Annotations;

namespace SkincareBookingSystem.API.Controllers;

[Route("api/revenue")]
[ApiController]
public class ManagerController : ControllerBase
{
    private readonly IManagerSerivce _managerService;

    public ManagerController(IManagerSerivce managerService)
    {
        _managerService = managerService;
    }

    [HttpGet("orders")]
    [Authorize(Roles = StaticUserRoles.Manager)]
    [SwaggerOperation(Summary = "API gets revenue from orders", Description = "Requires roles")]
    public async Task<IActionResult> GetRevenueOrders(DateTime startDate, DateTime endDate, int pageNumber = 1, int pageSize = 10)
    {
        var response = await _managerService.GetRevenueOrders(startDate, endDate, pageNumber, pageSize);
        return StatusCode(response.StatusCode, response);
    }

    [HttpGet("profit")]
    //[Authorize(Roles = StaticUserRoles.Manager)]
    [SwaggerOperation(Summary = "API gets revenue profit", Description = "Requires roles")]
    public async Task<IActionResult> GetRevenueProfit(DateTime startDate, DateTime endDate, int pageNumber = 1, int pageSize = 10)
    {
        var response = await _managerService.GetRevenueProfit(startDate, endDate, pageNumber, pageSize);
        return StatusCode(response.StatusCode, response);
    }
    
    [HttpGet("transactions")]
    [Authorize(Roles = StaticUserRoles.Manager)]
    [SwaggerOperation(Summary = "API gets revenue transactions", Description = "Requires roles")]
    public async Task<IActionResult> GetRevenueTransactions(DateTime startDate, DateTime endDate, int pageNumber = 1, int pageSize = 10)
    {
        var response = await _managerService.GetRevenueTransactions(startDate, endDate, pageNumber, pageSize);
        return StatusCode(response.StatusCode, response);
    }
    
    [HttpPost("lock-user")]
    [Authorize(Roles = StaticUserRoles.Manager)]
    [SwaggerOperation(Summary = "API locks user", Description = "Requires Manager roles")]
    public async Task<IActionResult> LockUser([FromBody] LockUserDto lockUserDto)
    {
        var response = await _managerService.LockUser(lockUserDto);
        return StatusCode(response.StatusCode, response);
    }
    
    [HttpPost("unlock-user")]
    [Authorize(Roles = StaticUserRoles.Manager)]
    [SwaggerOperation(Summary = "API unlocks user", Description = "Requires Manager roles")]
    public async Task<IActionResult> UnlockUser([FromBody] UnLockUserDto unLockUserDto)
    {
        var response = await _managerService.UnlockUser(unLockUserDto);
        return StatusCode(response.StatusCode, response);
    }
}