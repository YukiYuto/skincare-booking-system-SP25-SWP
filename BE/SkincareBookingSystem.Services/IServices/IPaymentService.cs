using System.Security.Claims;
using SkincareBookingSystem.Models.Dto.Payment;
using SkincareBookingSystem.Models.Dto.Response;

namespace SkincareBookingSystem.Services.IServices;

public interface IPaymentService
{
    Task<ResponseDto> CreatePayOsPaymentLink(ClaimsPrincipal User, CreatePaymentLinkDto createPaymentLinkDto);
    Task<ResponseDto> ConfirmPayOsTransaction(ConfirmPaymentDto confirmPaymentDto);

    Task<ResponseDto> GetAll
    (
        ClaimsPrincipal User,
        int pageNumber = 1,
        int pageSize = 10,
        string? filterOn = null,
        string? filterQuery = null,
        string? sortBy = null
    );

    Task<ResponseDto> GetPaymentById(ClaimsPrincipal User, Guid paymentTransactionId);
}