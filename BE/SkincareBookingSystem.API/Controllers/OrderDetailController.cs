using Microsoft.AspNetCore.Mvc;
using SkincareBookingSystem.Models.Dto.OrderDetails;
using SkincareBookingSystem.Services.IServices;
using Swashbuckle.AspNetCore.Annotations;

namespace SkincareBookingSystem.API.Controllers
{
    [Route("api/order-detail")]
    [ApiController]
    public class OrderDetailController : ControllerBase
    {
        private readonly IOrderDetailService _orderDetailService;

        public OrderDetailController(IOrderDetailService orderDetailService)
        {
            _orderDetailService = orderDetailService;
        }

        
        [HttpGet]
        [SwaggerOperation(Summary = "API gets all order details", Description = "Requires admin role")]
        public async Task<IActionResult> GetAllOrderDetails()
        {
            var result = await _orderDetailService.GetAllOrderDetails();
            return StatusCode(result.StatusCode, result);
        }

        [HttpGet("{id}")]
        [SwaggerOperation(Summary = "API gets an order detail by id", Description = "Requires id")]
        public async Task<IActionResult> GetOrderDetailById([FromQuery] Guid id)
        {
            var result = await _orderDetailService.GetOrderDetailById(id);
            return StatusCode(result.StatusCode, result);
        }

        [HttpPut]
        [SwaggerOperation(Summary = "API updates an order detail", Description = "Requires user role")]
        public async Task<IActionResult> UpdateOrderDetail([FromBody] UpdateOrderDetailDto updateOrderDetailDto)
        {
            var result = await _orderDetailService.UpdateOrderDetail(User, updateOrderDetailDto);
            return StatusCode(result.StatusCode, result);
        }

        [HttpDelete]
        [SwaggerOperation(Summary = "API soft deletes an order detail", Description = "Requires id")]
        public async Task<IActionResult> DeleteOrderDetail([FromQuery] Guid id)
        {
            var result = await _orderDetailService.DeleteOrderDetail(id);
            return StatusCode(result.StatusCode, result);
        }  
    }
}
