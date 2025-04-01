using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SkincareBookingSystem.Models.Dto.Customer;
using SkincareBookingSystem.Services.IServices;
using SkincareBookingSystem.Utilities.Constants;
using Swashbuckle.AspNetCore.Annotations;

namespace SkincareBookingSystem.API.Controllers
{
    [Route("api/customer")]
    [Authorize]
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
        
        [HttpGet("timetable")]
        [SwaggerOperation(Summary = "API gets a customer's timetable", Description = "Requires customer, admin roles")]
        public async Task<IActionResult> GetCustomerTimeTable()
        {
            var response = await _customerService.GetCustomerTimeTable(User);
            return StatusCode(response.StatusCode, response);
        }
        
        [HttpGet("orders")]
        [SwaggerOperation(Summary = "API gets a customer's orders", Description = "Requires customer, admin roles")]
        public async Task<IActionResult> GetOrderByCustomer()
        {
            var response = await _customerService.GetOrderByCustomer(User);
            return StatusCode(response.StatusCode, response);
        }
        
        [HttpPost("recommendation")]
        [SwaggerOperation(Summary = "API gets a customer's recommendation by skin profile", Description = "Requires customer roles")]
        public async Task<IActionResult> GetRecommendationBySkinProfile([FromBody] RecommendationDto recommendationDto)
        {
            var response = await _customerService.GetRecommendationBySkinProfile(recommendationDto);
            return StatusCode(response.StatusCode, response);
        }
    }
}
