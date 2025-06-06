﻿using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Web;
using FirebaseAdmin.Auth;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SkincareBookingSystem.DataAccess.IRepositories;
using SkincareBookingSystem.Models.Domain;
using SkincareBookingSystem.Models.Dto.Authentication;
using SkincareBookingSystem.Models.Dto.Email;
using SkincareBookingSystem.Models.Dto.Response;
using SkincareBookingSystem.Services.Helpers.Responses;
using SkincareBookingSystem.Services.IServices;
using SkincareBookingSystem.Utilities.Constants;

namespace SkincareBookingSystem.Services.Services;

public class AuthService : IAuthService
{
    private readonly IEmailService _emailService;
    private readonly IAutoMapperService _mapperService;
    private readonly RoleManager<IdentityRole> _roleManager;
    private readonly JwtSecurityTokenHandler _tokenHandler;
    private readonly ITokenService _tokenService;
    private readonly IUnitOfWork _unitOfWork;
    private readonly UserManager<ApplicationUser> _userManager;

    public AuthService
    (
        UserManager<ApplicationUser> userManager,
        RoleManager<IdentityRole> roleManager,
        ITokenService tokenService,
        IUnitOfWork unitOfWork,
        IAutoMapperService mapperService,
        IEmailService emailService
    )
    {
        _userManager = userManager;
        _roleManager = roleManager;
        _tokenService = tokenService;
        _tokenHandler = new JwtSecurityTokenHandler();
        _unitOfWork = unitOfWork;
        _mapperService = mapperService;
        _emailService = emailService;
    }

    public async Task<ResponseDto> SignUpCustomer(SignUpCustomerDto signUpCustomerDto)
    {
        // Kiểm tra email đã tồn tại
        var isEmailExit = await _userManager.FindByEmailAsync(signUpCustomerDto.Email);
        if (isEmailExit is not null)
            return new ResponseDto
            {
                Message = "Email is being used by another user",
                Result = null,
                IsSuccess = false,
                StatusCode = 400
            };

        // Kiểm tra số điện thoại đã tồn tại
        var isPhoneNumberExit = await _userManager.Users
            .AnyAsync(u => u.PhoneNumber == signUpCustomerDto.PhoneNumber);
        if (isPhoneNumberExit)
            return new ResponseDto
            {
                Message = "Phone number is being used by another user",
                Result = null,
                IsSuccess = false,
                StatusCode = 400
            };

        // Tạo đối tượng ApplicationUser mới
        ApplicationUser newUser;
        newUser = _mapperService.Map<SignUpCustomerDto, ApplicationUser>(signUpCustomerDto);

        using (var transaction = await _unitOfWork.BeginTransactionAsync())
        {
            // Thêm người dùng mới vào database
            var createUserResult = await _userManager.CreateAsync(newUser, signUpCustomerDto.Password);

            // Kiểm tra lỗi khi tạo
            if (!createUserResult.Succeeded)
                return new ResponseDto
                {
                    Message = "Create user failed",
                    IsSuccess = false,
                    StatusCode = 400,
                    Result = null
                };

            Customer customer = new();
            customer = _mapperService.Map<SignUpCustomerDto, Customer>(signUpCustomerDto);
            customer.UserId = newUser.Id;

            var isRoleExist = await _roleManager.RoleExistsAsync(StaticUserRoles.Customer);

            if (!isRoleExist) await _roleManager.CreateAsync(new IdentityRole(StaticUserRoles.Customer));


            // Thêm role "Customer" cho người dùng
            var isRoleAdded = await _userManager.AddToRoleAsync(newUser, StaticUserRoles.Customer);

            if (!isRoleAdded.Succeeded)
                return new ResponseDto
                {
                    Message = "Error adding role",
                    IsSuccess = false,
                    StatusCode = 500,
                    Result = null
                };

            // Lưu thay đổi vào cơ sở dữ liệu
            await _unitOfWork.Customer.AddAsync(customer);
            await _unitOfWork.SaveAsync();

            await transaction.CommitAsync();

            return new ResponseDto
            {
                Message = "User created successfully",
                IsSuccess = true,
                StatusCode = 201,
                Result = new
                {
                    Email = newUser.Email,
                    FullName = newUser.FullName
                }
            };
        }
    }

