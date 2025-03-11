using SkincareBookingSystem.DataAccess.IRepositories;
using SkincareBookingSystem.Models.Domain;
using SkincareBookingSystem.Models.Dto.Booking.Appointment;
using SkincareBookingSystem.Models.Dto.Booking.Order;
using SkincareBookingSystem.Models.Dto.Booking.SkinTherapist;
using SkincareBookingSystem.Models.Dto.OrderDetails;
using SkincareBookingSystem.Models.Dto.Response;
using SkincareBookingSystem.Services.Helpers.Responses;
using static SkincareBookingSystem.Services.Helpers.Schedules.DateValidator;
using static SkincareBookingSystem.Services.Helpers.Schedules.ScheduleValidator;
using SkincareBookingSystem.Services.Helpers.Users;
using SkincareBookingSystem.Services.IServices;
using SkincareBookingSystem.Utilities.Constants;
using System.Security.Claims;
using SkincareBookingSystem.Models.Dto.BookingSchedule;
using SkincareBookingSystem.Models.Dto.Booking.Appointment.FinalizeAppointment;
using SkincareBookingSystem.Services.Helpers.Schedules;
using SkincareBookingSystem.Models.Dto.Booking.Appointment.RescheduleAppointment;

namespace SkincareBookingSystem.Services.Services
{
    public class BookingService : IBookingService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IAutoMapperService _autoMapperService;
        private readonly ITherapistScheduleService _therapistScheduleService;

        public BookingService
        (
            IUnitOfWork unitOfWork,
            IAutoMapperService autoMapperService,
            ITherapistScheduleService therapistScheduleService
        )
        {
            _unitOfWork = unitOfWork;
            _autoMapperService = autoMapperService;
            _therapistScheduleService = therapistScheduleService;
        }


        public async Task<ResponseDto> GetTherapistsForServiceType(Guid serviceTypeId)
        {
            var therapistsFromDb = await _unitOfWork.SkinTherapist.GetAllAsync(
                filter: st => st.TherapistServiceTypes.Any(tst => tst.ServiceTypeId == serviceTypeId),
                includeProperties: $"{nameof(SkinTherapist.ApplicationUser)},{nameof(SkinTherapist.TherapistServiceTypes)}");

            if (therapistsFromDb is null || !therapistsFromDb.Any())
                return ErrorResponse.Build(StaticOperationStatus.SkinTherapist.NotFound, StaticOperationStatus.StatusCode.NotFound);

            var therapistDtos = _autoMapperService.MapCollection<SkinTherapist, PreviewTherapistDto>(therapistsFromDb).ToList();

            return SuccessResponse.Build(
                message: StaticOperationStatus.SkinTherapist.Found,
                statusCode: StaticOperationStatus.StatusCode.Ok,
                result: therapistDtos);

        }

        public async Task<ResponseDto> GetOccupiedSlotsFromTherapist(Guid therapistId, DateOnly dateToSearch)
        {
            IEnumerable<Slot> occupiedSlotsFromDb = await _unitOfWork.Slot.GetAllAsync(
                filter: s => s.TherapistSchedules.Any(
                    ts => ts.TherapistId == therapistId && ts.Appointment.AppointmentDate == dateToSearch),
                includeProperties: $"{nameof(Slot.TherapistSchedules)},{nameof(Slot.TherapistSchedules)}.{nameof(TherapistSchedule.Appointment)}"
                );

            if (occupiedSlotsFromDb is null || !occupiedSlotsFromDb.Any())
                return SuccessResponse.Build(
                    StaticOperationStatus.Slot.NotFound,
                    StaticOperationStatus.StatusCode.Ok,
                    new List<Slot>());

            var occupiedSlotsDto = occupiedSlotsFromDb.Select(s => new
            {
                SlotId = s.SlotId,
                StartTime = s.StartTime,
                EndTime = s.EndTime
            });

            return SuccessResponse.Build(
                message: StaticOperationStatus.Slot.Found,
                statusCode: StaticOperationStatus.StatusCode.Ok,
                result: occupiedSlotsDto);
        }

