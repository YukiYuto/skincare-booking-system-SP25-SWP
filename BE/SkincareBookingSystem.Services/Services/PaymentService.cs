using System.Security.Claims;
using Net.payOS;
using Net.payOS.Types;
using SkincareBookingSystem.DataAccess.IRepositories;
using SkincareBookingSystem.Models.Domain;
using SkincareBookingSystem.Models.Dto.Payment;
using SkincareBookingSystem.Models.Dto.Response;
using SkincareBookingSystem.Services.IServices;
using SkincareBookingSystem.Utilities.Constants;
using Transaction = SkincareBookingSystem.Models.Domain.Transaction;

namespace SkincareBookingSystem.Services.Services;

public class PaymentService : IPaymentService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly PayOS _payOs;
    private readonly IAutoMapperService _mapper;

    public PaymentService(IUnitOfWork unitOfWork, PayOS payOs, IAutoMapperService mapper)
    {
        _unitOfWork = unitOfWork;
        _payOs = payOs;
        _mapper = mapper;
    }

    public async Task<ResponseDto> GetAll
    (
        ClaimsPrincipal User,
        int pageNumber = 1,
        int pageSize = 10,
        string? filterOn = null,
        string? filterQuery = null,
        string? sortBy = null
    )
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (string.IsNullOrEmpty(userId))
        {
            return new ResponseDto()
            {
                Message = "User not found",
                IsSuccess = false,
                StatusCode = 404,
                Result = null
            };
        }
        
        bool isManager = User.IsInRole("MANAGER");
        
        Guid? customerId = null;
        if (!isManager)
        {
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

            customerId = customer.CustomerId;
        }
        
        var (payments, totalPayments) =
            await _unitOfWork.Payment.GetPaymentsAsync(pageNumber, pageSize, filterOn, filterQuery, sortBy,customerId);

        if (!payments.Any())
        {
            return new ResponseDto()
            {
                Message = "No payments found",
                IsSuccess = false,
                Result = payments,
                StatusCode = 200
            };
        }

        int totalPages = (int)Math.Ceiling((double)totalPayments / pageSize);
        
        var paymentDtos = _mapper.MapCollection<Payment, GetAllPaymentDto>(payments);

        return new ResponseDto()
        {
            Message = "Get all payments successfully",
            IsSuccess = true,
            StatusCode = 200,
            Result = new
            {
                TotalPayments = totalPayments,
                TotalPages = totalPages,
                PageSize = pageSize,
                CurrentPage = pageNumber,
                Payments = paymentDtos
            }
        };
    }

    public async Task<ResponseDto> GetPaymentById(ClaimsPrincipal User, Guid paymentTransactionId)
    {
        var payment = await _unitOfWork.Payment.GetAsync(p => p.PaymentTransactionId == paymentTransactionId);
        if (payment == null)
        {
            return new ResponseDto()
            {
                Message = "Payment not found",
                IsSuccess = false,
                StatusCode = 404,
                Result = null
            };
        }

        var paymentDto = _mapper.Map<Payment, GetAllPaymentDto>(payment);

        return new ResponseDto()
        {
            Message = "Get payment by Id successfully",
            IsSuccess = true,
            StatusCode = 200,
            Result = paymentDto
        };
    }

    public async Task<ResponseDto> CreatePayOsPaymentLink(ClaimsPrincipal User,
        CreatePaymentLinkDto createPaymentLinkDto)
    {
        try
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId))
            {
                return new ResponseDto()
                {
                    Message = "User not found",
                    IsSuccess = false,
                    StatusCode = 404,
                    Result = null
                };
            }

            var customer = await _unitOfWork.Customer.GetAsync(c => c.UserId == userId);
            if (customer == null)
            {
                return new ResponseDto()
                {
                    Message = "Customer not found",
                    IsSuccess = false,
                    StatusCode = 404,
                    Result = null
                };
            }

            var order = await _unitOfWork.Order.GetAsync(
                x => x.CustomerId == customer.CustomerId
                     && x.OrderNumber == createPaymentLinkDto.OrderNumber,
                includeProperties:
                $"{nameof(Order.OrderDetails)}.{nameof(OrderDetail.Services)},{nameof(Order.OrderDetails)}.{nameof(OrderDetail.ServiceCombo)}");

            if (!order.OrderDetails.Any())
            {
                return new ResponseDto()
                {
                    Message = "No services or service combos found for this order",
                    IsSuccess = false,
                    StatusCode = 404,
                    Result = null
                };
            }

            // Chuyển danh sách thành Dictionary để truy xuất nhanh hơn

            var serviceDict = order.OrderDetails.Where(od => od.Services != null).Select(od => od.Services)
                .ToDictionary(s => s.ServiceId);

            var serviceComboDict = order.OrderDetails.Where(od => od.ServiceCombo != null).Select(od => od.ServiceCombo)
                .ToDictionary(sc => sc.ServiceComboId);

            var groupedItems = order.OrderDetails.SelectMany(od =>
            {
                var items = new List<ItemData>();

                // Xử lý Service
                if (od.ServiceId != null && serviceDict.TryGetValue(od.ServiceId.Value, out var service))
                {
                    items.Add(new ItemData(
                        name: service.ServiceName,
                        quantity: 1,
                        price: Convert.ToInt32(service.Price)
                    ));
                }

                // Xử lý ServiceCombo
                if (od.ServiceComboId != null &&
                    serviceComboDict.TryGetValue(od.ServiceComboId.Value, out var serviceCombo))
                {
                    items.Add(new ItemData(
                        name: serviceCombo.ComboName,
                        quantity: 1,
                        price: Convert.ToInt32(serviceCombo.Price)
                    ));
                }

                return items;
            }).ToList();


            var totalPrice = groupedItems.Sum(i => i.price * i.quantity);

            var paymentData = new PaymentData(
                orderCode: createPaymentLinkDto.OrderNumber,
                amount: totalPrice,
                description: "Payment for services",
                items: groupedItems,
                cancelUrl: createPaymentLinkDto.CancelUrl,
                returnUrl: createPaymentLinkDto.ReturnUrl
            );

            CreatePaymentResult result = await _payOs.createPaymentLink(paymentData);

            Payment payment = new Payment()
            {
                OrderNumber = createPaymentLinkDto.OrderNumber,
                Amount = result.amount,
                Description = result.description.Trim(),
                CancelUrl = paymentData.cancelUrl,
                ReturnUrl = paymentData.returnUrl,
                CreatedAt = DateTime.UtcNow.AddHours(7),
                Status = PaymentStatus.Pending
            };

            await _unitOfWork.Payment.AddAsync(payment);
            await _unitOfWork.SaveAsync();

            return new ResponseDto()
            {
                Message = "Create payment link successfully",
                IsSuccess = true,
                StatusCode = 201,
                Result = new
                {
                    result
                }
            };
        }
        catch (Exception e)
        {
            return new ResponseDto()
            {
                Message = e.Message,
                IsSuccess = false,
                StatusCode = 500,
                Result = null
            };
        }
    }


    public async Task<ResponseDto> ConfirmPayOsTransaction(ConfirmPaymentDto confirmPaymentDto)
    {
        try
        {
            var order = await _unitOfWork.Order.GetOrderByOrderNumber(confirmPaymentDto.OrderNumber);
            if (order == null)
            {
                return new ResponseDto()
                {
                    Message = "Order not found",
                    IsSuccess = false,
                    StatusCode = 404,
                    Result = null
                };
            }

            PaymentLinkInformation transactionInfo =
                await _payOs.getPaymentLinkInformation(confirmPaymentDto.OrderNumber);

            if (transactionInfo == null)
            {
                return new ResponseDto()
                {
                    Message = "Transaction not found",
                    IsSuccess = false,
                    StatusCode = 400,
                    Result = null
                };
            }

            var payment = await _unitOfWork.Payment.GetPaymentByOrderNumber(confirmPaymentDto.OrderNumber);
            if (payment == null)
            {
                return new ResponseDto()
                {
                    Message = "Payment record not found",
                    IsSuccess = false,
                    StatusCode = 404,
                    Result = null
                };
            }

            if (transactionInfo.status == "PAID")
            {
                payment.Status = PaymentStatus.Paid;
                var transaction = new Transaction()
                {
                    CustomerId = order.CustomerId,
                    OrderId = order.OrderId,
                    PaymentId = payment.PaymentTransactionId,
                    Amount = payment.Amount,
                    TransactionMethod = "Transfer",
                    TransactionDateTime = DateTime.UtcNow
                };

                await _unitOfWork.Transaction.AddAsync(transaction);
            }
            else if (transactionInfo.status == "CANCELLED")
            {
                payment.Status = PaymentStatus.Cancelled;
            }
            else
            {
                return new ResponseDto()
                {
                    Message = $"{transactionInfo.status}",
                    IsSuccess = false,
                    StatusCode = 400,
                    Result = null
                };
            }

            _unitOfWork.Payment.Update(payment);
            await _unitOfWork.SaveAsync();

            var orderId = await _unitOfWork.Order.GetOrderByOrderNumber(confirmPaymentDto.OrderNumber);

            return new ResponseDto()
            {
                Message = "Payment status updated successfully and Transaction created",
                IsSuccess = true,
                StatusCode = 201,
                Result = new
                {
                    OrderNumber = payment.OrderNumber,
                    OrderId = order.OrderId
                }
            };
        }
        catch (Exception ex)
        {
            return new ResponseDto()
            {
                Message = ex.Message,
                IsSuccess = false,
                StatusCode = 500,
                Result = null
            };
        }
    }
}