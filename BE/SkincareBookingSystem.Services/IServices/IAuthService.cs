﻿using Microsoft.AspNetCore.Http;
using SkincareBookingSystem.Models.Dto.Authentication;
using SkincareBookingSystem.Models.Dto.Response;
using System.Security.Claims;

namespace SkincareBookingSystem.Services.IServices;

public interface IAuthService
{
    Task<ResponseDto> SignUp(SignUpDto registerDto);
    Task<ResponseDto> SignIn(SignInDto signDto);
    Task<ResponseDto> SignInByGoogle(SignInByGoogleDto signInByGoogleDto);
    Task<ResponseDto> UpdateUserProfile(string userId, UpdateUserProfileDto updateUserProfileDto);
    Task<ResponseDto> GetUserById(Guid userId);
    Task<ResponseDto> RefreshToken(RefreshTokenDto refreshTokenDto);
    Task<ResponseDto> FetchUserByToken(string token);
    Task<ResponseDto> UploadUserAvatar(IFormFile file, ClaimsPrincipal user);
    Task<MemoryStream> GetUserAvatar(ClaimsPrincipal user);
    Task<ResponseDto> SendVerifyEmail(string email, string userId, string token);
    Task<ResponseDto> VerifyEmail(string userId, string token);
    Task<ResponseDto> ChangePassword(string userId, string oldPassword, string newPassword, string confirmNewPassword);
    Task<ResponseDto> ForgotPassword(ForgotPasswordDto forgotPasswordDto);
    Task<ResponseDto> ResetPassword(string resetPasswordDto, string token, string password);
    Task<ResponseDto> LockUser(string id);
    Task<ResponseDto> UnlockUser(string id);
}