using Microsoft.AspNetCore.Http;
using SkincareBookingSystem.Models.Dto.Authentication;
using SkincareBookingSystem.Models.Dto.Response;
using SkincareBookingSystem.Services.IServices;
using System.Security.Claims;
using Microsoft.AspNetCore.Identity;
using SkincareBookingSystem.Models.Domain;

namespace SkincareBookingSystem.Services.Services;

public class AuthService : IAuthService
{
    private readonly RoleManager<IdentityRole> _roleManager;
    private readonly UserManager<ApplicationUser> _userManager;
    
    public AuthService(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
    {
        _userManager = userManager;
        _roleManager = roleManager;
    }
    public Task<ResponseDto> SignUp(SignUpDto registerDto)
    {
        throw new NotImplementedException();
    }

    public Task<ResponseDto> SignIn(SignInDto signDto)
    {
        throw new NotImplementedException();
    }

    public Task<ResponseDto> SignInByGoogle(SignInByGoogleDto signInByGoogleDto)
    {
        throw new NotImplementedException();
    }
    public Task<ResponseDto> ChangePassword(string userId, string oldPassword, string newPassword, string confirmNewPassword)
    {
        throw new NotImplementedException();
    }

    public Task<ResponseDto> FetchUserByToken(string token)
    {
        throw new NotImplementedException();
    }

    public Task<ResponseDto> ForgotPassword(ForgotPasswordDto forgotPasswordDto)
    {
        throw new NotImplementedException();
    }

    public Task<MemoryStream> GetUserAvatar(ClaimsPrincipal user)
    {
        throw new NotImplementedException();
    }

    public Task<ResponseDto> GetUserById(Guid userId)
    {
        throw new NotImplementedException();
    }

    public Task<ResponseDto> LockUser(string id)
    {
        throw new NotImplementedException();
    }

    public Task<ResponseDto> RefreshToken(RefreshTokenDto refreshTokenDto)
    {
        throw new NotImplementedException();
    }

    public Task<ResponseDto> ResetPassword(string resetPasswordDto, string token, string password)
    {
        throw new NotImplementedException();
    }

    public Task<ResponseDto> SendVerifyEmail(string email, string userId, string token)
    {
        throw new NotImplementedException();
    }

    public Task<ResponseDto> UnlockUser(string id)
    {
        throw new NotImplementedException();
    }

    public Task<ResponseDto> UpdateUserProfile(string userId, UpdateUserProfileDto updateUserProfileDto)
    {
        throw new NotImplementedException();
    }

    public Task<ResponseDto> UploadUserAvatar(IFormFile file, ClaimsPrincipal user)
    {
        throw new NotImplementedException();
    }

    public Task<ResponseDto> VerifyEmail(string userId, string token)
    {
        throw new NotImplementedException();
    }
}