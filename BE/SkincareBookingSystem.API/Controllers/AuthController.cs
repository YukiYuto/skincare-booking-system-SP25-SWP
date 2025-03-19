using Microsoft.AspNetCore.Mvc;
using SkincareBookingSystem.Models.Dto.Authentication;
using SkincareBookingSystem.Models.Dto.Email;
using SkincareBookingSystem.Models.Dto.Response;
using SkincareBookingSystem.Services.IServices;
using Swashbuckle.AspNetCore.Annotations;

namespace SkincareBookingSystem.API.Controllers;

[ApiController]
[Route("api/auth")]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;

    public AuthController(IAuthService authService)
    {
        _authService = authService;
    }


    /// <summary>
    ///     API register new customer
    /// </summary>
    /// <param name="signUpCustomerDto">Customer register information</param>
    /// <returns>ResponseDto include result</returns>
    [HttpPost("customers")]
    [SwaggerOperation(Summary = "API creates a new customer account", Description = "Requires customer role")]
    [ProducesResponseType(typeof(ResponseDto), 201)]
    [ProducesResponseType(typeof(ResponseDto), 400)]
    public async Task<ActionResult<ResponseDto>> SignUpCustomer([FromBody] SignUpCustomerDto signUpCustomerDto)
    {
        if (!ModelState.IsValid)
            return BadRequest(new ResponseDto
            {
                IsSuccess = false,
                Message = "Invalid input data.",
                Result = ModelState.Values.SelectMany(v => v.Errors.Select(e => e.ErrorMessage))
            });

        var result = await _authService.SignUpCustomer(signUpCustomerDto);
        return result.IsSuccess
            ? CreatedAtAction(nameof(SignUpCustomer), result.Result, result)
            : BadRequest(result);
    }


    /// <summary>
    ///     API register new therapist
    /// </summary>
    /// <param name="signUpSkinTherapistDto">Therapist register information</param>
    /// <returns>ResponseDto include result</returns>
    [HttpPost("skin-therapists")]
    [SwaggerOperation(Summary = "API creates a new skin therapist account", Description = "Requires therapist role")]
    public async Task<ActionResult<ResponseDto>> SignUpSkinTherapist(
        [FromBody] SignUpSkinTherapistDto signUpSkinTherapistDto)
    {
        if (!ModelState.IsValid)
            return BadRequest(new ResponseDto
            {
                IsSuccess = false,
                Message = "Invalid input data.",
                Result = ModelState.Values.SelectMany(v => v.Errors.Select(e => e.ErrorMessage))
            });

        var result = await _authService.SignUpSkinTherapist(signUpSkinTherapistDto);
        return result.IsSuccess
            ? CreatedAtAction(nameof(SignUpSkinTherapist), result.Result, result)
            : BadRequest(result);
    }

    [HttpPost("staff")]
    [SwaggerOperation(Summary = "API creates new a staff account", Description = "Requires staff role")]
    public async Task<ActionResult<ResponseDto>> SignUpStaff([FromBody] SignUpStaffDto signUpStaffDto)
    {
        if (!ModelState.IsValid)
            return BadRequest(new ResponseDto
            {
                IsSuccess = false,
                Message = "Invalid input data.",
                Result = ModelState.Values.SelectMany(v => v.Errors.Select(e => e.ErrorMessage))
            });

        var result = await _authService.SignUpStaff(signUpStaffDto);
        return result.IsSuccess
            ? CreatedAtAction(nameof(SignUpStaff), result.Result, result)
            : BadRequest(result);
    }

    [HttpPost("signin")]
    [SwaggerOperation(Summary = "API signs in available account",
        Description = "Requires customer's, therapist's, staff's  account")]
    public async Task<ActionResult<ResponseDto>> SignIn([FromBody] SignInDto signInDto)
    {
        var responseDto = await _authService.SignIn(signInDto);
        return StatusCode(responseDto.StatusCode, responseDto);
    }

    [HttpGet("user")]
    [SwaggerOperation(Summary = "API gets user token", Description = "Requires customer's, therapist's, staff's token")]
    public async Task<IActionResult> GetUserByToken()
    {
        var responseDto = await _authService.FetchUserByToken(User);
        return StatusCode(responseDto.StatusCode, responseDto);
    }

    [HttpPost("refresh-token")]
    [SwaggerOperation(Summary = "API refreshes access token",
        Description = "Requires customer's, therapist's, staff's refresh token")]
    public async Task<IActionResult> RefreshAccessToken([FromBody] RefreshTokenDto refreshTokenDto)
    {
        var responseDto = await _authService.RefreshAccessToken(refreshTokenDto);
        return StatusCode(responseDto.StatusCode, responseDto);
    }

    [HttpPost("password/change")]
    [SwaggerOperation(Summary = "API changes available account's pasword",
        Description = "Requires customer's, therapist's, staff's  account")]
    public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordDto changePasswordDto)
    {
        var responseDto = await _authService.ChangePassword(changePasswordDto);
        return StatusCode(responseDto.StatusCode, responseDto);
    }

    [HttpPost("password/forgot")]
    [SwaggerOperation(Summary = "API sends forgot available account's pasword email",
        Description = "Requires customer's, therapist's, staff's  account")]
    public async Task<IActionResult> ForgotPassword([FromBody] EmailDto forgotPasswordDto)
    {
        var responseDto = await _authService.ForgotPassword(forgotPasswordDto);
        return StatusCode(responseDto.StatusCode, responseDto);
    }

    [HttpPost("password/reset")]
    [SwaggerOperation(Summary = "API resets available account's pasword",
        Description = "Requires customer's, therapist's, staff's  account")]
    public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordDto resetPasswordDto)
    {
        var responseDto = await _authService.ResetPassword(resetPasswordDto);
        return StatusCode(responseDto.StatusCode, responseDto);
    }


    [HttpPut("profile")]
    [SwaggerOperation(Summary = "API updates user profile",
        Description = "Requires customer's, therapist's, staff's  account")]
    public async Task<IActionResult> UpdateUserProfile([FromBody] UpdateUserProfileDto updateUserProfileDto)
    {
        var responseDto = await _authService.UpdateUserProfile(User, updateUserProfileDto);
        return StatusCode(responseDto.StatusCode, responseDto);
    }


    [HttpPost("email/verification/send")]
    [SwaggerOperation(Summary = "API sends verification email",
        Description = "Requires customer's, therapist's, staff's  account")]
    public async Task<IActionResult> SendVerifyEmail([FromBody] EmailDto emailDto)
    {
        var responseDto = await _authService.SendVerifyEmail(emailDto);
        return StatusCode(responseDto.StatusCode, responseDto);
    }

    [HttpPost("email/verification/confirm")]
    [SwaggerOperation(Summary = "API verifies email",
        Description = "Requires customer's, therapist's, staff's  account")]
    public async Task<IActionResult> VerifyEmail([FromBody] VerifyEmailDto verifyEmailDto)
    {
        var responseDto = await _authService.VerifyEmail(verifyEmailDto);
        return StatusCode(responseDto.StatusCode, responseDto);
    }
}