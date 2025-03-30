using SkincareBookingSystem.Models.Dto.Response;
using SkincareBookingSystem.Models.Dto.SkinTest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace SkincareBookingSystem.Services.IServices
{
    public interface ISkinTestService
    {
        Task<ResponseDto> CreateSkinTest(ClaimsPrincipal User, CreateSkinTestDto createSkinTestDto);
        Task<ResponseDto> UpdateSkinTest(ClaimsPrincipal User, UpdateSkinTestDto updateSkinTestDto);
        Task<ResponseDto> GetSkinTestById(ClaimsPrincipal User, Guid skinTestId);
        Task<ResponseDto> GetAllSkinTests();
        Task<ResponseDto> DeleteSkinTest(ClaimsPrincipal User, Guid skinTestId);
    }
}
