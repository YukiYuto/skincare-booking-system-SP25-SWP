using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SkincareBookingSystem.Models.Dto.Payment;
using SkincareBookingSystem.Models.Dto.Response;
using SkincareBookingSystem.Services.IServices;
using Swashbuckle.AspNetCore.Annotations;

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
    [SwaggerOperation(Summary = "API creates a payment link", Description = "Requires user role")]
    public async Task<ActionResult<ResponseDto>> CreatePaymentLink([FromBody] CreatePaymentLinkDto createPaymentLinkDTO)
    {
        var responseDto = await _paymentService.CreatePayOsPaymentLink(User, createPaymentLinkDTO);
        return StatusCode(responseDto.StatusCode, responseDto);
    }

    [HttpPost("confirm-transaction")]
    [SwaggerOperation(Summary = "API confirms a payment transaction", Description = "Requires order number")]
    public async Task<ActionResult<ResponseDto>> ConfirmPayment([FromBody] ConfirmPaymentDto confirmPaymentDto)
    {
        var responseDto = await _paymentService.ConfirmPayOsTransaction(confirmPaymentDto);
        return StatusCode(responseDto.StatusCode, responseDto);
    }

    [HttpGet]
    [Authorize]
    [SwaggerOperation(Summary = "API gets all payments", Description = "Requires admin role")]
    public async Task<ActionResult<ResponseDto>> GetAll
    (
        [FromQuery] 
        int pageNumber = 1,
        int pageSize = 10,
        string? filterOn = null,
        string? filterQuery = null,
        string? sortBy = null
    )
    {
        var responseDto = await _paymentService.GetAll(User,pageNumber, pageSize, filterOn, filterQuery, sortBy);
        return StatusCode(responseDto.StatusCode, responseDto);
    }

    [HttpGet("get-by-id")]
    [SwaggerOperation(Summary = "API gets a payment by id", Description = "Requires admin role")]
    public async Task<ActionResult<ResponseDto>> GetPaymentById(Guid paymentTransactionId)
    {
        var responseDto = await _paymentService.GetPaymentById(User, paymentTransactionId);
        return StatusCode(responseDto.StatusCode, responseDto);
    }
}