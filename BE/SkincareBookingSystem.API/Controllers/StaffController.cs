using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SkincareBookingSystem.Models.Dto.Staff;
using SkincareBookingSystem.Services.IServices;
using SkincareBookingSystem.Utilities.Constants;
using Swashbuckle.AspNetCore.Annotations;

namespace SkincareBookingSystem.API.Controllers;

[Route("api/staff")]
[ApiController]
public class StaffController : ControllerBase
{
    private readonly IStaffService _staffService;

    public StaffController(IStaffService staffService)
    {
        _staffService = staffService;
    }

    [HttpGet]
    [SwaggerOperation(Summary = "API gets all customer's accounts", Description = "Requires  roles")]
    public async Task<IActionResult> GetCustomerByInfor
    (
        int pageNumber = 1,
        int pageSize = 10,
        string? filterQuery = null
    )
    {
        var response = await _staffService.GetCustomerByInfor(User, pageNumber, pageSize, filterQuery);
        return StatusCode(response.StatusCode, response);
    }
    
    [HttpGet("today-appointments")]
    [Authorize(Roles = StaticUserRoles.Staff)]
    [SwaggerOperation(Summary = "API gets today's appointments", Description = "Requires roles")]
    public async Task<IActionResult> GetTodayAppointments
    (
        int pageNumber = 1,
        int pageSize = 10,
        string? filterQuery = null
    )
    {
        var response = await _staffService.GetTodayAppointments(User, pageNumber, pageSize, filterQuery);
        return StatusCode(response.StatusCode, response);
    }
    
    [HttpPut("check-in")]
    [Authorize(Roles = StaticUserRoles.Staff)]
    [SwaggerOperation(Summary = "API checks in a customer", Description = "Requires roles")]
    public async Task<IActionResult> CheckInCustomer(CheckInDto checkInDto)
    {
        var response = await _staffService.CheckInCustomer(User,checkInDto);
        return StatusCode(response.StatusCode, response);
    }
    
    [HttpPut("check-out")]
    [Authorize(Roles = StaticUserRoles.Staff)]
    [SwaggerOperation(Summary = "API checks out a customer", Description = "Requires roles")]
    public async Task<IActionResult> CheckOutCustomer(CheckInDto checkOutDto)
    {
        var response = await _staffService.CheckOutCustomer(User, checkOutDto);
        return StatusCode(response.StatusCode, response);
    }
}