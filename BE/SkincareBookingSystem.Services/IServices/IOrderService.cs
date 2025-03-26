using SkincareBookingSystem.Models.Dto.Orders;
using SkincareBookingSystem.Models.Dto.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace SkincareBookingSystem.Services.IServices
{
    public interface IOrderService
    {
        Task<ResponseDto> UpdateOrder(ClaimsPrincipal User, UpdateOrderDto updateOrderDto);
        Task<ResponseDto> DeleteOrder(Guid id);
        Task<ResponseDto> GetOrderById(Guid id);
        Task<ResponseDto> GetAllOrders();

    }
}
