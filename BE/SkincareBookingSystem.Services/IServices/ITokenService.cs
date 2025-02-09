using System.Security.Claims;
using SkincareBookingSystem.Models.Domain;

namespace SkincareBookingSystem.Services.IServices;

public interface ITokenService
{
    Task<string> GenerateJwtAccessTokenAsync(ApplicationUser user);
    Task<string> GenerateJwtRefreshTokenAsync(ApplicationUser user);
    Task<bool> StoreRefreshToken(string userId, string refreshToken);
    Task<ClaimsPrincipal> GetPrincipalFromToken(string token);
}