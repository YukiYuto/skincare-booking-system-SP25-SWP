using SkincareBookingSystem.Models.Dto.Response;
using SkincareBookingSystem.Models.Dto.TherapistServiceTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace SkincareBookingSystem.Services.IServices
{
    public interface ITherapistServiceTypeService
    {
        Task<ResponseDto> AssignServiceTypesToTherapist(
            ClaimsPrincipal User,
            TherapistServiceTypesDto therapistServiceTypesDto);
        Task<ResponseDto> RemoveServiceTypesForTherapist(
            ClaimsPrincipal User,
            TherapistServiceTypesDto therapistServiceTypesDto);

    }
}
