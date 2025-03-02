using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SkincareBookingSystem.Services.IServices;

namespace SkincareBookingSystem.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomerController : ControllerBase
    {
        private readonly ICustomerService _customerService;

        public CustomerController(ICustomerService customerService)
        {
            _customerService = customerService;
        }

        [HttpGet("list")]
        public async Task<IActionResult> GetAllCustomers()
        {
            var response = await _customerService.GetAllCustomers();
            return StatusCode(response.StatusCode, response);
        }

        [HttpGet("{customerId}")]
        public async Task<IActionResult> GetCustomerDetailsById(Guid customerId)
        {
            var response = await _customerService.GetCustomerDetailsById(User, customerId);
            return StatusCode(response.StatusCode, response);
        }

        [HttpGet("id")]
        public async Task<IActionResult> GetCustomerIdByUserId()
        {
            var response = await _customerService.GetCustomerIdByUserId(User);
            return StatusCode(response.StatusCode, response);
        }
    }
}
