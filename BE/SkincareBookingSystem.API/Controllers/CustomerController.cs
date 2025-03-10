using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SkincareBookingSystem.Services.IServices;
using Swashbuckle.AspNetCore.Annotations;

namespace SkincareBookingSystem.API.Controllers
{
    [Route("api/customer")]
    [ApiController]
    public class CustomerController : ControllerBase
    {
        private readonly ICustomerService _customerService;

        public CustomerController(ICustomerService customerService)
        {
            _customerService = customerService;
        }

        [HttpGet]
        [SwaggerOperation(Summary = "API gets all customer's accounts", Description = "Requires customer, admin roles")]
        public async Task<IActionResult> GetAllCustomers()
        {
            var response = await _customerService.GetAllCustomers();
            return StatusCode(response.StatusCode, response);
        }

        [HttpGet("{customerId:guid}")]
        [SwaggerOperation(Summary = "API gets a customer's account by id", Description = "Requires customer, admin roles")]
        public async Task<IActionResult> GetCustomerDetailsById(Guid customerId)
        {
            var response = await _customerService.GetCustomerDetailsById(User, customerId);
            return StatusCode(response.StatusCode, response);
        }

        [HttpGet("user")]
        [SwaggerOperation(Summary = "API gets a customer's id by user id", Description = "Requires customer, admin roles")]
        public async Task<IActionResult> GetCustomerIdByUserId()
        {
            var response = await _customerService.GetCustomerIdByUserId(User);
            return StatusCode(response.StatusCode, response);
        }
    }
}
