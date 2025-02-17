using SkincareBookingSystem.DataAccess.IRepositories;
using SkincareBookingSystem.DataAccess.Repositories;
using SkincareBookingSystem.Models.Domain;
using SkincareBookingSystem.Models.Dto.OrderDetails;
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
    public class OrderDetailServcie : IOrderDetailService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IAutoMapperService _mapper;

        public OrderDetailServcie(IUnitOfWork unitOfWork, IAutoMapperService mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<ResponseDto> CreateOrderDetail(ClaimsPrincipal User, CreateOrderDetailDto createOrderDetailDto)
        {
            try
            {

                var orderDetail = _mapper.Map<CreateOrderDetailDto, OrderDetail>(createOrderDetailDto);
                orderDetail.OrderDetailId = Guid.NewGuid();

                await _unitOfWork.OrderDetail.AddAsync(orderDetail);
                await _unitOfWork.SaveAsync();

                return new ResponseDto
                {
                    Result = orderDetail,
                    Message = "Order Detail created successfully",
                    IsSuccess = true,
                    StatusCode = 201
                };
            }
            catch (Exception ex)
            {
                return new ResponseDto
                {
                    Message = $"Error when creating Order Detail: {ex.Message}",
                    IsSuccess = false,
                    StatusCode = 500
                };
            }
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
