using SkincareBookingSystem.DataAccess.IRepositories;
using SkincareBookingSystem.DataAccess.Repositories;
using SkincareBookingSystem.Models.Domain;
using SkincareBookingSystem.Models.Dto.Booking.Order;
using SkincareBookingSystem.Models.Dto.OrderDetails;
using SkincareBookingSystem.Models.Dto.Response;
using SkincareBookingSystem.Services.Helpers.Responses;
using SkincareBookingSystem.Services.Helpers.Users;
using SkincareBookingSystem.Services.IServices;
using SkincareBookingSystem.Utilities.Constants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace SkincareBookingSystem.Services.Services
{
    public class OrderDetailService : IOrderDetailService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IAutoMapperService _mapper;

        public OrderDetailService(IUnitOfWork unitOfWork, IAutoMapperService mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public Task<ResponseDto> DeleteOrderDetail(Guid orderDetailId)
        {
            throw new NotImplementedException();
        }

        public async Task<ResponseDto> GetAllOrderDetails()
        {
            var orderDetails = await _unitOfWork.OrderDetail.GetAllAsync();
            return new ResponseDto
            {
                Result = orderDetails,
                Message = "List of Order Details",
                IsSuccess = true,
                StatusCode = 200
            };
        }

        public async Task<ResponseDto> GetOrderDetailById(Guid orderDetailId)
        {
            var orderDetail = await _unitOfWork.OrderDetail.GetAsync(o => o.OrderDetailId == orderDetailId);
            if (orderDetail == null)
            {
                return new ResponseDto
                {
                    Message = "Order Detail not found",
                    IsSuccess = false,
                    StatusCode = 404
                };
            }
            return new ResponseDto
            {
                Result = orderDetail,
                Message = "Order Detail found",
                IsSuccess = true,
                StatusCode = 200
            };
        }

        public async Task<ResponseDto> GetOrderDetailByOrderId(ClaimsPrincipal User, Guid orderId)
        {
            var orders = await _unitOfWork.Order.GetAsync(o => o.OrderId == orderId, includeProperties: 
                $"{nameof(Order.OrderDetails)},{nameof(Order.OrderDetails)}.{nameof(OrderDetail.Services)}");
            if (orders == null)
            {
                return ErrorResponse.Build(
                    message: StaticResponseMessage.Order.NotFound,
                    statusCode: StaticOperationStatus.StatusCode.NotFound
                );
            }

            // 3. Map sang DTO
            var result = new GetOrderDetailByOrderIdDto
            {
                OrderId = orders.OrderId,
                OrderDetails = orders.OrderDetails?.Select(od => new GetOrderDetailDto
                {
                    OrderDetailId = od.OrderDetailId,
                    ServiceId = od.ServiceId ?? Guid.Empty,
                    ServiceName = od.Services?.ServiceName ?? "Unknown Service",
                    Price = od.Price,
                    Description = od.Description
                }).ToList()
            };

            // 4. Trả về response
            return new ResponseDto
            {
                Result = result,
                Message = "Hello",
                IsSuccess = true,
                StatusCode = 200
            };

        }

        public async Task<ResponseDto> UpdateOrderDetail(ClaimsPrincipal User, UpdateOrderDetailDto orderDetailDto)
        {
            var orderDetail = await _unitOfWork.OrderDetail.GetAsync(o => o.OrderDetailId == orderDetailDto.OrderDetailId);
            if (orderDetail == null)
            {
                return new ResponseDto
                {
                    Message = "Order Detail not found",
                    IsSuccess = false,
                    StatusCode = 404
                };
            }

            var orderDetailUpdate = _mapper.Map<UpdateOrderDetailDto, OrderDetail>(orderDetailDto);

            _unitOfWork.OrderDetail.Update(orderDetail, orderDetailUpdate);
            await _unitOfWork.SaveAsync();

            return new ResponseDto
            {
                Result = orderDetail,
                Message = "Order Detail updated successfully",
                IsSuccess = true,
                StatusCode = 200
            };
        }
    }
}
