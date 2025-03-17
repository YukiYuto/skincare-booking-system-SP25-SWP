using System.Security.Claims;
using SkincareBookingSystem.DataAccess.IRepositories;
using SkincareBookingSystem.Models.Domain;
using SkincareBookingSystem.Models.Dto.Booking.Appointment;
using SkincareBookingSystem.Models.Dto.Booking.Appointment.FinalizeAppointment;
using SkincareBookingSystem.Models.Dto.Booking.Appointment.RescheduleAppointment;
using SkincareBookingSystem.Models.Dto.Booking.Order;
using SkincareBookingSystem.Models.Dto.Booking.SkinTherapist;
using SkincareBookingSystem.Models.Dto.BookingSchedule;
using SkincareBookingSystem.Models.Dto.Response;
using SkincareBookingSystem.Services.Helpers.Responses;
using SkincareBookingSystem.Services.Helpers.Users;
using SkincareBookingSystem.Services.IServices;
using SkincareBookingSystem.Utilities.Constants;
using static SkincareBookingSystem.Services.Helpers.Schedules.DateValidator;
using static SkincareBookingSystem.Services.Helpers.Schedules.ScheduleValidator;

namespace SkincareBookingSystem.Services.Services;

public class BookingService : IBookingService
{
    private readonly IAutoMapperService _autoMapperService;
    private readonly ITherapistScheduleService _therapistScheduleService;
    private readonly IUnitOfWork _unitOfWork;

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
            st => st.TherapistServiceTypes.Any(tst => tst.ServiceTypeId == serviceTypeId),
            $"{nameof(SkinTherapist.ApplicationUser)},{nameof(SkinTherapist.TherapistServiceTypes)}");

        if (therapistsFromDb is null || !therapistsFromDb.Any())
            return ErrorResponse.Build(StaticOperationStatus.SkinTherapist.NotFound,
                StaticOperationStatus.StatusCode.NotFound);

        var therapistDtos = _autoMapperService.MapCollection<SkinTherapist, PreviewTherapistDto>(therapistsFromDb)
            .ToList();

        return SuccessResponse.Build(
            StaticOperationStatus.SkinTherapist.Found,
            StaticOperationStatus.StatusCode.Ok,
            therapistDtos);
    }

    public async Task<ResponseDto> GetOccupiedSlotsFromTherapist(Guid therapistId, DateOnly dateToSearch)
    {
        var occupiedSlotsFromDb = await _unitOfWork.Slot.GetAllAsync(
            s => s.TherapistSchedules.Any(
                ts => ts.TherapistId == therapistId && ts.Appointment.AppointmentDate == dateToSearch),
            $"{nameof(Slot.TherapistSchedules)},{nameof(Slot.TherapistSchedules)}.{nameof(TherapistSchedule.Appointment)}"
        );

        if (occupiedSlotsFromDb is null || !occupiedSlotsFromDb.Any())
            return SuccessResponse.Build(
                StaticOperationStatus.Slot.NotFound,
                StaticOperationStatus.StatusCode.Ok,
                new List<Slot>());

        var occupiedSlotsDto = occupiedSlotsFromDb.Select(s => new
        {
            s.SlotId,
            s.StartTime,
            s.EndTime
        });

        return SuccessResponse.Build(
            StaticOperationStatus.Slot.Found,
            StaticOperationStatus.StatusCode.Ok,
            occupiedSlotsDto);
    }

    public async Task<ResponseDto> BundleOrder(BundleOrderDto bundleOrderDto, ClaimsPrincipal User)
    {
        if (UserError.NotExists(User))
            return ErrorResponse.Build(StaticOperationStatus.User.UserNotFound,
                StaticOperationStatus.StatusCode.NotFound);

        if (!bundleOrderDto.OrderDetails.Any())
            return ErrorResponse.Build(StaticOperationStatus.OrderDetail.EmptyList,
                StaticOperationStatus.StatusCode.BadRequest);

        await using var transaction = await _unitOfWork.BeginTransactionAsync();
        try
        {
            var order = _autoMapperService.Map<BundleOrderDto, Order>(bundleOrderDto);
            order.OrderNumber = await _unitOfWork.Order.GenerateUniqueNumberAsync();
            order.CreatedBy = User.Identity?.Name;

            var orderDetails = new List<OrderDetail>();

            var comboIds = bundleOrderDto.OrderDetails
                .Where(od => od.ServiceComboId.HasValue)
                .Select(od => od.ServiceComboId!.Value)
                .Distinct()
                .ToList();

            var serviceIds = bundleOrderDto.OrderDetails
                .Where(od => !od.ServiceComboId.HasValue)
                .Select(od => od.ServiceId)
                .Distinct()
                .ToList();

            var comboItems = await _unitOfWork.ComboItem
                .GetListAsync(ci => comboIds.Contains(ci.ServiceComboId),
                    nameof(ComboItem.ServiceCombo));

            var comboItemDict = comboItems
                .GroupBy(ci => ci.ServiceComboId)
                .ToDictionary(g => g.Key, g => g.OrderBy(ci => ci.Priority).ToList());

            foreach (var detail in bundleOrderDto.OrderDetails)
            {
                if (detail.ServiceComboId.HasValue)
                {
                    var comboServices = comboItemDict.ContainsKey(detail.ServiceComboId.Value)
                        ? comboItemDict[detail.ServiceComboId.Value]
                        : new List<ComboItem>();

                    foreach (var item in comboServices)
                        orderDetails.Add(new OrderDetail
                        {
                            OrderId = order.OrderId,
                            ServiceId = item.ServiceId,
                            ServiceComboId = detail.ServiceComboId,
                            Price = detail.Price,
                            Description = item.ServiceCombo.ComboName,
                            ComboItem = item,
                            ServiceTracking = new OrderServiceTracking
                            {
                                Status = item.Priority == 1
                                    ? StaticOperationStatus.OrderServiceTracking.Pending
                                    : StaticOperationStatus.OrderServiceTracking.Locked,
                                CreatedTime = DateTime.UtcNow.AddHours(7),
                                UpdatedTime = null
                            }
                        });
                }

                if (!detail.ServiceComboId.HasValue)
                    orderDetails.Add(new OrderDetail
                    {
                        OrderId = order.OrderId,
                        ServiceId = detail.ServiceId,
                        Price = detail.Price,
                        Description = detail.Description,
                        ServiceTracking = new OrderServiceTracking
                        {
                            Status = StaticOperationStatus.OrderServiceTracking.Pending,
                            CreatedTime = DateTime.UtcNow.AddHours(7),
                            UpdatedTime = null
                        }
                    });
            }

            order.TotalPrice = GetTotalPrice(orderDetails);

            await _unitOfWork.Order.AddAsync(order);
            await _unitOfWork.SaveAsync();
            await transaction.CommitAsync();

            var orderResponseDto = _autoMapperService.Map<Order, OrderDto>(order);
            return SuccessResponse.Build(
                StaticOperationStatus.Order.Created,
                StaticOperationStatus.StatusCode.Created,
                orderResponseDto);
        }
        catch (Exception ex)
        {
            await transaction.RollbackAsync();
            return ErrorResponse.Build(
                $"{StaticOperationStatus.Order.Message.NotCreated}: {ex.Message}",
                StaticOperationStatus.StatusCode.InternalServerError);
        }
    }

    public async Task<ResponseDto> FinalizeAppointment(BookAppointmentDto bookingRequest, ClaimsPrincipal User)
    {
        try
        {
            if (UserError.NotExists(User))
                return ErrorResponse.Build(StaticResponseMessage.User.NotFound,
                    StaticOperationStatus.StatusCode.NotFound);

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var customerFromDb = await _unitOfWork.Customer.GetAsync(c => c.UserId == userId).ConfigureAwait(false);

            if (customerFromDb is null)
                return ErrorResponse.Build(StaticResponseMessage.Customer.NotFound,
                    StaticOperationStatus.StatusCode.NotFound);

            // 1. Check payment status by retrieving linked payment with the order number
            var paymentFromDb = await _unitOfWork.Payment
                .GetAsync(
                    p => p.OrderNumber == bookingRequest.OrderNumber,
                    nameof(Payment.Orders))
                .ConfigureAwait(false);

            if (paymentFromDb is null || paymentFromDb.Status != PaymentStatus.Paid)
                return ErrorResponse.Build(StaticResponseMessage.Payment.NotCompleted,
                    StaticOperationStatus.StatusCode.BadRequest);
            // 1.1. Check if the order in the payment is linked to the customer
            if (paymentFromDb.Orders?.CustomerId != customerFromDb.CustomerId)
                return ErrorResponse.Build(StaticResponseMessage.Customer.Invalid,
                    StaticOperationStatus.StatusCode.BadRequest);

            // 2. Check slot availability
            var slotFromDb = await _unitOfWork.Slot
                .GetAsync(s => s.SlotId == bookingRequest.SlotId)
                .ConfigureAwait(false);
            if (slotFromDb is null)
                return ErrorResponse.Build(StaticResponseMessage.Slot.InvalidSelected,
                    StaticOperationStatus.StatusCode.BadRequest);

            // 3. Check if the therapist is already scheduled for the selected slot
            var therapistScheduleFromDb = await _unitOfWork.TherapistSchedule
                .GetAllAsync(
                    filter: ts => ts.SlotId == bookingRequest.SlotId,
                    includeProperties: nameof(TherapistSchedule.Appointment))
                .ConfigureAwait(false);

            var existingTherapistSchedule = therapistScheduleFromDb
                .FirstOrDefault(ts => IsScheduleExisting(ts, bookingRequest));

            if (existingTherapistSchedule is not null)
                return ErrorResponse.Build(StaticResponseMessage.TherapistSchedule.AlreadyScheduled,
                    StaticOperationStatus.StatusCode.BadRequest);

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

                var scheduleResponse =
                    await _therapistScheduleService.CreateTherapistSchedule(User, therapistScheduleToCreate);
                if (!scheduleResponse.IsSuccess)
                    throw new Exception("Failed to create therapist schedule. " + scheduleResponse.Message);

                await transaction.CommitAsync();

                // 6. Return the customized response to avoid circular reference in the response JSON
                var appointmentResponseDto = _autoMapperService.Map<Appointments, AppointmentDto>(appointmentToCreate);
                var scheduleResponseDto = _autoMapperService.Map<TherapistSchedule, ScheduleDto>(
                    (TherapistSchedule)scheduleResponse.Result!);
                return SuccessResponse.Build(
                    StaticResponseMessage.Appointment.Created,
                    StaticOperationStatus.StatusCode.Created,
                    new
                    {
                        appointmentResponseDto,
                        scheduleResponseDto
                    });
            }
            catch
            {
                await transaction.RollbackAsync();
                return ErrorResponse.Build(
                    StaticResponseMessage.Appointment.NotCreated,
                    StaticOperationStatus.StatusCode.InternalServerError);
            }
        }
        catch (Exception e)
        {
            return new ResponseDto
            {
                IsSuccess = false,
                StatusCode = StaticOperationStatus.StatusCode.InternalServerError,
                Message = e.Message
            };
        }
    }

    public async Task<ResponseDto> HandleTherapistAutoAssignment(AutoAssignmentDto autoAssignmentDto)
    {
        var today = DateOnly.FromDateTime(DateTime.Today);
        var firstDayOfCurrentMonth = new DateOnly(today.Year, today.Month, 1);

        var availableTherapists = await _unitOfWork.SkinTherapist
            .GetAllAsync(
                st =>
                    st.TherapistServiceTypes.Any(tst => tst.ServiceTypeId == autoAssignmentDto.ServiceTypeId) &&
                    !st.TherapistSchedules.Any(ts =>
                        ts.Appointment.AppointmentDate == autoAssignmentDto.AppointmentDate &&
                        ts.SlotId == autoAssignmentDto.SlotId),
                $"{nameof(SkinTherapist.TherapistServiceTypes)}," +
                $"{nameof(SkinTherapist.TherapistSchedules)}," +
                $"{nameof(SkinTherapist.TherapistSchedules)}.{nameof(TherapistSchedule.Appointment)},")
            .ConfigureAwait(false);

        if (availableTherapists is null || !availableTherapists.Any())
            return ErrorResponse.Build(
                StaticResponseMessage.TherapistSchedule.NoTherapistForSlot,
                StaticOperationStatus.StatusCode.NotFound);

        // Retrieve the therapist with the least number of bookings (excluding deleted ones)
        var leastBookedTherapist = availableTherapists
            .OrderBy(t => t.TherapistSchedules?.Count(
                ts => ts.Status == StaticOperationStatus.BookingSchedule.Completed && IsDateWithinRange(
                    ts.Appointment.AppointmentDate,
                    firstDayOfCurrentMonth,
                    today)) ?? 0)
            .FirstOrDefault();

        if (leastBookedTherapist is null)
            return ErrorResponse.Build(
                StaticResponseMessage.TherapistSchedule.NoTherapistForSlot,
                StaticOperationStatus.StatusCode.NotFound);

        return SuccessResponse.Build(
            StaticResponseMessage.TherapistSchedule.AutoAssignmentHandled,
            StaticOperationStatus.StatusCode.Ok,
            leastBookedTherapist.SkinTherapistId);
    }

    public async Task<ResponseDto> RescheduleAppointment(RescheduleAppointmentDto rescheduleRequest,
        ClaimsPrincipal User)
    {
        if (UserError.NotExists(User))
            return ErrorResponse.Build(StaticResponseMessage.User.NotFound,
                StaticOperationStatus.StatusCode.NotFound);
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        // 1. Appointment validation
        // 1.1. Retrieve the appointment to reschedule
        var appointmentFromDb = await _unitOfWork.Appointments
            .GetAsync(a => a.AppointmentId == rescheduleRequest.AppointmentId,
                $"{nameof(Appointments.TherapistSchedules)}.{nameof(TherapistSchedule.Slot)},{nameof(Appointments.Customer)}")
            .ConfigureAwait(false);

        if (appointmentFromDb is null)
            return ErrorResponse.Build(StaticResponseMessage.Appointment.NotFound,
                StaticOperationStatus.StatusCode.NotFound);

        // 1.2. Check if the appointment is linked to the customer
        if (appointmentFromDb.Customer.UserId != userId)
            return ErrorResponse.Build(StaticResponseMessage.Appointment.NotMatchedToCustomer,
                StaticOperationStatus.StatusCode.BadRequest);

        // 1.3. Ensure the appointment is not already completed / cancelled 
        if (!IsScheduleReschedulable(appointmentFromDb.TherapistSchedules))
            return ErrorResponse.Build(StaticResponseMessage.Appointment.NotReschedulable,
                StaticOperationStatus.StatusCode.BadRequest);

        // 1.4. Ensure the appointment is not within the grace period
        if (IsWithinGracePeriod(appointmentFromDb))
            return ErrorResponse.Build(StaticResponseMessage.Appointment.RescheduleWithinGracePeriod,
                StaticOperationStatus.StatusCode.BadRequest);

        // 2. Slot validation
        var slotToReschedule = await _unitOfWork.Slot
            .GetAsync(s => s.SlotId == rescheduleRequest.NewSlotId)
            .ConfigureAwait(false);

        if (slotToReschedule is null)
            return ErrorResponse.Build(StaticResponseMessage.Slot.InvalidSelected,
                StaticOperationStatus.StatusCode.BadRequest);

        // 3. Therapist schedule validation
        var therapistId = appointmentFromDb.TherapistSchedules.Last().TherapistId;
        var existingSchedule = await _unitOfWork.TherapistSchedule
            .GetAsync(ts => ts.TherapistId == therapistId &&
                            ts.SlotId == rescheduleRequest.NewSlotId &&
                            ts.ScheduleStatus != ScheduleStatus.Rescheduled)
            .ConfigureAwait(false);

        if (existingSchedule is not null)
            return ErrorResponse.Build(StaticResponseMessage.TherapistSchedule.AlreadyScheduled,
                StaticOperationStatus.StatusCode.BadRequest);

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
                StaticResponseMessage.Appointment.Rescheduled,
                StaticOperationStatus.StatusCode.Ok,
                new
                {
                    OldScheduleStatus = ScheduleStatus.Rescheduled,
                    NewSchedule = newSchedule
                });
        }
        catch
        {
            await transaction.RollbackAsync();
            return ErrorResponse.Build(
                StaticResponseMessage.Appointment.NotRescheduled,
                StaticOperationStatus.StatusCode.InternalServerError);
        }
    }

    public async Task<ResponseDto> CancelAppointment(Guid appointmentId, ClaimsPrincipal User)
    {
        if (UserError.NotExists(User))
            return ErrorResponse.Build(StaticResponseMessage.User.NotFound,
                StaticOperationStatus.StatusCode.NotFound);

        var appointmentFromDb = await _unitOfWork.Appointments
            .GetAsync(a => a.AppointmentId == appointmentId,
                $"{nameof(Appointments.TherapistSchedules)},{nameof(Appointments.Customer)}")
            .ConfigureAwait(false);

        if (appointmentFromDb is null)
            return ErrorResponse.Build(StaticResponseMessage.Appointment.NotFound,
                StaticOperationStatus.StatusCode.NotFound);

        if (appointmentFromDb.Customer.UserId != User.FindFirstValue(ClaimTypes.NameIdentifier))
            return ErrorResponse.Build(StaticResponseMessage.Appointment.NotMatchedToCustomer,
                StaticOperationStatus.StatusCode.BadRequest);

        if (!IsScheduleReschedulable(appointmentFromDb.TherapistSchedules))
            return ErrorResponse.Build(StaticResponseMessage.Appointment.NotCancellable,
                StaticOperationStatus.StatusCode.BadRequest);

        // 1. Ensure the appointment is not within the grace period
        if (IsWithinGracePeriod(appointmentFromDb))
            return ErrorResponse.Build(StaticResponseMessage.Appointment.CancelWithinGracePeriod,
                StaticOperationStatus.StatusCode.BadRequest);

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
                StaticResponseMessage.Appointment.Cancelled,
                StaticOperationStatus.StatusCode.Ok,
                new
                {
                    appointmentFromDb.AppointmentId,
                    Status = ScheduleStatus.Cancelled
                });
        }
        catch
        {
            await transaction.RollbackAsync();
            return ErrorResponse.Build(
                StaticResponseMessage.Appointment.NotCancelled,
                StaticOperationStatus.StatusCode.InternalServerError);
        }
    }

    public Task<ResponseDto> CompletedService(ClaimsPrincipal User, CompletedServiceDto completedServiceDto)
    {
        //user
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (userId is null)
            return Task.FromResult(ErrorResponse.Build(StaticOperationStatus.User.UserNotFound,
                StaticOperationStatus.StatusCode.NotFound));
        //Therapist
        var therapist = _unitOfWork.SkinTherapist.GetAsync(t => t.UserId == userId).Result;
        if (therapist is null)
            return Task.FromResult(ErrorResponse.Build(StaticOperationStatus.SkinTherapist.NotFound,
                StaticOperationStatus.StatusCode.NotFound));

        //Appointment
        /*var appointment = _unitOfWork.Appointments.GetAsync(a => a.AppointmentId == completedServiceDto.AppointmentId);
        if (appointment is null)
            return Task.FromResult(ErrorResponse.Build(StaticOperationStatus.Appointment.NotFound,
                StaticOperationStatus.StatusCode.NotFound));*/

        return null;
    }

    private static int GetTotalPrice(IEnumerable<OrderDetail> orderDetails)
    {
        return Convert.ToInt32(orderDetails.Sum(detail => detail.Price));
    }
}

