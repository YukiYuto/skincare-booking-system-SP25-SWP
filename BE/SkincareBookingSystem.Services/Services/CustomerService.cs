using SkincareBookingSystem.DataAccess.IRepositories;
using SkincareBookingSystem.Models.Domain;
using SkincareBookingSystem.Models.Dto.Customer;
using SkincareBookingSystem.Models.Dto.GetCustomerInfo;
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
        private readonly ICustomerRepository _customerRepository;

        public CustomerService(IUnitOfWork unitOfWork, IAutoMapperService autoMapperService,
            ICustomerRepository customerRepository)
        {
            _unitOfWork = unitOfWork;
            _autoMapperService = autoMapperService;
            _customerRepository = customerRepository;
        }

        public async Task<ResponseDto> GetAllCustomers()
        {
            var customersFromDb = await _unitOfWork.Customer.GetAllAsync(
                includeProperties: nameof(ApplicationUser));

            var customersDto = _autoMapperService.MapCollection<Customer, GetCustomerDto>(customersFromDb);

            return (customersFromDb.Any())
                ? SuccessResponse.Build(
                    message: StaticOperationStatus.Customer.Found,
                    statusCode: StaticOperationStatus.StatusCode.Ok,
                    result: customersDto)
                : ErrorResponse.Build(
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

        public async Task<ResponseDto> GetCustomerTimeTable(ClaimsPrincipal User)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId is null)
            {
                return ErrorResponse.Build(
                    message: StaticOperationStatus.User.UserNotFound,
                    statusCode: StaticOperationStatus.StatusCode.NotFound);
            }

            ;

            var customerFromDb = await _unitOfWork.Customer.GetAsync(c => c.UserId == userId);
            if (customerFromDb is null)
            {
                return ErrorResponse.Build(
                    message: StaticResponseMessage.Customer.Invalid,
                    statusCode: StaticOperationStatus.StatusCode.BadRequest);
            }

            ;

            var appointmentsFromCustomer = await _unitOfWork.Appointments.GetAllAsync(
                filter: a => a.CustomerId == customerFromDb.CustomerId,
                includeProperties: $"{nameof(Appointments.TherapistSchedules)}." +
                                   $"{nameof(TherapistSchedule.SkinTherapist)}." +
                                   $"{nameof(SkinTherapist.ApplicationUser)}," +
                                   $"{nameof(Appointments.Order)}." +
                                   $"{nameof(Order.OrderDetails)}." +
                                   $"{nameof(OrderDetail.Services)}." +
                                   $"{nameof(Models.Domain.Services.TypeItems)}"
            );
            if (appointmentsFromCustomer is null)
            {
                return ErrorResponse.Build(
                    message: StaticResponseMessage.Appointment.NotFound,
                    statusCode: StaticOperationStatus.StatusCode.NotFound);
            };

            var appointmentsDto = appointmentsFromCustomer.Select(a => new
            {
                AppointmentId = a.AppointmentId,
                AppointmentDate = a.AppointmentDate,
                AppointmentTime = a.AppointmentTime,
                ScheduleStatus = a.TherapistSchedules.FirstOrDefault()?.ScheduleStatus ?? ScheduleStatus.Pending,
                SlotId = a.TherapistSchedules.FirstOrDefault()?.SlotId,
            }).Distinct().ToList();

            var services = appointmentsFromCustomer.SelectMany(
                    a => a.Order.OrderDetails.Select(od => od.Services))
                .Where(s => s is not null)
                .Distinct()
                .ToList();
            var serviceTypeIds = services.SelectMany(
                s => s.TypeItems.Select(ti => ti.ServiceTypeId))
                .Distinct()
                .ToList();

            var therapistsFromDb = await _unitOfWork.SkinTherapist.GetAllAsync(
                filter: s => s.TherapistServiceTypes.Any(
                    tst => serviceTypeIds.Contains(tst.ServiceTypeId)),
                includeProperties: $"{nameof(SkinTherapist.TherapistServiceTypes)},{nameof(ApplicationUser)}");

            var therapists = therapistsFromDb.Select(t => new
                {
                    TherapistId = t.SkinTherapistId,
                    TherapistName = t.ApplicationUser.FullName,
                }).Distinct().ToList();


            return SuccessResponse.Build(
                message: "Customer's timetable found",
                statusCode: StaticOperationStatus.StatusCode.Ok,
                result: new
                {
                    Appointments = appointmentsDto,
                    Therapists = therapists
                });
        }

        public async Task<ResponseDto> GetOrderByCustomer(ClaimsPrincipal User)
        {
            try
            {
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                if (userId == null)
                {
                    return new ResponseDto()
                    {
                        Message = "User not found",
                        IsSuccess = false,
                        StatusCode = 404
                    };
                }

                var customer = await _unitOfWork.Customer.GetAsync(c => c.UserId == userId);
                if (customer == null)
                {
                    return new ResponseDto()
                    {
                        Message = "Customer not found",
                        IsSuccess = false,
                        StatusCode = 404
                    };
                }

                var orders = await _unitOfWork.Order.GetAllAsync(
                    filter: o => o.CustomerId == customer.CustomerId,
                    includeProperties: $"{nameof(Order.Transactions)}"
                );

                if (!orders.Any())
                {
                    return new ResponseDto()
                    {
                        Message = "No orders found",
                        IsSuccess = true,
                        StatusCode = 200,
                        Result = new List<GetOrderByCustomerDto>()
                    };
                }

                var orderDtos = orders.Select(o => new GetOrderByCustomerDto
                {
                    OrderId = o.OrderId,
                    CreatedTime = o.CreatedTime ?? DateTime.MinValue,
                    TotalAmount = (decimal)o.TotalPrice,
                    Status = o.Status ?? string.Empty,
                    Transaction = o.Transactions.FirstOrDefault() != null ? new TransactionInfo
                    {
                        TransactionId = o.Transactions.FirstOrDefault().TransactionId,
                        Amount = (decimal)o.Transactions.First().Amount,
                        TransactionTime = o.Transactions.First().TransactionDateTime
                    } : null
                }).OrderByDescending(o => o.CreatedTime).ToList();

                return new ResponseDto()
                {
                    Message = "Orders retrieved successfully",
                    IsSuccess = true,
                    StatusCode = 200,
                    Result = orderDtos
                };
            }
            catch (Exception ex)
            {
                return new ResponseDto()
                {
                    Message = ex.Message,
                    IsSuccess = false,
                    StatusCode = 500
                };
            }
        }
    }
}