        public async Task<ResponseDto> BundleOrder(BundleOrderDto bundleOrderDto, ClaimsPrincipal User)
        {
            if (UserError.NotExists(User))
                return ErrorResponse.Build(StaticOperationStatus.User.UserNotFound, StaticOperationStatus.StatusCode.NotFound);

            if (bundleOrderDto.OrderDetails is null || !bundleOrderDto.OrderDetails.Any())
                return ErrorResponse.Build(StaticOperationStatus.OrderDetail.EmptyList, StaticOperationStatus.StatusCode.BadRequest);

            await using var transaction = await _unitOfWork.BeginTransactionAsync();
            try
            {
                // Create Order
                var order = _autoMapperService.Map<BundleOrderDto, Order>(bundleOrderDto);
                order.OrderNumber = await _unitOfWork.Order.GenerateUniqueNumberAsync();
                order.CreatedBy = User.Identity?.Name;

                // Map OrderDetails and Assign OrderId
                var orderDetails = _autoMapperService.MapCollection<CreateOrderDetailDto, OrderDetail>(bundleOrderDto.OrderDetails).ToList();
                orderDetails.ForEach(detail =>
                {
                    detail.OrderId = order.OrderId;
                });
                order.TotalPrice = GetTotalPrice(orderDetails);

                //Lưu 1 lần xuống dưới Db
                await _unitOfWork.Order.AddAsync(order);
                await _unitOfWork.SaveAsync();

                await transaction.CommitAsync();

                // Custom response is created to avoid circular reference in the response JSON
                var orderResponseDto = _autoMapperService.Map<Order, OrderDto>(order);
                return SuccessResponse.Build(
                    message: StaticOperationStatus.Order.Created,
                    statusCode: StaticOperationStatus.StatusCode.Created,
                    result: orderResponseDto);
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                return ErrorResponse.Build(
                    message: $"{StaticOperationStatus.Order.Message.NotCreated}: {ex.Message}",
                    statusCode: StaticOperationStatus.StatusCode.InternalServerError);
            }
        }

        public async Task<ResponseDto> FinalizeAppointment(BookAppointmentDto bookingRequest, ClaimsPrincipal User)
        {
            if (UserError.NotExists(User))
                return ErrorResponse.Build(StaticResponseMessage.User.NotFound, StaticOperationStatus.StatusCode.NotFound);

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var customerFromDb = await _unitOfWork.Customer.GetAsync(c => c.UserId == userId).ConfigureAwait(false);

            if (customerFromDb is null)
                return ErrorResponse.Build(StaticResponseMessage.Customer.NotFound, StaticOperationStatus.StatusCode.NotFound);

            // 1. Check payment status by retrieving linked payment with the order number
            var paymentFromDb = await _unitOfWork.Payment
                .GetAsync(
                    filter: p => p.OrderNumber == bookingRequest.OrderNumber,
                    includeProperties: nameof(Payment.Orders))
                .ConfigureAwait(false);

            if (paymentFromDb is null || paymentFromDb.Status != PaymentStatus.Paid)
                return ErrorResponse.Build(StaticResponseMessage.Payment.NotCompleted, StaticOperationStatus.StatusCode.BadRequest);
            // 1.1. Check if the order in the payment is linked to the customer
            if (paymentFromDb.Orders?.CustomerId != customerFromDb.CustomerId)
                return ErrorResponse.Build(StaticResponseMessage.Customer.Invalid, StaticOperationStatus.StatusCode.BadRequest);

            // 2. Check slot availability
            var slotFromDb = await _unitOfWork.Slot
                .GetAsync(filter: s => s.SlotId == bookingRequest.SlotId)
                .ConfigureAwait(false);
            if (slotFromDb is null)
                return ErrorResponse.Build(StaticResponseMessage.Slot.InvalidSelected, StaticOperationStatus.StatusCode.BadRequest);

            // 3. Check if the therapist is already scheduled for the selected slot
            var existingTherapistSchedule = await _unitOfWork.TherapistSchedule
                .GetAsync(
                    filter: ts => IsScheduleExisting(ts, bookingRequest) && !IsScheduleDisabled(ts),
                    includeProperties: nameof(TherapistSchedule.Appointment))
                .ConfigureAwait(false);

            if (existingTherapistSchedule is not null)
                return ErrorResponse.Build(StaticResponseMessage.TherapistSchedule.AlreadyScheduled, StaticOperationStatus.StatusCode.BadRequest);

            await using var transaction = await _unitOfWork.BeginTransactionAsync();
            try
            {
                // 4. Create the appointment using validated data
                var appointmentToCreate = _autoMapperService.Map<BookAppointmentDto, Appointments>(bookingRequest);
                appointmentToCreate.OrderId = paymentFromDb.Orders!.OrderId;
                appointmentToCreate.CustomerId = customerFromDb.CustomerId;
                appointmentToCreate.CreatedBy = User.Identity?.Name;

                await _unitOfWork.Appointments.AddAsync(appointmentToCreate);
                await _unitOfWork.SaveAsync();
                // 5. Create the therapist schedule & set confirmation status
                var therapistScheduleToCreate = new CreateTherapistScheduleDto
                {
                    TherapistId = bookingRequest.TherapistId,
                    AppointmentId = appointmentToCreate.AppointmentId,
                    SlotId = bookingRequest.SlotId
                };

                var scheduleResponse = await _therapistScheduleService.CreateTherapistSchedule(User, therapistScheduleToCreate);
                if (!scheduleResponse.IsSuccess)
                    throw new Exception("Failed to create therapist schedule. " + scheduleResponse.Message);

                await transaction.CommitAsync();

                // 6. Return the customized response to avoid circular reference in the response JSON
                var appointmentResponseDto = _autoMapperService.Map<Appointments, AppointmentDto>(appointmentToCreate);
                var scheduleResponseDto = _autoMapperService.Map<TherapistSchedule, ScheduleDto>(
                    (TherapistSchedule)scheduleResponse.Result!);
                return SuccessResponse.Build(
                    message: StaticResponseMessage.Appointment.Created,
                    statusCode: StaticOperationStatus.StatusCode.Created,
                    result: new
                    {
                        appointmentResponseDto,
                        scheduleResponseDto
                    });
            }
            catch
            {
                await transaction.RollbackAsync();
                return ErrorResponse.Build(
                    message: StaticResponseMessage.Appointment.NotCreated,
                    statusCode: StaticOperationStatus.StatusCode.InternalServerError);
            }
        }

