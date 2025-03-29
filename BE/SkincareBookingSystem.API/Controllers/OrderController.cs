using Microsoft.AspNetCore.Mvc;
using SkincareBookingSystem.Models.Dto.Orders;
using SkincareBookingSystem.Services.IServices;
using Swashbuckle.AspNetCore.Annotations;

namespace SkincareBookingSystem.API.Controllers
{
    [Route("api/order")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly IOrderService _orderService;
        public OrderController(IOrderService orderService)
        {
            _orderService = orderService;
        }

        [HttpGet]
        [SwaggerOperation(Summary = "API gets all orders", Description = "Requires admin role")]
        public async Task<IActionResult> GetAllOrders()
        {
            var result = await _orderService.GetAllOrders();
            return StatusCode(result.StatusCode, result);
        }

        [HttpGet("{orderId}")]
        [SwaggerOperation(Summary = "API gets an order by id", Description = "Requires admin role")]
        public async Task<IActionResult> GetOrderById(Guid orderId)
        {
            var result = await _orderService.GetOrderById(User, orderId);
            return StatusCode(result.StatusCode, result);
        }
    }
}
