using System.Security.Claims;
using SkincareBookingSystem.Models.Dto.CustomerSkinTest;
using SkincareBookingSystem.Models.Dto.Response;

namespace SkincareBookingSystem.Services.IServices;

public interface ICustomerSkinTestService
{
    Task<ResponseDto> CreateCustomerSkinTest(ClaimsPrincipal user, CreateCustomerSkinTestDto createCustomerSkinTestDto);
}