using System.IdentityModel.Tokens.Jwt;
using Microsoft.AspNetCore.Http;
using SkincareBookingSystem.Models.Dto.Authentication;
using SkincareBookingSystem.Models.Dto.Response;
using SkincareBookingSystem.Services.IServices;
using System.Security.Claims;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SkincareBookingSystem.DataAccess.IRepositories;
using SkincareBookingSystem.Models.Domain;
using SkincareBookingSystem.Utilities.Constants;
using SkincareBookingSystem.Utilities.Generators;

namespace SkincareBookingSystem.Services.Services;

public class AuthService : IAuthService
{
    private readonly RoleManager<IdentityRole> _roleManager;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly ITokenService _tokenService;
    private readonly JwtSecurityTokenHandler _tokenHandler;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IAutoMapperService _mapperService;

    public AuthService
    (
        UserManager<ApplicationUser> userManager,
        RoleManager<IdentityRole> roleManager,
        ITokenService tokenService,
        IUnitOfWork unitOfWork,
        IAutoMapperService mapperService
    )
    {
        _userManager = userManager;
        _roleManager = roleManager;
        _tokenService = tokenService;
        _tokenHandler = new JwtSecurityTokenHandler();
        _unitOfWork = unitOfWork;
        _mapperService = mapperService;
    }

    public async Task<ResponseDto> SignUpCustomer(SignUpCustomerDto signUpCustomerDto)
    {
        // Kiểm tra email đã tồn tại
        var isEmailExit = await _userManager.FindByEmailAsync(signUpCustomerDto.Email);
        if (isEmailExit is not null)
        {
            return new ResponseDto()
            {
                Message = "Email is being used by another user",
                Result = signUpCustomerDto,
                IsSuccess = false,
                StatusCode = 400
            };
        }

        // Kiểm tra số điện thoại đã tồn tại
        var isPhoneNumberExit = await _userManager.Users
            .AnyAsync(u => u.PhoneNumber == signUpCustomerDto.PhoneNumber);
        if (isPhoneNumberExit)
        {
            return new ResponseDto()
            {
                Message = "Phone number is being used by another user",
                Result = signUpCustomerDto,
                IsSuccess = false,
                StatusCode = 400
            };
        }

        // Tạo đối tượng ApplicationUser mới
        ApplicationUser newUser;
        newUser = _mapperService.Map<SignUpCustomerDto, ApplicationUser>(signUpCustomerDto);

        // Thêm người dùng mới vào database
        var createUserResult = await _userManager.CreateAsync(newUser, signUpCustomerDto.Password);

        // Kiểm tra lỗi khi tạo
        if (!createUserResult.Succeeded)
        {
            return new ResponseDto()
            {
                Message = "Create user failed",
                IsSuccess = false,
                StatusCode = 400,
                Result = signUpCustomerDto
            };
        }
        
        Customer customer = new ();
        customer = _mapperService.Map<SignUpCustomerDto, Customer>(signUpCustomerDto);
        customer.UserId = newUser.Id; 

        var isRoleExist = await _roleManager.RoleExistsAsync(StaticUserRoles.Customer);

        if (!isRoleExist)
        {
            await _roleManager.CreateAsync(new IdentityRole(StaticUserRoles.Customer));
        }

        // Thêm role "Customer" cho người dùng
        var isRoleAdded = await _userManager.AddToRoleAsync(newUser, StaticUserRoles.Customer);

        if (!isRoleAdded.Succeeded)
        {
            return new ResponseDto()
            {
                Message = "Error adding role",
                IsSuccess = false,
                StatusCode = 500,
                Result = signUpCustomerDto
            };
        }

        // Lưu thay đổi vào cơ sở dữ liệu
        await _unitOfWork.Customer.AddAsync(customer);
        await _unitOfWork.SaveAsync();
        return new ResponseDto()
        {
            Message = "User created successfully",
            IsSuccess = true,
            StatusCode = 201,
            Result = signUpCustomerDto
        };
    }

    public async Task<ResponseDto> SignUpStaff(SignUpStaffDto signUpStaffDto)
    {
        // Kiểm tra email đã tồn tại
        var isEmailExit = await _userManager.FindByEmailAsync(signUpStaffDto.Email);
        if (isEmailExit is not null)
        {
            return new ResponseDto()
            {
                Message = "Email is being used by another user",
                Result = signUpStaffDto,
                IsSuccess = false,
                StatusCode = 400
            };
        }

        // Kiểm tra số điện thoại đã tồn tại
        var isPhoneNumberExit = await _userManager.Users
            .AnyAsync(u => u.PhoneNumber == signUpStaffDto.PhoneNumber);
        if (isPhoneNumberExit)
        {
            return new ResponseDto()
            {
                Message = "Phone number is being used by another user",
                Result = signUpStaffDto,
                IsSuccess = false,
                StatusCode = 400
            };
        }

        // Tạo đối tượng ApplicationUser mới
        ApplicationUser newUser;
        newUser = _mapperService.Map<SignUpStaffDto, ApplicationUser>(signUpStaffDto);

        // Thêm người dùng mới vào database
        var createUserResult = await _userManager.CreateAsync(newUser, signUpStaffDto.Password);

        // Kiểm tra lỗi khi tạo
        if (!createUserResult.Succeeded)
        {
            return new ResponseDto()
            {
                Message = "Create user failed",
                IsSuccess = false,
                StatusCode = 400,
                Result = signUpStaffDto
            };
        }
        
        Staff staff = new ()
        {
            UserId = newUser.Id,
            StaffCode = StaffCodeGenerator.GetStaffCode()
        };

        var isRoleExist = await _roleManager.RoleExistsAsync(StaticUserRoles.Staff);

        if (!isRoleExist)
        {
            await _roleManager.CreateAsync(new IdentityRole(StaticUserRoles.Staff));
        }

        // Thêm role "Customer" cho người dùng
        var isRoleAdded = await _userManager.AddToRoleAsync(newUser, StaticUserRoles.Staff);

        if (!isRoleAdded.Succeeded)
        {
            return new ResponseDto()
            {
                Message = "Error adding role",
                IsSuccess = false,
                StatusCode = 500,
                Result = signUpStaffDto
            };
        }

        // Lưu thay đổi vào cơ sở dữ liệu
        await _unitOfWork.Staff.AddAsync(staff);
        await _unitOfWork.SaveAsync();
        return new ResponseDto()
        {
            Message = "User created successfully",
            IsSuccess = true,
            StatusCode = 201,
            Result = signUpStaffDto
        };
    }


