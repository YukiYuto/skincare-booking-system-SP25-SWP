using SkincareBookingSystem.DataAccess.IRepositories;
using SkincareBookingSystem.Models.Domain;
using SkincareBookingSystem.Models.Dto.Orders;
using SkincareBookingSystem.Models.Dto.Response;
using SkincareBookingSystem.Services.Helpers.Responses;
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
    public class OrderService : IOrderService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IAutoMapperService _autoMapperService;

        public OrderService(IUnitOfWork unitOfWork, IAutoMapperService autoMapperService)
        {
            _unitOfWork = unitOfWork;
            _autoMapperService = autoMapperService;
        }

        public async Task<ResponseDto> GetAllOrders()
        {
            var orders = await _unitOfWork.Order.GetAllAsync();
            var sortedOrders = orders.OrderBy(o => o.OrderNumber).ToList();
            return (sortedOrders.Any())
                ? SuccessResponse.Build(
                    message: StaticResponseMessage.Order.RetrievedAll,
                    statusCode: StaticOperationStatus.StatusCode.Ok,
                    result: sortedOrders)
                : SuccessResponse.Build(
                    message: StaticResponseMessage.Order.NotFound,
                    statusCode: StaticOperationStatus.StatusCode.NotFound,
                    result: new List<Order>()
                );
        }

        public async Task<ResponseDto> GetOrderById(ClaimsPrincipal User, Guid orderId)
        {
            if (User.FindFirstValue(ClaimTypes.NameIdentifier) is null)
            {
                return ErrorResponse.Build(
                    message: StaticResponseMessage.User.NotFound,
                    statusCode: StaticOperationStatus.StatusCode.NotFound
                );
            }

            var getOrder = await _unitOfWork.Order.GetAsync(o => o.OrderId == orderId);
            return (getOrder != null)
                ? SuccessResponse.Build(
                    message: StaticResponseMessage.Order.Retrieved,
                    statusCode: StaticOperationStatus.StatusCode.Ok,
                    result: getOrder)
                : ErrorResponse.Build(
                    message: StaticResponseMessage.Order.NotFound,
                    statusCode: StaticOperationStatus.StatusCode.NotFound
                );
        }

        private async Task<bool> SaveChangesAsync()
        {
            return await _unitOfWork.SaveAsync() == StaticOperationStatus.Database.Success;
        }
    }
}