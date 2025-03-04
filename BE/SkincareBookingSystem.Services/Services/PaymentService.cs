using System.Security.Claims;
using Net.payOS;
using Net.payOS.Types;
using SkincareBookingSystem.DataAccess.IRepositories;
using SkincareBookingSystem.Models.Domain;
using SkincareBookingSystem.Models.Dto.Payment;
using SkincareBookingSystem.Models.Dto.Response;
using SkincareBookingSystem.Services.IServices;
using Transaction = SkincareBookingSystem.Models.Domain.Transaction;

namespace SkincareBookingSystem.Services.Services;

public class PaymentService : IPaymentService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly PayOS _payOs;

    public PaymentService(IUnitOfWork unitOfWork, PayOS payOs)
    {
        _unitOfWork = unitOfWork;
        _payOs = payOs;
    }

    public async Task<ResponseDto> CreatePayOsPaymentLink(ClaimsPrincipal User, CreatePaymentLinkDto createPaymentLinkDto)
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
                     && x.OrderNumber == createPaymentLinkDto.OrderNumber);

            // Lấy danh sách OrderDetail liên quan
            var orderDetails = await _unitOfWork.OrderDetail
                .GetListAsync(od => od.OrderId == order!.OrderId);

            var enumerable = orderDetails.ToList();
            if (!enumerable.Any())
            {
                return new ResponseDto()
                {
                    Message = "No services or service combos found for this order",
                    IsSuccess = false,
                    StatusCode = 404,
                    Result = null
                };
            }

            var (services, serviceCombos) =
                await _unitOfWork.OrderDetail.GetServicesAndCombosByOrderIdAsync(order!.OrderId);

            // Chuyển danh sách thành Dictionary để truy xuất nhanh hơn
            var serviceDict = services.ToDictionary(s => s.ServiceId);
            var serviceComboDict = serviceCombos.ToDictionary(sc => sc.ServiceComboId);

            var groupedItems = enumerable.SelectMany(od =>
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

            // Tính tổng tiền
            var totalPrice = groupedItems.Sum(i => i.price * i.quantity);

            // Tạo dữ liệu thanh toán
            var paymentData = new PaymentData(
                orderCode: createPaymentLinkDto.OrderNumber,
                amount: totalPrice,
                description: "Payment for services",
                items: groupedItems,
                cancelUrl: createPaymentLinkDto.CancelUrl,
                returnUrl: createPaymentLinkDto.ReturnUrl
            );

            CreatePaymentResult result = await _payOs.createPaymentLink(paymentData);

            // Lưu vào database
            Payment payment = new Payment()
            {
                OrderNumber = createPaymentLinkDto.OrderNumber,
                Amount = result.amount,
                Description = result.description.Trim(),
                CancelUrl = paymentData.cancelUrl,
                ReturnUrl = paymentData.returnUrl,
                CreatedAt = DateTime.UtcNow,
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
                    //PaymentTransactionId = payment.PaymentTransactionId
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

    public Task<ResponseDto> CancelPayOsPaymentLink(ClaimsPrincipal User, Guid paymentTransactionId,
        string cancellationReason)
    {
        throw new NotImplementedException();
    }
}