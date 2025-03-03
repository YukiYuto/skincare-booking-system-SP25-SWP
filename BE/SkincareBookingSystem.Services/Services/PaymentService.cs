﻿using System.Security.Claims;
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

    public async Task<ResponseDto> CreatePayOsPaymentLink(ClaimsPrincipal User,
        CreatePaymentLinkDto createPaymentLinkDto)
    {
        try
        {
            var order = await _unitOfWork.Order.GetOrderByOrderNumber(createPaymentLinkDto.OrderNumber);

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

            // Lấy danh sách OrderDetail liên quan
            var orderDetails = await _unitOfWork.OrderDetail
                .GetListAsync(od => od.OrderId == order.OrderId);

            if (orderDetails == null || !orderDetails.Any())
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
                await _unitOfWork.OrderDetail.GetServicesAndCombosByOrderIdAsync(order.OrderId);

            // Chuyển danh sách thành Dictionary để truy xuất nhanh hơn
            var serviceDict = services.ToDictionary(s => s.ServiceId);
            var serviceComboDict = serviceCombos.ToDictionary(sc => sc.ServiceComboId);

            var groupedItems = orderDetails.SelectMany(od =>
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
                StatusCode = 200,
                Result = new
                {
                    result,
                    PaymentTransactionId = payment.PaymentTransactionId
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
            var order = await _unitOfWork.Order.GetOrderByOrderNumber(confirmPaymentDto.orderNumber);
            if (order is null)
            {
                return new ResponseDto()
                {
                    Message = "Order does not exist",
                    IsSuccess = false,
                    StatusCode = 404,
                    Result = null
                };
            }
            
            PaymentLinkInformation transactionInfo = await _payOs.getPaymentLinkInformation(confirmPaymentDto.orderNumber);
            if (transactionInfo == null)
            {
                return new ResponseDto()
                {
                    Message = "Transaction not found or not successful",
                    IsSuccess = false,
                    StatusCode = 400,
                    Result = null
                };
            }
            
            var payment = await _unitOfWork.Payment.GetPaymentByOrderNumber(confirmPaymentDto.orderNumber);
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

            payment.Status = PaymentStatus.Paid;
            _unitOfWork.Payment.Update(payment);
            
            var transaction = new Transaction()
            {
                CustomerId = order.CustomerId,
                OrderId = order.OrderId,
                PaymentId = confirmPaymentDto.paymentTransactionId,
                Amount = payment.Amount,
                TransactionMethod = "Tranfer",
                TransactionDateTime = DateTime.UtcNow
            };

            await _unitOfWork.Transaction.AddAsync(transaction);
            await _unitOfWork.SaveAsync();

            return new ResponseDto()
            {
                Message = "Payment confirmed successfully",
                IsSuccess = true,
                StatusCode = 200,
                Result = new
                {
                    TransactionId = transaction.PaymentId,
                    Status = payment.Status,
                    PayOS_ConfirmUrl = transactionInfo.transactions
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