    public async Task<ResponseDto> SignUpSkinTherapist(SignUpSkinTherapistDto signUpSkinTherapistDto)
    {
        // Kiểm tra email đã tồn tại
        var isEmailExit = await _userManager.FindByEmailAsync(signUpSkinTherapistDto.Email);
        if (isEmailExit is not null)
        {
            return new ResponseDto()
            {
                Message = "Email is being used by another user",
                Result = signUpSkinTherapistDto,
                IsSuccess = false,
                StatusCode = 400
            };
        }

        // Kiểm tra số điện thoại đã tồn tại
        var isPhoneNumberExit = await _userManager.Users
            .AnyAsync(u => u.PhoneNumber == signUpSkinTherapistDto.PhoneNumber);
        if (isPhoneNumberExit)
        {
            return new ResponseDto()
            {
                Message = "Phone number is being used by another user",
                Result = signUpSkinTherapistDto,
                IsSuccess = false,
                StatusCode = 400
            };
        }

        // Tạo đối tượng ApplicationUser mới
        ApplicationUser newUser;
        newUser = _mapperService.Map<SignUpSkinTherapistDto, ApplicationUser>(signUpSkinTherapistDto);

        // Thêm người dùng mới vào database
        var createUserResult = await _userManager.CreateAsync(newUser, signUpSkinTherapistDto.Password);

        // Kiểm tra lỗi khi tạo
        if (!createUserResult.Succeeded)
        {
            return new ResponseDto()
            {
                Message = "Create user failed",
                IsSuccess = false,
                StatusCode = 400,
                Result = signUpSkinTherapistDto
            };
        }
        
        //var user = await _userManager.FindByEmailAsync(signUpSkinTherapistDto.Email);

         SkinTherapist skinTherapist = new ();
         skinTherapist = _mapperService.Map<SignUpSkinTherapistDto, SkinTherapist>(signUpSkinTherapistDto);
         skinTherapist.UserId = newUser.Id; 
        var isRoleExist = await _roleManager.RoleExistsAsync(StaticUserRoles.SkinTherapist);

        if (!isRoleExist)
        {
            await _roleManager.CreateAsync(new IdentityRole(StaticUserRoles.SkinTherapist));
        }

        // Thêm role "Customer" cho người dùng
        var isRoleAdded = await _userManager.AddToRoleAsync(newUser, StaticUserRoles.SkinTherapist);

        if (!isRoleAdded.Succeeded)
        {
            return new ResponseDto()
            {
                Message = "Error adding role",
                IsSuccess = false,
                StatusCode = 500,
                Result = signUpSkinTherapistDto
            };
        }

        // Lưu thay đổi vào cơ sở dữ liệu
        await _unitOfWork.SkinTherapist.AddAsync(skinTherapist);
        await _unitOfWork.SaveAsync();
        return new ResponseDto()
        {
            Message = "User created successfully",
            IsSuccess = true,
            StatusCode = 201,
            Result = signUpSkinTherapistDto
        };
    }

    public async Task<ResponseDto> SignIn(SignInDto signInDto)
    {
        var user = await _userManager.FindByEmailAsync(signInDto.Email);
        if (user == null)
        {
            return new ResponseDto()
            {
                Message = "User does not exist!",
                Result = null,
                IsSuccess = false,
                StatusCode = 404
            };
        }

        var isPasswordCorrect = await _userManager.CheckPasswordAsync(user, signInDto.Password);

        if (!isPasswordCorrect)
        {
            return new ResponseDto()
            {
                Message = "Incorrect email or password",
                Result = null,
                IsSuccess = false,
                StatusCode = 400
            };
        }

        if (!user.EmailConfirmed)
        {
            return new ResponseDto()
            {
                Message = "You need to confirm email!",
                Result = null,
                IsSuccess = false,
                StatusCode = 401
            };
        }

        if (user.LockoutEnd is not null)
        {
            return new ResponseDto()
            {
                Message = "User has been locked",
                IsSuccess = false,
                StatusCode = 403,
                Result = null
            };
        }

        var accessToken = await _tokenService.GenerateJwtAccessTokenAsync(user);
        var refreshToken = await _tokenService.GenerateJwtRefreshTokenAsync(user);
        await _tokenService.StoreRefreshToken(user.Id, refreshToken);

        return new ResponseDto()
        {
            Result = new SignInResponseDto()
            {
                AccessToken = accessToken,
                RefreshToken = refreshToken,
            },
            Message = "Sign in successfully",
            IsSuccess = true,
            StatusCode = 200
        };
    }


    public Task<ResponseDto> SignInByGoogle(SignInByGoogleDto signInByGoogleDto)
    {
        throw new NotImplementedException();
    }

    public Task<ResponseDto> ChangePassword(string userId, string oldPassword, string newPassword,
        string confirmNewPassword)
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