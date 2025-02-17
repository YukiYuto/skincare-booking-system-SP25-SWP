using SkincareBookingSystem.Models.Dto.OrderDetails;
using SkincareBookingSystem.Models.Dto.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace SkincareBookingSystem.Services.IServices
{
    public interface IOrderDetailService
    {
        Task<ResponseDto> GetOrderDetailById(Guid orderDetailId);
        Task<ResponseDto> GetAllOrderDetails();
        Task<ResponseDto> CreateOrderDetail(ClaimsPrincipal User, CreateOrderDetailDto newOrderDetail);
        Task<ResponseDto> UpdateOrderDetail(ClaimsPrincipal User, UpdateOrderDetailDto orderDetail);
        Task<ResponseDto> DeleteOrderDetail(Guid orderDetailId);
    }
}