    public async Task<ResponseDto> SignUpStaff(SignUpStaffDto signUpStaffDto)
    {
        // Kiểm tra email đã tồn tại
        var isEmailExit = await _userManager.FindByEmailAsync(signUpStaffDto.Email);
        if (isEmailExit is not null)
            return new ResponseDto
            {
                Message = "Email is being used by another user",
                Result = null,
                IsSuccess = false,
                StatusCode = 400
            };

        // Kiểm tra số điện thoại đã tồn tại
        var isPhoneNumberExit = await _userManager.Users
            .AnyAsync(u => u.PhoneNumber == signUpStaffDto.PhoneNumber);
        if (isPhoneNumberExit)
            return new ResponseDto
            {
                Message = "Phone number is being used by another user",
                Result = null,
                IsSuccess = false,
                StatusCode = 400
            };

        // Tạo đối tượng ApplicationUser mới
        ApplicationUser newUser;
        newUser = _mapperService.Map<SignUpStaffDto, ApplicationUser>(signUpStaffDto);
        newUser.EmailConfirmed = true;

        // Bắt đầu transaction qua UnitOfWork
        using (var transaction = await _unitOfWork.BeginTransactionAsync())
        {
            // Thêm người dùng mới vào database
            var createUserResult = await _userManager.CreateAsync(newUser, signUpStaffDto.Password);

            // Kiểm tra lỗi khi tạo
            if (!createUserResult.Succeeded)
                return new ResponseDto
                {
                    Message = "Create user failed",
                    IsSuccess = false,
                    StatusCode = 400,
                    Result = null
                };

            var staffCode = await _unitOfWork.Staff.GetNextStaffCodeAsync();
            Staff staff = new()
            {
                UserId = newUser.Id,
                StaffCode = staffCode
            };

            var isRoleExist = await _roleManager.RoleExistsAsync(StaticUserRoles.Staff);

            if (!isRoleExist) await _roleManager.CreateAsync(new IdentityRole(StaticUserRoles.Staff));

            // Thêm role "Customer" cho người dùng
            var isRoleAdded = await _userManager.AddToRoleAsync(newUser, StaticUserRoles.Staff);

            if (!isRoleAdded.Succeeded)
                return new ResponseDto
                {
                    Message = "Error adding role",
                    IsSuccess = false,
                    StatusCode = 500,
                    Result = null
                };

            // Lưu thay đổi vào cơ sở dữ liệu
            await _unitOfWork.Staff.AddAsync(staff);
            await _unitOfWork.SaveAsync();

            await transaction.CommitAsync();

            return new ResponseDto
            {
                Message = "User created successfully",
                IsSuccess = true,
                StatusCode = 201,
                Result = new
                {
                    Email = newUser.Email,
                    FullName = newUser.FullName
                }
            };
        }
    }


