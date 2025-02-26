﻿using Microsoft.AspNetCore.Http;
using SkincareBookingSystem.Models.Dto.Authentication;
using SkincareBookingSystem.Models.Dto.Response;
using System.Security.Claims;
using SkincareBookingSystem.Models.Dto.Email;

namespace SkincareBookingSystem.Services.IServices;

public interface IAuthService
{
    Task<ResponseDto> SignUpCustomer(SignUpCustomerDto signUpCustomerDto);
    Task<ResponseDto> SignUpStaff(SignUpStaffDto signUpStaffDto);
    Task<ResponseDto> SignUpSkinTherapist(SignUpSkinTherapistDto signUpSkinTherapistDto);
    Task<ResponseDto> SignIn(SignInDto signDto);
    Task<ResponseDto> SignInByGoogle(SignInByGoogleDto signInByGoogleDto);
    Task<ResponseDto> UpdateUserProfile(ClaimsPrincipal userPrincipal,UpdateUserProfileDto updateUserProfileDto);
    Task<ResponseDto> GetUserById(Guid userId);
    Task<ResponseDto> RefreshToken(RefreshTokenDto refreshTokenDto);
    Task<ResponseDto> FetchUserByToken(string token);
    Task<ResponseDto> UploadUserAvatar(IFormFile file, ClaimsPrincipal user);
    Task<MemoryStream> GetUserAvatar(ClaimsPrincipal user);
    Task<ResponseDto> SendVerifyEmail(EmailDto emailDto);
    Task<ResponseDto> VerifyEmail(VerifyEmailDto verifyEmailDto);
    Task<ResponseDto> ChangePassword(ChangePasswordDto changePasswordDto);
    Task<ResponseDto> ForgotPassword(EmailDto forgotPasswordDto);
    Task<ResponseDto> ResetPassword(ResetPasswordDto resetPasswordDto);
    Task<ResponseDto> LockUser(string id);
    Task<ResponseDto> UnlockUser(string id);
}