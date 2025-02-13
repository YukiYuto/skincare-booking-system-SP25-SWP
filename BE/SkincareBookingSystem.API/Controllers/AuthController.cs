using Microsoft.AspNetCore.Mvc;
using SkincareBookingSystem.Models.Dto.Authentication;
using SkincareBookingSystem.Models.Dto.Response;
using SkincareBookingSystem.Services.IServices;
using SkincareBookingSystem.Models.Dto.Email;

namespace SkincareBookingSystem.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

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

        [HttpPost("signin")]
        public async Task<ActionResult<ResponseDto>> SignIn([FromBody] SignInDto signInDto)
        {
            var responseDto = await _authService.SignIn(signInDto);
            return StatusCode(responseDto.StatusCode, responseDto);
        }

        [HttpGet("user")]
        public async Task<IActionResult> GetUserByToken(string token)
        {
            var responseDto = await _authService.FetchUserByToken(token);
            return StatusCode(responseDto.StatusCode, responseDto);
        }


        [HttpPost("password/change")]
        public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordDto changePasswordDto)
        {
            var responseDto = await _authService.ChangePassword(changePasswordDto);
            return responseDto.IsSuccess ? Ok(responseDto.Message) : BadRequest(responseDto.Message);
        }

        [HttpPost("password/forgot")]
        public async Task<IActionResult> ForgotPassword([FromBody] EmailDto forgotPasswordDto)
        {
            var responseDto = await _authService.ForgotPassword(forgotPasswordDto);
            return StatusCode(responseDto.StatusCode, responseDto);
        }

        [HttpPost("password/reset")]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordDto resetPasswordDto)
        {
            var responseDto = await _authService.ResetPassword(resetPasswordDto);
            return StatusCode(responseDto.StatusCode, responseDto);
        }


        [HttpPut("profile")]
        public async Task<IActionResult> UpdateUserProfile([FromBody] UpdateUserProfileDto updateUserProfileDto)
        {
            var responseDto = await _authService.UpdateUserProfile(User, updateUserProfileDto);
            return StatusCode(responseDto.StatusCode, responseDto);
        }


        [HttpPost("email/verification/send")]
        public async Task<IActionResult> SendVerifyEmail([FromBody] EmailDto emailDto)
        {
            var responseDto = await _authService.SendVerifyEmail(emailDto);
            return StatusCode(responseDto.StatusCode, responseDto);
        }

        [HttpPost("email/verification/confirm")]
        public async Task<IActionResult> VerifyEmail([FromBody] VerifyEmailDto verifyEmailDto)
        {
            var responseDto = await _authService.VerifyEmail(verifyEmailDto);
            return StatusCode(responseDto.StatusCode, responseDto);
        }
    }
}