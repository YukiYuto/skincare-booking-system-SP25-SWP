using SkincareBookingSystem.DataAccess.IRepositories;
using SkincareBookingSystem.Models.Dto.Orders;
using SkincareBookingSystem.Models.Dto.Response;
using SkincareBookingSystem.Services.IServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace SkincareBookingSystem.Services.Services
{
    public class OrderService : IOrderService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IAutoMapperService _autoMapperService;

        public OrderService(IUnitOfWork unitOfWork, IAutoMapperService autoMapperService)
        {
            _unitOfWork = unitOfWork;
            _autoMapperService = autoMapperService;
        }

        public async Task<ResponseDto> DeleteOrder(Guid id)
        {
            var order = await _unitOfWork.Order.GetAsync(o => o.OrderId == id);
            if (order == null)
            {
                return new ResponseDto
                {
                    Message = "Order not found",
                    IsSuccess = false,
                    StatusCode = 404
                };
            }

            order.Status = "1"; // Soft delete
            order.UpdatedTime = DateTime.UtcNow;

            await _unitOfWork.SaveAsync();

            return new ResponseDto
            {
                Message = "Order deleted successfully",
                IsSuccess = true,
                StatusCode = 200
            };
        }

        public async Task<ResponseDto> GetAllOrders()
        {
            var orders = await _unitOfWork.Order.GetAllAsync(o => o.Status != "1");
            return new ResponseDto
            {
                Result = orders,
                Message = "Orders retrieved successfully",
                IsSuccess = true,
                StatusCode = 200
            };
        }

        public async Task<ResponseDto> GetOrderById(Guid id)
        {
            var order = await _unitOfWork.Order.GetAsync(o => o.OrderId == id);
            if (order == null)
            {
                return new ResponseDto
                {
                    Message = "Order not found",
                    IsSuccess = false,
                    StatusCode = 404
                };
            }

            return new ResponseDto
            {
                Result = order,
                Message = "Order retrieved successfully",
                IsSuccess = true,
                StatusCode = 200
            };
        }

        public async Task<ResponseDto> UpdateOrder(ClaimsPrincipal User, UpdateOrderDto updateOrderDto)
        {
            var order = await _unitOfWork.Order.GetAsync(o => o.OrderId == updateOrderDto.OrderId);
            if (order == null)
            {
                return new ResponseDto
                {
                    Message = "Order not found",
                    IsSuccess = false,
                    StatusCode = 404
                };
            }

            var updateDate = _autoMapperService.Map<UpdateOrderDto, Models.Domain.Order>(updateOrderDto);
            order.UpdatedBy = User.Identity?.Name;

            _unitOfWork.Order.Update(order, updateDate);
            await _unitOfWork.SaveAsync();

            return new ResponseDto
            {
                Message = "Order updated successfully",
                IsSuccess = true,
                StatusCode = 200
            };
        }
    }
}