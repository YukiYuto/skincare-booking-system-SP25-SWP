using System.Security.Claims;
using SkincareBookingSystem.Models.Dto.Payment;
using SkincareBookingSystem.Models.Dto.Response;

namespace SkincareBookingSystem.Services.IServices;

public interface IPaymentService
{
    Task<ResponseDto> CreatePayOsPaymentLink(ClaimsPrincipal User ,CreatePaymentLinkDto createPaymentLinkDto);
    Task<ResponseDto> ConfirmPayOsTransaction(ConfirmPaymentDto confirmPaymentDto);
    Task<ResponseDto> CancelPayOsPaymentLink(ClaimsPrincipal User, Guid paymentTransactionId, string cancellationReason);
}