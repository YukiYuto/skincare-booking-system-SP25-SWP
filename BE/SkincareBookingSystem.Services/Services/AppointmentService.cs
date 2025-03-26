using SkincareBookingSystem.DataAccess.IRepositories;
using SkincareBookingSystem.Models.Domain;
using SkincareBookingSystem.Models.Dto.Appointment;
using SkincareBookingSystem.Models.Dto.Response;
using SkincareBookingSystem.Services.IServices;
using SkincareBookingSystem.Utilities.Constants;
using System.Security.Claims;
using SkincareBookingSystem.Services.Helpers.Responses;
using SkincareBookingSystem.Models.Dto.Appointment.Details;
using SkincareBookingSystem.Services.Helpers.Users;

namespace SkincareBookingSystem.Services.Services
{
    /// <summary>
    /// Appointment service class, handles all CRUD operations for appointments
    /// Statuses will be referenced from StaticOperationStatus.Appointment
    /// </summary>
    public class AppointmentService : IAppointmentService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IAutoMapperService _autoMapperService;

        public AppointmentService(IUnitOfWork unitOfWork, IAutoMapperService autoMapperService)
        {
            _unitOfWork = unitOfWork;
            _autoMapperService = autoMapperService;
        }
        public async Task<ResponseDto> CreateAppointment(ClaimsPrincipal user, CreateAppointmentDto appointmentDto)
        {
            var userId = user.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId))
            {
                return ErrorResponse.Build(
                    message: StaticResponseMessage.User.NotFound,
                    statusCode: StaticOperationStatus.StatusCode.NotFound);
            }

            var customerId = await _unitOfWork.Customer.GetAsync(c => c.UserId == userId);
            if (customerId is null)
            {
                return ErrorResponse.Build(
                    message: StaticResponseMessage.User.NotFound,
                    statusCode: StaticOperationStatus.StatusCode.NotFound);
            }

            var order = await _unitOfWork.Order.GetLatestOrderByCustomerIdAsync(customerId.CustomerId);
            if (order is null)
            {
                return ErrorResponse.Build(
                    message: StaticOperationStatus.Order.Message.NotFound,
                    statusCode: StaticOperationStatus.StatusCode.NotFound);
            }

            var payment = await _unitOfWork.Payment.GetAsync(p => p.OrderNumber == order.OrderNumber && p.Status == PaymentStatus.Paid);
            if (payment == null)
            {
                return ErrorResponse.Build(
                    message: StaticResponseMessage.Payment.Pending,
                    statusCode: StaticOperationStatus.StatusCode.BadRequest);
            }

            var appointmentToCreate = _autoMapperService.Map<CreateAppointmentDto, Appointments>(appointmentDto);
            appointmentToCreate.CustomerId = customerId.CustomerId;
            appointmentToCreate.CreatedBy = user.FindFirstValue("FullName");

            try
            {
                await _unitOfWork.Appointments.AddAsync(appointmentToCreate);
                await _unitOfWork.SaveAsync();
            }
            catch (Exception e)
            {
                return ErrorResponse.Build(
                    message: StaticResponseMessage.Appointment.NotCreated,
                    statusCode: StaticOperationStatus.StatusCode.InternalServerError);
            }
            return SuccessResponse.Build(
                message: StaticResponseMessage.Appointment.Created,
                statusCode: StaticOperationStatus.StatusCode.Ok,
                result: appointmentToCreate);
        }

        public async Task<ResponseDto> DeleteAppointment(ClaimsPrincipal user, Guid appointmentId)
        {
            if (user.FindFirstValue(ClaimTypes.NameIdentifier) is null)
            {
                return ErrorResponse.Build(
                    message: StaticResponseMessage.User.NotFound,
                    statusCode: StaticOperationStatus.StatusCode.NotFound);
            }
            var appointmentToDelete = await _unitOfWork.Appointments.GetAsync(a => a.AppointmentId == appointmentId);
            if (appointmentToDelete is null)
            {
                return ErrorResponse.Build(
                    message: StaticResponseMessage.Appointment.NotFound,
                    statusCode: StaticOperationStatus.StatusCode.NotFound);
            }

            appointmentToDelete.Status = StaticOperationStatus.Appointment.Deleted;
            appointmentToDelete.UpdatedTime = StaticOperationStatus.Timezone.Vietnam;
            appointmentToDelete.UpdatedBy = user.FindFirstValue("FullName");

            return (await SaveChangesAsync()) ?
                SuccessResponse.Build(
                    message: StaticResponseMessage.Appointment.Deleted,
                    statusCode: StaticOperationStatus.StatusCode.Ok,
                    result: appointmentToDelete)
                :
                ErrorResponse.Build(
                    message: StaticResponseMessage.Appointment.NotDeleted,
                    statusCode: StaticOperationStatus.StatusCode.InternalServerError);
        }

        // TODO: Implement GetAllAppointments with pagination, sorting, and filtering
        public async Task<ResponseDto> GetAllAppointments()
        {
            var appointmentsFromDb = await _unitOfWork.Appointments.GetAllAsync(
                filter: a => a.Status != StaticOperationStatus.Appointment.Deleted);

            return (appointmentsFromDb.Any()) ?
                SuccessResponse.Build(
                    message: StaticResponseMessage.Appointment.RetrievedAll,
                    statusCode: StaticOperationStatus.StatusCode.Ok,
                    result: appointmentsFromDb)
                :
                SuccessResponse.Build(
                    message: StaticResponseMessage.Appointment.NotFound,
                    statusCode: StaticOperationStatus.StatusCode.Ok,
                    result: new List<Appointments>());
        }

        public async Task<ResponseDto> GetAppointmentById(ClaimsPrincipal user, Guid appointmentId)
        {
            if (UserError.NotExists(user))
            {
                return ErrorResponse.Build(
                    message: StaticResponseMessage.User.NotFound,
                    statusCode: StaticOperationStatus.StatusCode.NotFound);
            }

            try
            {
                var appointmentFromDb = await _unitOfWork.Appointments.GetAsync(
                    filter: a => a.AppointmentId == appointmentId,
                    includeProperties:
                       $"{nameof(Appointments.Customer)}," +
                       $"{nameof(Appointments.Customer)}.{nameof(Customer.ApplicationUser)}," +
                       $"{nameof(Appointments.TherapistSchedules)}," +
                       $"{nameof(Appointments.TherapistSchedules)}.{nameof(TherapistSchedule.SkinTherapist)}," +
                       $"{nameof(Appointments.TherapistSchedules)}.{nameof(TherapistSchedule.SkinTherapist)}.{nameof(SkinTherapist.ApplicationUser)}," +
                       $"{nameof(Appointments.Order)}," +
                       $"{nameof(Appointments.Order)}.{nameof(Order.OrderDetails)}," +
                       $"{nameof(Appointments.Order)}.{nameof(Order.OrderDetails)}.{nameof(OrderDetail.Services)}," +
                       $"{nameof(Appointments.Order)}.{nameof(Order.OrderDetails)}.{nameof(OrderDetail.Services)}.{nameof(Models.Domain.Services.DurationItems)}," +
                       $"{nameof(Appointments.Order)}.{nameof(Order.OrderDetails)}.{nameof(OrderDetail.Services)}.{nameof(Models.Domain.Services.DurationItems)}.{nameof(DurationItem.ServiceDuration)}"
);

                if (appointmentFromDb is null)
                {
                    return ErrorResponse.Build(
                        message: StaticResponseMessage.Appointment.NotFound,
                        statusCode: StaticOperationStatus.StatusCode.NotFound);
                }

                var therapistInfo = appointmentFromDb.TherapistSchedules.Select(ts => ts.SkinTherapist).First();
                var serviceInfo = appointmentFromDb.Order.OrderDetails.Select(od => od.Services).First();

                var customerInfoDto = _autoMapperService.Map<Customer, CustomerInfoDto>(appointmentFromDb.Customer);
                var therapistInfoDto = _autoMapperService.Map<SkinTherapist, TherapistInfoDto>(therapistInfo);
                var serviceInfoDto = _autoMapperService.Map<Models.Domain.Services, ServiceInfoDto>(serviceInfo!);
                serviceInfoDto.ServiceDuration = appointmentFromDb.DurationMinutes;

                var appointmentDetailsDto = _autoMapperService.Map<Appointments, AppointmentDetailsDto>(appointmentFromDb);
                appointmentDetailsDto.CustomerInfo = customerInfoDto;
                appointmentDetailsDto.TherapistInfo = therapistInfoDto;
                appointmentDetailsDto.ServiceInfo = serviceInfoDto;
                appointmentDetailsDto.CheckInTime = appointmentFromDb.CheckInTime.ToString();
                appointmentDetailsDto.CheckOutTime = appointmentFromDb.CheckOutTime.ToString();

                return SuccessResponse.Build(
                    message: StaticResponseMessage.Appointment.Retrieved,
                    statusCode: StaticOperationStatus.StatusCode.Ok,
                    result: appointmentDetailsDto);
            }
            catch (Exception e)
            {
                return ErrorResponse.Build(
                    message: "An error occurred retrieving appointment details: " + e.Message,
                    statusCode: StaticOperationStatus.StatusCode.InternalServerError);
            }
        }

        public async Task<ResponseDto> GetAppointmentsByCustomer(ClaimsPrincipal User)
        {
            if (UserError.NotExists(User))
                return ErrorResponse.Build(
                    message: StaticResponseMessage.User.NotFound,
                    statusCode: StaticOperationStatus.StatusCode.NotFound);
            var customerFromDb = await _unitOfWork.Customer.GetAsync(
                filter: c => c.UserId == User.FindFirstValue(ClaimTypes.NameIdentifier));

            if (customerFromDb is null)
                return ErrorResponse.Build(
                    message: StaticResponseMessage.Customer.Invalid,
                    statusCode: StaticOperationStatus.StatusCode.BadRequest);

            var appointmentsFromDb = await _unitOfWork.Appointments.GetAllAsync(a => a.CustomerId == customerFromDb.CustomerId);

            return (appointmentsFromDb.Any()) ?
                SuccessResponse.Build(
                    message: StaticResponseMessage.Appointment.RetrievedAll,
                    statusCode: StaticOperationStatus.StatusCode.Ok,
                    result: appointmentsFromDb)
                :
                SuccessResponse.Build(
                    message: StaticResponseMessage.Appointment.NotFound,
                    statusCode: StaticOperationStatus.StatusCode.Ok,
                    result: new List<Appointments>());
        }

        public async Task<ResponseDto> UpdateAppointment(ClaimsPrincipal user, UpdateAppointmentDto appointmentDto)
        {
            if (user.FindFirstValue(ClaimTypes.NameIdentifier) is null)
            {
                return ErrorResponse.Build(
                    message: StaticResponseMessage.User.NotFound,
                    statusCode: StaticOperationStatus.StatusCode.NotFound);
            }

            var appointmentFromDb = await _unitOfWork.Appointments.GetAsync(a => a.AppointmentId == appointmentDto.AppointmentId);
            if (appointmentFromDb is null)
            {
                return ErrorResponse.Build(
                    message: StaticResponseMessage.Appointment.NotFound,
                    statusCode: StaticOperationStatus.StatusCode.NotFound);
            }

            var appointmentToUpdate = _autoMapperService.Map<UpdateAppointmentDto, Appointments>(appointmentDto);
            _unitOfWork.Appointments.Update(appointmentFromDb, appointmentToUpdate);

            return (!await SaveChangesAsync()) ?
                ErrorResponse.Build(
                    message: StaticResponseMessage.Appointment.NotUpdated,
                    statusCode: StaticOperationStatus.StatusCode.InternalServerError)
                :
                SuccessResponse.Build(
                    message: StaticResponseMessage.Appointment.Updated,
                    statusCode: StaticOperationStatus.StatusCode.Ok,
                    result: appointmentToUpdate);
        }

        private async Task<bool> SaveChangesAsync()
        {
            return await _unitOfWork.SaveAsync() == StaticOperationStatus.Database.Success;
        }
    }
}