    public async Task<ResponseDto> SignUpSkinTherapist(SignUpSkinTherapistDto signUpSkinTherapistDto)
    {
        // Kiểm tra email đã tồn tại
        var isEmailExit = await _userManager.FindByEmailAsync(signUpSkinTherapistDto.Email);
        if (isEmailExit is not null)
            return new ResponseDto
            {
                Message = "Email is being used by another user",
                Result = null,
                IsSuccess = false,
                StatusCode = 400
            };

        // Kiểm tra số điện thoại đã tồn tại
        var isPhoneNumberExit = await _userManager.Users
            .AnyAsync(u => u.PhoneNumber == signUpSkinTherapistDto.PhoneNumber);
        if (isPhoneNumberExit)
            return new ResponseDto
            {
                Message = "Phone number is being used by another user",
                Result = null,
                IsSuccess = false,
                StatusCode = 400
            };

        // Tạo đối tượng ApplicationUser mới
        ApplicationUser newUser;
        newUser = _mapperService.Map<SignUpSkinTherapistDto, ApplicationUser>(signUpSkinTherapistDto);
        newUser.EmailConfirmed = true;

        using (var transaction = await _unitOfWork.BeginTransactionAsync())
        {
            // Thêm người dùng mới vào database
            var createUserResult = await _userManager.CreateAsync(newUser, signUpSkinTherapistDto.Password);

            // Kiểm tra lỗi khi tạo
            if (!createUserResult.Succeeded)
                return new ResponseDto
                {
                    Message = "Create user failed",
                    IsSuccess = false,
                    StatusCode = 400,
                    Result = null
                };

            var skinTherapist =
                _mapperService.Map<SignUpSkinTherapistDto, SkinTherapist>(signUpSkinTherapistDto);

            skinTherapist.UserId = newUser.Id;
            var isRoleExist = await _roleManager.RoleExistsAsync(StaticUserRoles.SkinTherapist);

            if (!isRoleExist) await _roleManager.CreateAsync(new IdentityRole(StaticUserRoles.SkinTherapist));

            // Thêm role "Customer" cho người dùng
            var isRoleAdded = await _userManager.AddToRoleAsync(newUser, StaticUserRoles.SkinTherapist);

            if (!isRoleAdded.Succeeded)
                return new ResponseDto
                {
                    Message = "Error adding role",
                    IsSuccess = false,
                    StatusCode = 500,
                    Result = null
                };

            // Lưu thay đổi vào cơ sở dữ liệu
            await _unitOfWork.SkinTherapist.AddAsync(skinTherapist);
            await _unitOfWork.SaveAsync();

            await transaction.CommitAsync();

            return new ResponseDto
            {
                Message = "User created successfully",
                IsSuccess = true,
                StatusCode = 201,
                Result = new
                {
                    Email = newUser.Email,
                    FullName = newUser.FullName
                }
            };
        }
    }

    public async Task<ResponseDto> SignIn(SignInDto signInDto)
    {
        var user = await _userManager.FindByEmailAsync(signInDto.Email);
        if (user == null)
            return new ResponseDto
            {
                Message = "User does not exist!",
                Result = null,
                IsSuccess = false,
                StatusCode = 404
            };

        var isPasswordCorrect = await _userManager.CheckPasswordAsync(user, signInDto.Password);

        if (!isPasswordCorrect)
            return new ResponseDto
            {
                Message = "Incorrect email or password",
                Result = null,
                IsSuccess = false,
                StatusCode = 400
            };

        if (!user.EmailConfirmed)
            return new ResponseDto
            {
                Message = "You need to confirm email!",
                Result = null,
                IsSuccess = false,
                StatusCode = 401
            };

        if (user.LockoutEnd is not null)
            return new ResponseDto
            {
                Message = "User has been locked",
                IsSuccess = false,
                StatusCode = 403,
                Result = null
            };

        var accessToken = await _tokenService.GenerateJwtAccessTokenAsync(user);
        var refreshToken = await _tokenService.GenerateJwtRefreshTokenAsync(user);
        await _tokenService.StoreRefreshToken(user.Id, refreshToken);

        return new ResponseDto
        {
            Result = new SignInResponseDto
            {
                AccessToken = accessToken,
                RefreshToken = refreshToken
            },
            Message = "Sign in successfully",
            IsSuccess = true,
            StatusCode = 200
        };
    }


