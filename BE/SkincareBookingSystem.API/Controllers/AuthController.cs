using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SkincareBookingSystem.Models.Domain;
using SkincareBookingSystem.Models.Dto.Authentication;
using SkincareBookingSystem.Models.Dto.Response;
using SkincareBookingSystem.Services.IServices;

namespace SkincareBookingSystem.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IAuthService _authService;

    public AuthController(UserManager<ApplicationUser> userManager, IAuthService authService)
    {
        _userManager = userManager;
        _authService = authService;
    }

    /// <summary>
    /// Sign up a customer
    /// </summary>
    /// <param name="signUpCustomerDto"></param>
    /// <returns></returns>
    [HttpPost("customers")]
    public async Task<ActionResult<ResponseDto>> SignUpCustomer([FromBody] SignUpCustomerDto signUpCustomerDto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(new ResponseDto
            {
                IsSuccess = false,
                Message = "Invalid input data.",
                Result = ModelState.Values.SelectMany(v => v.Errors.Select(e => e.ErrorMessage))
            });
        }

        var result = await _authService.SignUpCustomer(signUpCustomerDto);
        return result.IsSuccess ? Ok(result) : BadRequest(result);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="SignUpSkinTherapistDto"></param>
    /// <returns></returns>
    [HttpPost("skin-therapists")]
    public async Task<ActionResult<ResponseDto>> SignUpSkinTherapist(
        [FromBody] SignUpSkinTherapistDto signUpSkinTherapistDto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(new ResponseDto
            {
                IsSuccess = false,
                Message = "Invalid input data.",
                Result = ModelState.Values.SelectMany(v => v.Errors.Select(e => e.ErrorMessage))
            });
        }

        var result = await _authService.SignUpSkinTherapist(signUpSkinTherapistDto);
        return result.IsSuccess ? Ok(result) : BadRequest(result);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="signUpStaffDto"></param>
    /// <returns></returns>
    [HttpPost("staff")]
    public async Task<ActionResult<ResponseDto>> SignUpStaff([FromBody] SignUpStaffDto signUpStaffDto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(new ResponseDto
            {
                IsSuccess = false,
                Message = "Invalid input data.",
                Result = ModelState.Values.SelectMany(v => v.Errors.Select(e => e.ErrorMessage))
            });
        }

        var result = await _authService.SignUpStaff(signUpStaffDto);
        return result.IsSuccess ? Ok(result) : BadRequest(result);
    }

    /// <summary>
    /// Sign up a customer
    /// </summary>
    /// <param name="signUpCustomerDto"></param>
    /// <returns></returns>
    [HttpPost("sign-in")]
    public async Task<ActionResult<ResponseDto>> SignIn([FromBody] SignInDto signInDto)
    {
        var responseDto = await _authService.SignIn(signInDto);
        return StatusCode(responseDto.StatusCode, responseDto);
    }
    
    /// <summary>
    /// 
    /// </summary>
    /// <param name="token"></param>
    /// <returns></returns>
    [HttpGet("FetchUserByToken")]
    public async Task<IActionResult> FetchUserByToken(string token)
    {
        var responseDto = await _authService.FetchUserByToken(token);
        return StatusCode(responseDto.StatusCode, responseDto);
    }
}