using Microsoft.AspNetCore.Mvc;
using SkincareBookingSystem.Models.Dto.Payment;
using SkincareBookingSystem.Models.Dto.Response;
using SkincareBookingSystem.Services.IServices;

namespace SkincareBookingSystem.API.Controllers;

[Route("api/payment")]
[ApiController]
public class PaymentController : ControllerBase
{
    private readonly IPaymentService _paymentService;

    public PaymentController(IPaymentService paymentService)
    {
        _paymentService = paymentService;
    }

    [HttpPost("create-link")]
    public async Task<ActionResult<ResponseDto>> CreatePaymentLink([FromBody] CreatePaymentLinkDto createPaymentLinkDTO)
    {
        var responseDto = await _paymentService.CreatePayOsPaymentLink(User,createPaymentLinkDTO);

        return StatusCode(responseDto.StatusCode, responseDto);
    }
    
    [HttpPost("confirm-transaction")]
    public async Task<ActionResult<ResponseDto>> ConfirmPayment([FromBody] ConfirmPaymentDto confirmPaymentDto)
    {
        var responseDto = await _paymentService.ConfirmPayOsTransaction(confirmPaymentDto);

        return StatusCode(responseDto.StatusCode, responseDto);
    }
}