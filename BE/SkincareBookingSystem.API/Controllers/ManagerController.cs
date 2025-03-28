using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SkincareBookingSystem.DataAccess.IRepositories;
using SkincareBookingSystem.Services.IServices;
using SkincareBookingSystem.Utilities.Constants;
using Swashbuckle.AspNetCore.Annotations;

namespace SkincareBookingSystem.API.Controllers;

public class ManagerController : ControllerBase
{
    private readonly IManagerSerivce _managerService;

    public ManagerController(IManagerSerivce managerService)
    {
        _managerService = managerService;
    }

    [HttpGet("revenue-orders")]
    [Authorize(Roles = StaticUserRoles.Manager)]
    [SwaggerOperation(Summary = "API gets revenue from orders", Description = "Requires roles")]
    public async Task<IActionResult> GetRevenueOrders(DateTime startDate, DateTime endDate, int pageNumber = 1, int pageSize = 10)
    {
        var response = await _managerService.GetRevenueOrders(startDate, endDate, pageNumber, pageSize);
        return StatusCode(response.StatusCode, response);
    }

    [HttpGet("revenue-profit")]
    //[Authorize(Roles = StaticUserRoles.Manager)]
    [SwaggerOperation(Summary = "API gets revenue profit", Description = "Requires roles")]
    public async Task<IActionResult> GetRevenueProfit(DateTime startDate, DateTime endDate, int pageNumber = 1, int pageSize = 10)
    {
        var response = await _managerService.GetRevenueProfit(startDate, endDate, pageNumber, pageSize);
        return StatusCode(response.StatusCode, response);
    }
    
    [HttpGet("revenue-transactions")]
    [Authorize(Roles = StaticUserRoles.Manager)]
    [SwaggerOperation(Summary = "API gets revenue transactions", Description = "Requires roles")]
    public async Task<IActionResult> GetRevenueTransactions(DateTime startDate, DateTime endDate, int pageNumber = 1, int pageSize = 10)
    {
        var response = await _managerService.GetRevenueTransactions(startDate, endDate, pageNumber, pageSize);
        return StatusCode(response.StatusCode, response);
    }
}