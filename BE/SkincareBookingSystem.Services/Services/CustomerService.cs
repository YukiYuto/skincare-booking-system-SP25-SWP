using SkincareBookingSystem.DataAccess.IRepositories;
using SkincareBookingSystem.Models.Domain;
using SkincareBookingSystem.Models.Dto.Customer;
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
    public class CustomerService : ICustomerService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IAutoMapperService _autoMapperService;

        public CustomerService(IUnitOfWork unitOfWork, IAutoMapperService autoMapperService)
        {
            _unitOfWork = unitOfWork;
            _autoMapperService = autoMapperService;
        }

        public async Task<ResponseDto> GetAllCustomers()
        {
            var customersFromDb = await _unitOfWork.Customer.GetAllAsync(
                includeProperties: nameof(ApplicationUser));

            var customersDto = _autoMapperService.MapCollection<Customer, GetCustomerDto>(customersFromDb);

            return (customersFromDb.Any()) ?
                SuccessResponse.Build(
                message: StaticOperationStatus.Customer.Found,
                statusCode: StaticOperationStatus.StatusCode.Ok,
                result: customersDto) :
                ErrorResponse.Build(
                message: StaticOperationStatus.Customer.NotFound,
                statusCode: StaticOperationStatus.StatusCode.NotFound);
        }

        public async Task<ResponseDto> GetCustomerDetailsById(ClaimsPrincipal user, Guid customerId)
        {
            if (user.FindFirstValue(ClaimTypes.NameIdentifier) is null)
            {
                return ErrorResponse.Build(
                    message: StaticOperationStatus.User.UserNotFound,
                    statusCode: StaticOperationStatus.StatusCode.NotFound);
            }

            var customerFromDb = await _unitOfWork.Customer.GetAsync(
                filter: c => c.CustomerId == customerId,
                includeProperties: nameof(ApplicationUser));

            if (customerFromDb is null)
            {
                return ErrorResponse.Build(
                    message: StaticOperationStatus.Customer.NotFound,
                    statusCode: StaticOperationStatus.StatusCode.NotFound);
            }

            var customerDto = _autoMapperService.Map<Customer, GetCustomerDto>(customerFromDb);

            return SuccessResponse.Build(
                message: StaticOperationStatus.Customer.Found,
                statusCode: StaticOperationStatus.StatusCode.Ok,
                result: customerDto);
        }

        public async Task<ResponseDto> GetCustomerIdByUserId(ClaimsPrincipal User)
        {
            if (UserError.NotExists(User))
            {
                return ErrorResponse.Build(
                    message: StaticOperationStatus.User.UserNotFound,
                    statusCode: StaticOperationStatus.StatusCode.NotFound);
            }

            var customerFromDb = await _unitOfWork.Customer.GetAsync(
                filter: c => c.UserId == User.FindFirstValue(ClaimTypes.NameIdentifier),
                includeProperties: nameof(ApplicationUser));

            if (customerFromDb is null)
            {
                return ErrorResponse.Build(
                    message: StaticOperationStatus.Customer.NotFound,
                    statusCode: StaticOperationStatus.StatusCode.NotFound);
            }

            return SuccessResponse.Build(
                message: StaticOperationStatus.Customer.Found,
                statusCode: StaticOperationStatus.StatusCode.Ok,
                result: customerFromDb.CustomerId);
        }
    }
}
