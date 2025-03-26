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

        [HttpGet("{id}")]
        [SwaggerOperation(Summary = "API gets an order by id", Description = "Requires admin role")]
        public async Task<IActionResult> GetOrderById(Guid id)
        {
            var result = await _orderService.GetOrderById(id);
            return StatusCode(result.StatusCode, result);
        }

        [HttpPut]
        [SwaggerOperation(Summary = "API updates an order", Description = "Requires admin role")]
        public async Task<IActionResult> UpdateOrder([FromBody] UpdateOrderDto updateOrderDto)
        {
            var result = await _orderService.UpdateOrder(User, updateOrderDto);
            return StatusCode(result.StatusCode, result);
        }

        [HttpDelete("{id}")]
        [SwaggerOperation(Summary = "API soft deletes an order", Description = "Requires admin role")]
        public async Task<IActionResult> DeleteOrder(Guid id)
        {
            var result = await _orderService.DeleteOrder(id);
            return StatusCode(result.StatusCode, result);
        }
    }
}
