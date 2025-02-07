﻿using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using SkincareBookingSystem.Models.Domain;
using SkincareBookingSystem.Services.IServices;

namespace SkincareBookingSystem.Services.Services;

public class TokenService : ITokenService
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IConfiguration _configuration;

    public TokenService(UserManager<ApplicationUser> userManager, IConfiguration configuration)
    {
        _userManager = userManager;
        _configuration = configuration;
    }

    public async Task<string> GenerateJwtAccessTokenAsync(ApplicationUser user)
    {
        var userRoles = await _userManager.GetRolesAsync(user);
        var authClaims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, user.Id),
            new Claim(ClaimTypes.Name, user.UserName),
            new Claim(ClaimTypes.Email, user.Email),
            new Claim("FullName", user.FullName),
            new Claim("PhoneNumber", user.PhoneNumber),
            new Claim("Address", user.Address),
            new Claim("Age", user.Age.ToString()),
            new Claim("ImageUrl", user.ImageUrl)
        };

        // Thêm role của người dùng vào claims
        foreach (var role in userRoles)
        {
            authClaims.Add(new Claim(ClaimTypes.Role, role));
        }

        // Tạo security key và signing credentials
        var authSecret = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Secret"] ?? string.Empty));
        var signingCredentials = new SigningCredentials(authSecret, SecurityAlgorithms.HmacSha256);

        // Tạo đối tượng JWT token
        var tokenObject = new JwtSecurityToken(
            issuer: _configuration["JWT:ValidIssuer"],
            audience: _configuration["JWT:ValidAudience"],
            notBefore: DateTime.Now,
            expires: DateTime.Now.AddMinutes(60),
            claims: authClaims,
            signingCredentials: signingCredentials
        );

        // Tạo JWT access token
        var accessToken = new JwtSecurityTokenHandler().WriteToken(tokenObject);

        return accessToken;
    }

    public Task<string> GenerateJwtRefreshTokenAsync(ApplicationUser user)
    {
        // Create a list of claims containing user information
        var authClaims = new List<Claim>()
        {
            new Claim(ClaimTypes.NameIdentifier, user.Id),
        };

        // Create cryptographic objects for tokens
        var authSecret = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Secret"]));
        var signingCredentials = new SigningCredentials(authSecret, SecurityAlgorithms.HmacSha256);

        // Create JWT token object
        var tokenObject = new JwtSecurityToken(
            issuer: _configuration["JWT:ValidIssuer"],
            audience: _configuration["JWT:ValidAudience"],
            notBefore: DateTime.Now,
            expires: DateTime.Now.AddDays(3), //Expiration time is 3 days
            claims: authClaims,
            signingCredentials: signingCredentials
        );

        // Token generation successful
        var refreshToken = new JwtSecurityTokenHandler().WriteToken(tokenObject);

        return Task.FromResult(refreshToken);
    }
    
    public Task<bool> StoreRefreshToken(string userId, string refreshToken)
    {
        throw new NotImplementedException();
        
            /*string redisKey = $"userId:{userId}:refreshToken";
            var result = await _redisService.StoreString(redisKey, refreshToken);
            return true;*/
    }
    
}