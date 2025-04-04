﻿using System.Security.Claims;
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
    private readonly IEmailService _emailService;

    public BookingService(
        IUnitOfWork unitOfWork,
        IAutoMapperService autoMapperService,
        ITherapistScheduleService therapistScheduleService,
        IEmailService emailService
    )
    {
        _unitOfWork = unitOfWork;
        _autoMapperService = autoMapperService;
        _therapistScheduleService = therapistScheduleService;
        _emailService = emailService;
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
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (userId is null)
            return ErrorResponse.Build(StaticOperationStatus.User.UserNotFound,
                StaticOperationStatus.StatusCode.NotFound);
        var customer = await _unitOfWork.Customer.GetAsync(c => c.UserId == userId);
        if (customer is null)
            return ErrorResponse.Build(StaticOperationStatus.Customer.NotFound,
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
            var orderServiceTrackings = new List<OrderServiceTracking>();

            // Collect comboIDs from order details
            var comboIds = bundleOrderDto.OrderDetails
                .Where(od => od.ServiceComboId.HasValue)
                .Select(od => od.ServiceComboId!.Value)
                .Distinct()
                .ToList();

            // Collect serviceIDs from order details that don't have combo IDs
            var serviceIds = bundleOrderDto.OrderDetails
                .Where(od => !od.ServiceComboId.HasValue && od.ServiceId.HasValue)
                .Select(od => od.ServiceId!.Value)
                .Distinct()
                .ToList();

            // Get combo items for the combo IDs
            var comboItems = await _unitOfWork.ComboItem
                .GetListAsync(ci => comboIds.Contains(ci.ServiceComboId),
                    nameof(ComboItem.ServiceCombo));

            // Extract serviceIDs from combo items
            var comboServiceIds = comboItems.Select(ci => ci.ServiceId).Distinct().ToList();

            // Combine with direct service IDs
            var allServiceIds = serviceIds.Concat(comboServiceIds).Distinct().ToList();

            // Get all needed services
            var services = await _unitOfWork.Services
                .GetListAsync(s => allServiceIds.Contains(s.ServiceId));

            // Create dictionary service prices
            var serviceDict = services.ToDictionary(s => s.ServiceId, s => s.Price);

            // Group combo items by comboID
            var comboItemDict = comboItems
                .GroupBy(ci => ci.ServiceComboId)
                .ToDictionary(g => g.Key, g => g.OrderBy(ci => ci.Priority).ToList());

            bool hasPendingServiceInCombo = false;

            // Process combo items first
            foreach (var detail in bundleOrderDto.OrderDetails)
            {
                if (detail.ServiceComboId.HasValue)
                {
                    var comboServices = comboItemDict.ContainsKey(detail.ServiceComboId.Value)
                        ? comboItemDict[detail.ServiceComboId.Value]
                        : new List<ComboItem>();

                    foreach (var item in comboServices)
                    {
                        var orderDetail = new OrderDetail
                        {
                            OrderDetailId = Guid.NewGuid(),
                            OrderId = order.OrderId,
                            ServiceId = item.ServiceId,
                            ServiceComboId = detail.ServiceComboId,
                            Price = serviceDict.TryGetValue(item.Services.ServiceId, out double price) ? price : 0,
                            Description = item.ServiceCombo?.ComboName ?? "Unknown Combo"
                        };

                        orderDetails.Add(orderDetail);

                        var serviceTracking = new OrderServiceTracking
                        {
                            TrackingId = Guid.NewGuid(),
                            OrderDetailId = orderDetail.OrderDetailId,
                            Status = item.Priority == 1
                                ? StaticOperationStatus.OrderServiceTracking.Pending
                                : StaticOperationStatus.OrderServiceTracking.Locked,
                            CreatedTime = DateTime.UtcNow.AddHours(7),
                            CreatedBy = User.Identity?.Name,
                            UpdatedTime = null
                        };

                        orderServiceTrackings.Add(serviceTracking);
                        if (item.Priority == 1)
                            hasPendingServiceInCombo = true;
                    }
                }
            }

            // Process individual services
            foreach (var detail in bundleOrderDto.OrderDetails)
            {
                if (!detail.ServiceComboId.HasValue && detail.ServiceId.HasValue &&
                    serviceDict.TryGetValue(detail.ServiceId.Value, out var price))
                {
                    var orderDetail = new OrderDetail
                    {
                        OrderDetailId = Guid.NewGuid(),
                        OrderId = order.OrderId,
                        ServiceId = detail.ServiceId,
                        Price = price,
                        Description = detail.Description
                    };

                    orderDetails.Add(orderDetail);

                    var serviceTracking = new OrderServiceTracking
                    {
                        TrackingId = Guid.NewGuid(),
                        OrderDetailId = orderDetail.OrderDetailId,
                        Status = hasPendingServiceInCombo
                            ? StaticOperationStatus.OrderServiceTracking.Locked
                            : StaticOperationStatus.OrderServiceTracking.Pending,
                        CreatedTime = DateTime.UtcNow.AddHours(7),
                        CreatedBy = User.Identity?.Name,
                        UpdatedTime = null
                    };

                    orderServiceTrackings.Add(serviceTracking);
                }
            }

            order.TotalPrice = Convert.ToDouble(GetTotalPrice(orderDetails));
            order.OrderDetails = orderDetails;

            await _unitOfWork.Order.AddAsync(order);

            await _unitOfWork.OrderServiceTracking.AddRangeAsync(orderServiceTrackings);
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
            var customerFromDb = await _unitOfWork.Customer.GetAsync(
                c => c.UserId == userId,
                includeProperties: nameof(Customer.ApplicationUser)
            ).ConfigureAwait(false);

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

                // 6. Send booking success email to the customer
                var order = paymentFromDb.Orders;
                var userEmail = customerFromDb.ApplicationUser?.Email;
                var userName = customerFromDb.ApplicationUser?.FullName ?? "Customer";


                //handle appointment created but email not sent
                if (string.IsNullOrEmpty(userEmail))
                {
                    return SuccessResponse.Build(
                        StaticResponseMessage.Appointment.Created + " but email not sent due to missing email",
                        StaticOperationStatus.StatusCode.Created,
                        new
                        {
                            Appointment = _autoMapperService.Map<Appointments, AppointmentDto>(appointmentToCreate),
                            Schedule = _autoMapperService.Map<TherapistSchedule, ScheduleDto>(
                                (TherapistSchedule)scheduleResponse.Result!)
                        });
                }

                var orderDetails = await _unitOfWork.OrderDetail.GetAllAsync(od => od.OrderId == order.OrderId,
                    $"{nameof(OrderDetail.ServiceCombo)},{nameof(OrderDetail.Services)}");

                var bookingServices = orderDetails
                    .Where(od => od.Services != null)
                    .Select(od => od.Services!.ServiceName)
                    .ToList();

                bookingServices.AddRange(orderDetails
                    .Where(od => od.ServiceCombo != null)
                    .Select(od => od.ServiceCombo!.ComboName));

                var bookingDateTime = appointmentToCreate.AppointmentDate.ToString("yyyy-MM-dd");
                var viewOrderLink = $"https://lumiconnect-beauty.vercel.app/appointments";

                var emailSent = await _emailService.SendBookingSuccessEmailAsync(
                    toEmail: userEmail,
                    userName: userName,
                    bookingDateTime: bookingDateTime,
                    bookingServices: bookingServices,
                    viewOrderLink: viewOrderLink
                );

                // 7. Return the customized response to avoid circular reference in the response JSON
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

    public async Task<ResponseDto> CancelAppointment(CancelAppointmentDto cancelAppointmentDto, ClaimsPrincipal User)
    {
        if (UserError.NotExists(User))
            return ErrorResponse.Build(StaticResponseMessage.User.NotFound,
                StaticOperationStatus.StatusCode.NotFound);

        var appointmentFromDb = await _unitOfWork.Appointments
            .GetAsync(a => a.AppointmentId == cancelAppointmentDto.AppointmentId,
                $"{nameof(Appointments.TherapistSchedules)}.{nameof(TherapistSchedule.Slot)},{nameof(Appointments.Customer)}")
            .ConfigureAwait(false);

        if (appointmentFromDb is null)
            return ErrorResponse.Build(StaticResponseMessage.Appointment.NotFound,
                StaticOperationStatus.StatusCode.NotFound);

        if (await IsRequestFromCustomer(User) is true &&
            appointmentFromDb.Customer.UserId != User.FindFirstValue(ClaimTypes.NameIdentifier))
            return ErrorResponse.Build(StaticResponseMessage.Appointment.NotMatchedToCustomer,
                StaticOperationStatus.StatusCode.BadRequest);

        if (!IsScheduleReschedulable(appointmentFromDb.TherapistSchedules))
            return ErrorResponse.Build(StaticResponseMessage.Appointment.NotCancellable,
                StaticOperationStatus.StatusCode.BadRequest);

        // 1. Ensure the appointment is not within the grace period
        if (await IsRequestFromCustomer(User) is true && IsWithinGracePeriod(appointmentFromDb))
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
                schedule.Reason = cancelAppointmentDto.Reason;

                _unitOfWork.TherapistSchedule.Update(schedule, schedule);
            }

            // 3. Update the appointment status
            appointmentFromDb.Status = StaticOperationStatus.Appointment.Cancelled;

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

    public async Task<ResponseDto> CompletedService(ClaimsPrincipal User, CompletedServiceDto completedServiceDto)
    {
        //user
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (userId is null)
            return new ResponseDto()
            {
                IsSuccess = false,
                StatusCode = StaticOperationStatus.StatusCode.NotFound,
                Message = StaticOperationStatus.User.UserNotFound
            };

        //Therapist
        var therapist = await _unitOfWork.SkinTherapist.GetAsync(st => st.UserId == userId);
        if (therapist is null)
            return new ResponseDto()
            {
                IsSuccess = false,
                StatusCode = StaticOperationStatus.StatusCode.NotFound,
                Message = StaticOperationStatus.SkinTherapist.NotFound
            };

        //Appointment
        var appointment =
            await _unitOfWork.Appointments.GetAsync(a => a.AppointmentId == completedServiceDto.AppointmentId);
        if (appointment is null)
            return new ResponseDto()
            {
                IsSuccess = false,
                StatusCode = StaticOperationStatus.StatusCode.NotFound,
                Message = StaticOperationStatus.Appointment.NotFound
            };

        //TherapistSchedule
        var therapistAppoinment = await _unitOfWork.TherapistSchedule.GetAsync
            (ts => ts.AppointmentId == appointment.AppointmentId && ts.TherapistId == therapist.SkinTherapistId);
        if (therapistAppoinment is null)
            return new ResponseDto()
            {
                IsSuccess = false,
                StatusCode = StaticOperationStatus.StatusCode.NotFound,
                Message = StaticOperationStatus.SkinTherapist.Do_not
            };

        if (therapistAppoinment.ScheduleStatus == ScheduleStatus.Completed)
            return new ResponseDto()
            {
                IsSuccess = false,
                StatusCode = StaticOperationStatus.StatusCode.BadRequest,
                Message = "Service already completed"
            };

        //order
        var order = await _unitOfWork.Order.GetAsync(o => o.OrderId == appointment.OrderId);
        if (order is null)
            return new ResponseDto()
            {
                IsSuccess = false,
                StatusCode = StaticOperationStatus.StatusCode.NotFound,
                Message = StaticOperationStatus.Order.Message.NotFound
            };

        //orderDetail
        var orderDetail = await _unitOfWork.OrderDetail.GetAsync(od => od.OrderId == order.OrderId);
        if (orderDetail is null)
            return new ResponseDto()
            {
                IsSuccess = false,
                StatusCode = StaticOperationStatus.StatusCode.NotFound,
                Message = StaticOperationStatus.OrderDetail.NotFound
            };
        //OrderServiceTracking
        var orderServiceTracking =
            await _unitOfWork.OrderServiceTracking.GetAsync(ost => ost.OrderDetailId == orderDetail.OrderDetailId);
        if (orderServiceTracking is null)
            return new ResponseDto()
            {
                IsSuccess = false,
                StatusCode = StaticOperationStatus.StatusCode.NotFound,
                Message = StaticOperationStatus.OrderServiceTracking.NotFound
            };

        if (orderServiceTracking.Status != StaticOperationStatus.OrderServiceTracking.Pending)
            return new ResponseDto()
            {
                IsSuccess = false,
                StatusCode = StaticOperationStatus.StatusCode.BadRequest,
                Message = "Service is not available to complete"
            };

        await using var transaction = await _unitOfWork.BeginTransactionAsync();
        try
        {
            // Update OrderServiceTracking
            orderServiceTracking.Status = StaticOperationStatus.OrderServiceTracking.Completed;
            orderServiceTracking.UpdatedTime = DateTime.UtcNow.AddHours(7);
            orderServiceTracking.UpdatedBy = User.FindFirstValue("name");
            _unitOfWork.OrderServiceTracking.Update(orderServiceTracking);

            // Update next service if it's a combo
            if (orderDetail.ServiceComboId.HasValue)
            {
                var nextService = await _unitOfWork.OrderDetail.GetAsync(
                    od => od.ServiceComboId == orderDetail.ServiceComboId &&
                          od.ComboItem!.Priority == orderDetail.ComboItem!.Priority + 1,
                    includeProperties: "OrderServiceTracking");

                if (nextService != null)
                {
                    nextService.ServiceTracking!.Status = StaticOperationStatus.OrderServiceTracking.Pending;
                }
            }

            // Update TherapistSchedule
            therapistAppoinment.ScheduleStatus = ScheduleStatus.Completed;
            therapistAppoinment.UpdatedTime = DateTime.UtcNow.AddHours(7);
            therapistAppoinment.UpdatedBy = User.FindFirstValue("name");
            _unitOfWork.TherapistSchedule.UpdateStatus(therapistAppoinment);


            // Update Appointment
            appointment.Status = StaticOperationStatus.Appointment.Completed;
            await _unitOfWork.SaveAsync();
            await transaction.CommitAsync();

            return new ResponseDto()
            {
                IsSuccess = true,
                StatusCode = StaticOperationStatus.StatusCode.Ok,
                Message = "Service completed successfully"
            };
        }
        catch (Exception e)
        {
            await transaction.RollbackAsync();
            return new ResponseDto()
            {
                IsSuccess = false,
                StatusCode = StaticOperationStatus.StatusCode.InternalServerError,
                Message = e.Message
            };
        }
    }

    private async Task<bool> IsRequestFromCustomer(ClaimsPrincipal User)
    {
        return await _unitOfWork.Customer.GetAsync(c => c.UserId == User.FindFirstValue(ClaimTypes.NameIdentifier)) is
            not null;
    }


    private static int GetTotalPrice(IEnumerable<OrderDetail> orderDetails)
    {
        return Convert.ToInt32(orderDetails.Sum(detail => detail.Price));
    }
}