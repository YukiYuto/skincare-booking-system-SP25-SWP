using System.Security.Claims;
using SkincareBookingSystem.Models.Dto.Authentication;
using SkincareBookingSystem.Models.Dto.Email;
using SkincareBookingSystem.Models.Dto.Response;

namespace SkincareBookingSystem.Services.IServices;

public interface IAuthService
{
    Task<ResponseDto> SignUpCustomer(SignUpCustomerDto signUpCustomerDto);
    Task<ResponseDto> SignUpStaff(SignUpStaffDto signUpStaffDto);
    Task<ResponseDto> SignUpSkinTherapist(SignUpSkinTherapistDto signUpSkinTherapistDto);
    Task<ResponseDto> SignIn(SignInDto signDto);
    Task<ResponseDto> SignInByGoogle(SignInByGoogleDto signInByGoogleDto);

    Task<ResponseDto> UpdateUserProfile(ClaimsPrincipal userPrincipal, UpdateUserProfileDto updateUserProfileDto);

    //Task<ResponseDto> GetUserById(Guid userId);
    Task<ResponseDto> RefreshAccessToken(RefreshTokenDto refreshTokenDto);
    Task<ResponseDto> FetchUserByToken(ClaimsPrincipal user);
    Task<ResponseDto> SendVerifyEmail(EmailDto emailDto);
    Task<ResponseDto> VerifyEmail(VerifyEmailDto verifyEmailDto);
    Task<ResponseDto> ChangePassword(ChangePasswordDto changePasswordDto);
    Task<ResponseDto> ForgotPassword(EmailDto forgotPasswordDto);
    Task<ResponseDto> ResetPassword(ResetPasswordDto resetPasswordDto);
}