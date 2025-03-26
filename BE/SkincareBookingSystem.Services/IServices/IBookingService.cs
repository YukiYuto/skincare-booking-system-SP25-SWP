using System.Security.Claims;
using SkincareBookingSystem.Models.Dto.Booking.Appointment;
using SkincareBookingSystem.Models.Dto.Booking.Appointment.RescheduleAppointment;
using SkincareBookingSystem.Models.Dto.Booking.Order;
using SkincareBookingSystem.Models.Dto.Response;

namespace SkincareBookingSystem.Services.IServices;

/// <summary>
///     A service for appointment booking logic
/// </summary>
public interface IBookingService
{
    Task<ResponseDto> GetTherapistsForServiceType(Guid serviceTypeId);
    Task<ResponseDto> GetOccupiedSlotsFromTherapist(Guid therapistId, DateOnly date);
    Task<ResponseDto> BundleOrder(BundleOrderDto bundleOrderDto, ClaimsPrincipal User);
    Task<ResponseDto> FinalizeAppointment(BookAppointmentDto bookAppointmentDto, ClaimsPrincipal User);
    Task<ResponseDto> HandleTherapistAutoAssignment(AutoAssignmentDto autoAssignmentDto);
    Task<ResponseDto> RescheduleAppointment(RescheduleAppointmentDto rescheduleRequest, ClaimsPrincipal User);
    Task<ResponseDto> CancelAppointment(CancelAppointmentDto cancelAppointmentDto, ClaimsPrincipal User);
    Task<ResponseDto> CompletedService(ClaimsPrincipal User, CompletedServiceDto completedServiceDto);
}