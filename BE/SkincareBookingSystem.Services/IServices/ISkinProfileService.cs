using SkincareBookingSystem.Models.Dto.Response;
using SkincareBookingSystem.Models.Dto.SkinProfile;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace SkincareBookingSystem.Services.IServices
{
    public interface ISkinProfileService
    {
        Task<ResponseDto> CreateSkinProfile (ClaimsPrincipal User, CreateSkinProfileDto createSkinProfileDto);
        Task<ResponseDto> UpdateSkinProfile(ClaimsPrincipal User, UpdateSkinProfileDto updateSkinProfileDto);
        Task<ResponseDto> GetSkinProfileById(ClaimsPrincipal User, Guid skinProfileId);
        Task<ResponseDto> GetAllSkinProfiles();
        Task<ResponseDto> DeleteSkinProfile(ClaimsPrincipal User, Guid skinProfileId);
    }
}