    public async Task<ResponseDto> SignInByGoogle(SignInByGoogleDto signInByGoogleDto)
    {
        FirebaseToken googleToken =
            await FirebaseAuth.DefaultInstance.VerifyIdTokenAsync(signInByGoogleDto.Token);

        var userInfo = ExtractUserInfo(googleToken);

        var user = await _userManager.FindByEmailAsync(userInfo.Email);

        if (user is not null)
        {
            if (user.LockoutEnd is not null && user.LockoutEnd > DateTime.UtcNow)
            {
                return new ResponseDto()
                {
                    Message = "User has been locked",
                    IsSuccess = false,
                    StatusCode = 403,
                    Result = null
                };
            }
        }
        else
        {
            user = new ApplicationUser
            {
                Email = userInfo.Email,
                FullName = userInfo.Name,
                UserName = userInfo.Email,
                ImageUrl = userInfo.AvatarUrl,
                Address = userInfo.Address,
                Age = userInfo.BirthDate != string.Empty
                    ? DateTime.UtcNow.Year - DateTime.Parse(userInfo.BirthDate).Year
                    : 18,
                Gender = userInfo.Gender,
                PhoneNumber = userInfo.PhoneNumber,
                EmailConfirmed = true
            };

            var createUserResult = await _userManager.CreateAsync(user);
            if (!createUserResult.Succeeded)
            {
                return new ResponseDto()
                {
                    Message = "Failed to create user",
                    IsSuccess = false,
                    StatusCode = 500,
                    Result = createUserResult.Errors
                };
            }

            var newCustomer = new Customer()
            {
                UserId = user.Id,
            };
            
            await _unitOfWork.Customer.AddAsync(newCustomer);
            var isRoleExist = await _roleManager.RoleExistsAsync(StaticUserRoles.Customer);
            if (!isRoleExist) await _roleManager.CreateAsync(new IdentityRole(StaticUserRoles.Customer));

            var isRoleAdded = await _userManager.AddToRoleAsync(user, StaticUserRoles.Customer);
            if (!isRoleAdded.Succeeded)
                return new ResponseDto
                {
                    Message = "Error adding role",
                    IsSuccess = false,
                    StatusCode = 500,
                    Result = isRoleAdded.Errors
                };
            await _userManager.AddLoginAsync(user,
                new UserLoginInfo(StaticLoginProvider.Google, userInfo.UserId, "GOOGLE"));
        }

        var accessToken = await _tokenService.GenerateJwtAccessTokenAsync(user);
        var refreshToken = await _tokenService.GenerateJwtRefreshTokenAsync(user);
        await _tokenService.StoreRefreshToken(user.Id, refreshToken);
        await _userManager.UpdateAsync(user);

        return new ResponseDto()
        {
            Result = new SignInResponseDto
            {
                AccessToken = accessToken,
                RefreshToken = refreshToken
            },
            Message = "Sign in successfully",
            IsSuccess = true,
            StatusCode = 200
        };
    }

    // Extract user info from Firebase token
    private (string UserId, string Email, string Name, string AvatarUrl,
        string PhoneNumber, string BirthDate, string Gender, string Address)
        ExtractUserInfo(FirebaseToken token)
    {
        token.Claims.TryGetValue("phone_number", out var phoneNumber);
        token.Claims.TryGetValue("birthday", out var birthDate);
        token.Claims.TryGetValue("gender", out var gender);
        token.Claims.TryGetValue("address", out var address);

        return (
            token.Uid,
            token.Claims["email"].ToString()!,
            token.Claims["name"].ToString()!,
            token.Claims["picture"].ToString()!,
            phoneNumber?.ToString() ?? string.Empty,
            birthDate?.ToString() ?? string.Empty,
            gender?.ToString() ?? string.Empty,
            address?.ToString() ?? string.Empty
        );
    }

    public async Task<ResponseDto> UpdateUserProfile(ClaimsPrincipal userPrincipal,
        UpdateUserProfileDto updateUserProfileDto)
    {
        // Lấy thông tin người dùng từ token JWT
        var userId = userPrincipal.FindFirstValue(ClaimTypes.NameIdentifier);
        if (string.IsNullOrEmpty(userId))
            return new ResponseDto
            {
                Message = "Unauthorized",
                StatusCode = 401,
                IsSuccess = false,
                Result = null
            };

        var user = await _userManager.FindByIdAsync(userId);
        if (user == null)
            return new ResponseDto
            {
                Message = "Invalid user",
                StatusCode = 400,
                IsSuccess = false,
                Result = null
            };

        // Sử dụng mapping với overload có destination để cập nhật đối tượng user hiện có
        _mapperService.Map(updateUserProfileDto, user);

        var result = await _userManager.UpdateAsync(user);
        if (!result.Succeeded)
            return new ResponseDto
            {
                Message = "Update user profile failed",
                StatusCode = 400,
                IsSuccess = false,
                Result = result.Errors
            };

        // Nếu muốn trả về dữ liệu cập nhật, bạn có thể map lại đối tượng user sang DTO trả về
        var updatedUserDto = _mapperService.Map<ApplicationUser, UpdateUserProfileDto>(user);
        return new ResponseDto
        {
            Message = "Update user profile successfully",
            StatusCode = 200,
            IsSuccess = true,
            Result = updatedUserDto
        };
    }


