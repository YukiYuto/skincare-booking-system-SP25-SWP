using Microsoft.AspNetCore.Mvc;
using SkincareBookingSystem.Models.Dto.OrderDetails;
using SkincareBookingSystem.Services.IServices;

namespace SkincareBookingSystem.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderDetailController : ControllerBase
    {
        private readonly IOrderDetailService _orderDetailService;

        public OrderDetailController(IOrderDetailService orderDetailService)
        {
            _orderDetailService = orderDetailService;
        }

        [HttpPost("create")]
        public async Task<IActionResult> CreateOrderDetail([FromBody] CreateOrderDetailDto createOrderDetailDto)
        {
            var result = await _orderDetailService.CreateOrderDetail(User, createOrderDetailDto);
            return StatusCode(result.StatusCode, result);
        }

        [HttpGet("all")]
        public async Task<IActionResult> GetAllOrderDetails()
        {
            var result = await _orderDetailService.GetAllOrderDetails();
            return StatusCode(result.StatusCode, result);
        }

        [HttpGet("get/{id}")]
        public async Task<IActionResult> GetOrderDetailById([FromQuery] Guid id)
        {
            var result = await _orderDetailService.GetOrderDetailById(id);
            return StatusCode(result.StatusCode, result);
        }

        [HttpPut("update")]
        public async Task<IActionResult> UpdateOrderDetail([FromBody] UpdateOrderDetailDto updateOrderDetailDto)
        {
            var result = await _orderDetailService.UpdateOrderDetail(User, updateOrderDetailDto);
            return StatusCode(result.StatusCode, result);
        }

        [HttpDelete("delete/{id}")]
        public async Task<IActionResult> DeleteOrderDetail([FromQuery] Guid id)
        {
            var result = await _orderDetailService.DeleteOrderDetail(id);
            return StatusCode(result.StatusCode, result);
        }  
    }
}
