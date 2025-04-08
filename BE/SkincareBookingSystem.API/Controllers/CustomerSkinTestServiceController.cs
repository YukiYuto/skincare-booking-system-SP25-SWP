using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SkincareBookingSystem.Models.Dto.CustomerSkinTest;
using SkincareBookingSystem.Services.IServices;

namespace SkincareBookingSystem.API.Controllers;

[Route("api/customer-skin-test")]
[ApiController]

public class CustomerSkinTestServiceController : ControllerBase
{
    private readonly ICustomerSkinTestService _customerSkinTestService;

    public CustomerSkinTestServiceController(ICustomerSkinTestService customerSkinTestService)
    {
        _customerSkinTestService = customerSkinTestService;
    }

    [HttpPost]
    [Authorize]
    public async Task<IActionResult> CreateCustomerSkinTest([FromBody] CreateCustomerSkinTestDto createCustomerSkinTestDto)
    {
        var response = await _customerSkinTestService.CreateCustomerSkinTest(User, createCustomerSkinTestDto);
        return StatusCode(response.StatusCode, response);
    }
}