        public async Task<ResponseDto> HandleTherapistAutoAssignment(AutoAssignmentDto autoAssignmentDto)
        {
            var today = DateOnly.FromDateTime(DateTime.Today);
            var firstDayOfCurrentMonth = new DateOnly(today.Year, today.Month, day: 1);

            var availableTherapists = await _unitOfWork.SkinTherapist
                .GetAllAsync(
                    filter: (st => st.TherapistServiceTypes.Any(tst => tst.ServiceTypeId == autoAssignmentDto.ServiceTypeId) &&
                            !st.TherapistSchedules.Any(ts =>
                                ts.Appointment.AppointmentDate == autoAssignmentDto.AppointmentDate &&
                                ts.SlotId == autoAssignmentDto.SlotId)),
                    includeProperties: $"{nameof(SkinTherapist.TherapistServiceTypes)}," +
                                       $"{nameof(SkinTherapist.TherapistSchedules)}," +
                                       $"{nameof(SkinTherapist.TherapistSchedules)}.{nameof(TherapistSchedule.Appointment)},")
                .ConfigureAwait(false);

            if (availableTherapists is null || !availableTherapists.Any())
                return ErrorResponse.Build(
                    message: StaticResponseMessage.TherapistSchedule.NoTherapistForSlot,
                    statusCode: StaticOperationStatus.StatusCode.NotFound);

            // Retrieve the therapist with the least number of bookings (excluding deleted ones)
            var leastBookedTherapist = availableTherapists
                .OrderBy(t => t.TherapistSchedules?.Count(
                    ts => ts.Status == StaticOperationStatus.BookingSchedule.Completed && IsDateWithinRange(
                        dateToCheck: ts.Appointment.AppointmentDate,
                        startDate: firstDayOfCurrentMonth,
                        endDate: today)) ?? 0)
                .FirstOrDefault();

            if (leastBookedTherapist is null)
                return ErrorResponse.Build(
                    message: StaticResponseMessage.TherapistSchedule.NoTherapistForSlot,
                    statusCode: StaticOperationStatus.StatusCode.NotFound);

            return SuccessResponse.Build(
                message: StaticResponseMessage.TherapistSchedule.AutoAssignmentHandled,
                statusCode: StaticOperationStatus.StatusCode.Ok,
                result: leastBookedTherapist.SkinTherapistId);
        }

