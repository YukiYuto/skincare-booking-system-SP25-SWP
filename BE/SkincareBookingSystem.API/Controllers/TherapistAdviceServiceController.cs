using Microsoft.AspNetCore.Mvc;
using SkincareBookingSystem.Models.Dto.Response;
using SkincareBookingSystem.Models.Dto.SkinTherapist;
using SkincareBookingSystem.Services.IServices;
using Swashbuckle.AspNetCore.Annotations;

namespace SkincareBookingSystem.API.Controllers;

[Route("api/therapist-advice")]
[ApiController]
public class TherapistAdviceServiceController : ControllerBase
{
    private readonly ITherapistAdviceService _therapistAdviceService;
    
    public TherapistAdviceServiceController(ITherapistAdviceService therapistAdviceService)
    {
        _therapistAdviceService = therapistAdviceService;
    }
    
    [HttpPost]
    [SwaggerOperation(Summary = "Create a Therapist Advice", Description = "Requires therapist role")]
    public async Task<ActionResult<ResponseDto>> CreateTherapistAdvice([FromBody] CreateTherapistAdviceDto therapistAdviceDto)
    {
        var result = await _therapistAdviceService.CreateTherapistAdvice(User, therapistAdviceDto);
        return StatusCode(result.StatusCode, result);
    }
    
    [HttpGet("{appointmentId}")]
    [SwaggerOperation(Summary = "Get Therapist Advice by appointmentId", Description = "Requires therapist role")]
    public async Task<ActionResult<ResponseDto>> GetTherapistAdviceByAppointmentId(Guid appointmentId)
    {
        var result = await _therapistAdviceService.GetTherapistAdviceByAppointment(User,appointmentId);
        return StatusCode(result.StatusCode, result);
    }
}