    public async Task<ResponseDto> ChangePassword(ChangePasswordDto changePasswordDto)
    {
        // Lấy id của người dùng
        var user = await _userManager.FindByIdAsync(changePasswordDto.UserId);
        if (user == null) return new ResponseDto { IsSuccess = false, Message = "User not found." };

        // Không cho phép thay đổi mật khẩu cũ
        if (changePasswordDto.NewPassword == changePasswordDto.OldPassword)
            return new ResponseDto
                { IsSuccess = false, Message = "New password cannot be the same as the old password." };

        // Thực hiện thay đổi mật khẩu
        var result =
            await _userManager.ChangePasswordAsync(user, changePasswordDto.OldPassword, changePasswordDto.NewPassword);
        if (result.Succeeded) return new ResponseDto { IsSuccess = true, Message = "Password changed successfully." };

        return new ResponseDto
        {
            IsSuccess = false,
            Message = "Password change failed. Please ensure the old password is correct."
        };
    }

    public async Task<ResponseDto> FetchUserByToken(ClaimsPrincipal principal)
    {
        var userId = principal.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        var user = await _userManager.FindByIdAsync(userId!);

        if (user == null)
            return new ResponseDto
            {
                Message = "Invalid user",
                StatusCode = 400,
                IsSuccess = false,
                Result = null
            };

        // Lấy role từ UserManager
        var roles = await _userManager.GetRolesAsync(user);

        // Tạo GetUserDto từ claims
        var userDto = new GetUserDto
        {
            Id = user.Id,
            FullName = principal.FindFirst("FullName")!.Value,
            Email = user.Email!,
            PhoneNumber = user.PhoneNumber!,
            Address = principal.FindFirst("Address")?.Value,
            Gender = user.Gender,
            ImageUrl = principal.FindFirst("ImageUrl")?.Value,
            UserName = user.UserName!,
            Age = user.Age,
            Roles = roles.ToList()
        };

        return new ResponseDto
        {
            Message = "Get info successfully",
            StatusCode = 200,
            IsSuccess = true,
            Result = userDto
        };
    }


    public async Task<ResponseDto> SendVerifyEmail(EmailDto emailDto)
    {
        // Tìm user theo email
        var user = await _userManager.FindByEmailAsync(emailDto.Email);
        if (user == null)
            return new ResponseDto
            {
                IsSuccess = false,
                Message = "User not found",
                StatusCode = 404,
                Result = null
            };

        // Nếu email đã được xác nhận
        if (user.EmailConfirmed)
            return new ResponseDto
            {
                IsSuccess = true,
                Message = "Your email has already been confirmed",
                StatusCode = 200,
                Result = null
            };

        // Sinh token xác nhận email
        var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);

        // Xây dựng liên kết xác thực.
        // Lưu ý: thay đổi URL cho phù hợp với môi trường (local hay production)
        var verificationLink =
            $"https://lumiconnect-beauty.vercel.app/verify-email?userId={user.Id}&token={Uri.EscapeDataString(token)}";

        // Gọi EmailService để gửi email xác thực sử dụng template VerificationEmailTemplate
        var emailSent = await _emailService.SendVerificationEmailAsync(user.Email!, verificationLink, user.FullName);

        if (emailSent)
            return new ResponseDto
            {
                IsSuccess = true,
                Message = "Verification email sent successfully.",
                StatusCode = 200,
                Result = null
            };