        public async Task<ResponseDto> RescheduleAppointment(RescheduleAppointmentDto rescheduleRequest, ClaimsPrincipal User)
        {
            if (UserError.NotExists(User))
                return ErrorResponse.Build(StaticResponseMessage.User.NotFound, StaticOperationStatus.StatusCode.NotFound);
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            // 1. Appointment validation
            // 1.1. Retrieve the appointment to reschedule
            var appointmentFromDb = await _unitOfWork.Appointments
                .GetAsync(filter: a => a.AppointmentId == rescheduleRequest.AppointmentId,
                          includeProperties: $"{nameof(Appointments.TherapistSchedules)}.{nameof(TherapistSchedule.Slot)},{nameof(Appointments.Customer)}")
                .ConfigureAwait(false);

            if (appointmentFromDb is null)
                return ErrorResponse.Build(StaticResponseMessage.Appointment.NotFound, StaticOperationStatus.StatusCode.NotFound);

            // 1.2. Check if the appointment is linked to the customer
            if (appointmentFromDb.Customer.UserId != userId)
                return ErrorResponse.Build(StaticResponseMessage.Appointment.NotMatchedToCustomer, StaticOperationStatus.StatusCode.BadRequest);

            // 1.3. Ensure the appointment is not already completed / cancelled 
            if (!IsScheduleReschedulable(appointmentFromDb.TherapistSchedules))
                return ErrorResponse.Build(StaticResponseMessage.Appointment.NotReschedulable, StaticOperationStatus.StatusCode.BadRequest);

            // 1.4. Ensure the appointment is not within the grace period
            if (IsWithinGracePeriod(appointmentFromDb))
                return ErrorResponse.Build(StaticResponseMessage.Appointment.RescheduleWithinGracePeriod, StaticOperationStatus.StatusCode.BadRequest);

            // 2. Slot validation
            var slotToReschedule = await _unitOfWork.Slot
                .GetAsync(filter: s => s.SlotId == rescheduleRequest.NewSlotId)
                .ConfigureAwait(false);

            if (slotToReschedule is null)
                return ErrorResponse.Build(StaticResponseMessage.Slot.InvalidSelected, StaticOperationStatus.StatusCode.BadRequest);

            // 3. Therapist schedule validation
            var therapistId = appointmentFromDb.TherapistSchedules.Last().TherapistId;
            var existingSchedule = await _unitOfWork.TherapistSchedule
                .GetAsync(filter: ts => ts.TherapistId == therapistId &&
                                  ts.SlotId == rescheduleRequest.NewSlotId &&
                                  ts.ScheduleStatus != ScheduleStatus.Rescheduled)
                .ConfigureAwait(false);

            if (existingSchedule is not null)
                return ErrorResponse.Build(StaticResponseMessage.TherapistSchedule.AlreadyScheduled, StaticOperationStatus.StatusCode.BadRequest);

            // 4. Update the appointment & therapist schedule
            await using var transaction = await _unitOfWork.BeginTransactionAsync();
            try
            {
                // 4.1. Mark all past schedules as rescheduled
                var pastSchedules = appointmentFromDb.TherapistSchedules
                    .Where(ts => ts.ScheduleStatus != ScheduleStatus.Rescheduled &&
                                 ts.ScheduleStatus != ScheduleStatus.Cancelled)
                    .ToList();

                foreach (var schedule in pastSchedules)
                {
                    schedule.ScheduleStatus = ScheduleStatus.Rescheduled;
                    schedule.Reason = rescheduleRequest.Reason ?? "Rescheduled by customer.";
                    schedule.UpdatedBy = User.Identity?.Name;
                    schedule.UpdatedTime = StaticOperationStatus.Timezone.Vietnam;

                    _unitOfWork.TherapistSchedule.Update(schedule, schedule);
                }

                // 4.2. Create new therapist schedule for the rescheduled appointment
                var newSchedule = new TherapistSchedule
                {
                    TherapistScheduleId = Guid.NewGuid(),
                    TherapistId = therapistId,
                    AppointmentId = appointmentFromDb.AppointmentId,
                    SlotId = rescheduleRequest.NewSlotId,
                    ScheduleStatus = ScheduleStatus.Pending,
                    CreatedBy = User.Identity?.Name,
                    CreatedTime = StaticOperationStatus.Timezone.Vietnam
                };

                await _unitOfWork.TherapistSchedule.AddAsync(newSchedule);
                await _unitOfWork.SaveAsync();
                await transaction.CommitAsync();

                // 5. Return the response with updated therapist schedule
                return SuccessResponse.Build(
                    message: StaticResponseMessage.Appointment.Rescheduled,
                    statusCode: StaticOperationStatus.StatusCode.Ok,
                    result: new
                    {
                        OldScheduleStatus = ScheduleStatus.Rescheduled,
                        NewSchedule = newSchedule
                    });
            }
            catch
            {
                await transaction.RollbackAsync();
                return ErrorResponse.Build(
                    message: StaticResponseMessage.Appointment.NotRescheduled,
                    statusCode: StaticOperationStatus.StatusCode.InternalServerError);

            }
        }

