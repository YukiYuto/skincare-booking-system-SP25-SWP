using SkincareBookingSystem.Models.Dto.Booking.Order;
using SkincareBookingSystem.Models.Dto.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace SkincareBookingSystem.Services.IServices
{
    /// <summary>
    /// A service for appointment booking logic
    /// </summary>
    public interface IBookingService
    {
        Task<ResponseDto> GetTherapistsForServiceType(Guid serviceTypeId);
        Task<ResponseDto> GetOccupiedSlotsFromTherapist(Guid therapistId, DateTime date);
        Task<ResponseDto> BundleOrder(BundleOrderDto bundleOrderDto, ClaimsPrincipal User);

    }
}