        return new ResponseDto
        {
            IsSuccess = false,
            Message = "Failed to send verification email.",
            StatusCode = 500,
            Result = null
        };
    }


    public async Task<ResponseDto> VerifyEmail(VerifyEmailDto verifyEmailDto)
    {
        var user = await _userManager.FindByIdAsync(verifyEmailDto.UserId);

        if (user!.EmailConfirmed)
            return new ResponseDto
            {
                Message = "Your email has been confirmed!",
                IsSuccess = true,
                StatusCode = 200,
                Result = null
            };

        var decodedToken = Uri.UnescapeDataString(verifyEmailDto.Token);

        var confirmResult = await _userManager.ConfirmEmailAsync(user, decodedToken);

        if (!confirmResult.Succeeded)
            return new ResponseDto
            {
                Message = "Invalid token",
                StatusCode = 400,
                IsSuccess = false,
                Result = null
            };

        return new ResponseDto
        {
            Message = "Confirm Email Successfully",
            IsSuccess = true,
            StatusCode = 200,
            Result = null
        };
    }

    public async Task<ResponseDto> ForgotPassword(EmailDto forgotPasswordDto)
    {
        var user = await _userManager.FindByEmailAsync(forgotPasswordDto.Email);

        if (user == null)
            return new ResponseDto
            {
                IsSuccess = true,
                Message = "No account found matching the provided email.",
                StatusCode = 400,
                Result = null
            };

        //token reset password
        var token = await _userManager.GeneratePasswordResetTokenAsync(user);

        // build link reset password
        // string resetLink = $"http://localhost:5173/reset-password?email={user.Email}&token={Uri.UnescapeDataString(token)}";
        var resetLink =
            $"https://lumiconnect-beauty.vercel.app/reset-password?email={user.Email}&token={HttpUtility.UrlEncode(token)}";

        var emailSent = await _emailService.SendPasswordResetEmailAsync(user.Email!, resetLink);

        if (emailSent)
            return new ResponseDto
            {
                IsSuccess = true,
                Message = "Password reset email sent successfully.",
                StatusCode = 200,
                Result = null
            };

        return new ResponseDto
        {
            IsSuccess = false,
            Message = "Failed to send password reset email.",
            StatusCode = 500,
            Result = null
        };
    }

    public async Task<ResponseDto> ResetPassword(ResetPasswordDto resetPasswordDto)
    {
        // Check if new password and confirm password match
        if (resetPasswordDto.NewPassword != resetPasswordDto.ConfirmPassword)
            return new ResponseDto
            {
                IsSuccess = false,
                Message = "New password and confirmation password do not match.",
                StatusCode = 400,
                Result = null
            };

        var user = await _userManager.FindByEmailAsync(resetPasswordDto.Email);
        if (user == null)
            return new ResponseDto
            {
                IsSuccess = false,
                Message = "User not found",
                StatusCode = 404,
                Result = null
            };

        var result = await _userManager.ResetPasswordAsync(user, resetPasswordDto.Token, resetPasswordDto.NewPassword);
        if (!result.Succeeded)
            return new ResponseDto
            {
                IsSuccess = false,
                Message = "Reset password failed",
                StatusCode = 400,
                Result = null
            };
        return new ResponseDto
        {
            IsSuccess = true,
            Message = "Password has been reset successfully.",
            StatusCode = 200,
            Result = null
        };
    }

    public async Task<ResponseDto> RefreshAccessToken(RefreshTokenDto refreshTokenDto)
    {
        var principal = await _tokenService.GetPrincipalFromToken(refreshTokenDto.RefreshToken);
        if (principal is null)
            ErrorResponse.Build(
                StaticOperationStatus.Token.TokenInvalid,
                StaticOperationStatus.StatusCode.Unauthorized);

        var userId = principal!.FindFirstValue(ClaimTypes.NameIdentifier);
        var userFromDb = await _userManager.FindByIdAsync(userId);

        if (userFromDb is null)
            ErrorResponse.Build(
                StaticOperationStatus.User.UserNotFound,
                StaticOperationStatus.StatusCode.NotFound);

        var storedRefreshToken = await _tokenService.RetrieveRefreshTokenAsync(userId);

        if (storedRefreshToken != refreshTokenDto.RefreshToken)
            ErrorResponse.Build(
                StaticOperationStatus.Token.TokenInvalid,
                StaticOperationStatus.StatusCode.Unauthorized);
        // New access token creation
        var newAccessToken = await _tokenService.GenerateJwtAccessTokenAsync(userFromDb);

        return SuccessResponse.Build(
            StaticOperationStatus.Token.TokenRefreshed,
            StaticOperationStatus.StatusCode.Ok,
            newAccessToken);
    }

    public Task<ResponseDto> LockUser(string id)
    {
        throw new NotImplementedException();
    }

    public Task<ResponseDto> UnlockUser(string id)
    {
        throw new NotImplementedException();
    }
}