        public async Task<ResponseDto> CancelAppointment(Guid appointmentId, ClaimsPrincipal User)
        {
            if (UserError.NotExists(User))
                return ErrorResponse.Build(StaticResponseMessage.User.NotFound, StaticOperationStatus.StatusCode.NotFound);

            var appointmentFromDb = await _unitOfWork.Appointments
                .GetAsync(filter: a => a.AppointmentId == appointmentId,
                          includeProperties: $"{nameof(Appointments.TherapistSchedules)},{nameof(Appointments.Customer)}")
                .ConfigureAwait(false);

            if (appointmentFromDb is null)
                return ErrorResponse.Build(StaticResponseMessage.Appointment.NotFound, StaticOperationStatus.StatusCode.NotFound);

            if (appointmentFromDb.Customer.UserId != User.FindFirstValue(ClaimTypes.NameIdentifier))
                return ErrorResponse.Build(StaticResponseMessage.Appointment.NotMatchedToCustomer, StaticOperationStatus.StatusCode.BadRequest);

            if (!IsScheduleReschedulable(appointmentFromDb.TherapistSchedules))
                return ErrorResponse.Build(StaticResponseMessage.Appointment.NotCancellable, StaticOperationStatus.StatusCode.BadRequest);

            // 1. Ensure the appointment is not within the grace period
            if (IsWithinGracePeriod(appointmentFromDb))
                return ErrorResponse.Build(StaticResponseMessage.Appointment.CancelWithinGracePeriod, StaticOperationStatus.StatusCode.BadRequest);

            await using var transaction = await _unitOfWork.BeginTransactionAsync();
            try
            {
                // 2. Mark all related past schedules as cancelled
                var activeSchedules = appointmentFromDb.TherapistSchedules
                    .Where(ts => ts.ScheduleStatus != ScheduleStatus.Rescheduled &&
                                 ts.ScheduleStatus != ScheduleStatus.Cancelled)
                    .ToList();

                foreach (var schedule in activeSchedules)
                {
                    schedule.ScheduleStatus = ScheduleStatus.Cancelled;
                    schedule.Reason = "Cancelled by customer.";
                    schedule.UpdatedBy = User.Identity?.Name;
                    schedule.UpdatedTime = StaticOperationStatus.Timezone.Vietnam;

                    _unitOfWork.TherapistSchedule.Update(schedule, schedule);
                }

                await _unitOfWork.SaveAsync();
                await transaction.CommitAsync();

                return SuccessResponse.Build(
                    message: StaticResponseMessage.Appointment.Cancelled,
                    statusCode: StaticOperationStatus.StatusCode.Ok,
                    result: new
                    {
                        AppointmentId = appointmentFromDb.AppointmentId,
                        Status = ScheduleStatus.Cancelled
                    });
            }
            catch
            {
                await transaction.RollbackAsync();
                return ErrorResponse.Build(
                    message: StaticResponseMessage.Appointment.NotCancelled,
                    statusCode: StaticOperationStatus.StatusCode.InternalServerError);
            }

        }

        private static Int32 GetTotalPrice(IEnumerable<OrderDetail> orderDetails)
        {
            return Convert.ToInt32(orderDetails.Sum(detail => detail.Price));
        }

        }
}
