using SkincareBookingSystem.DataAccess.IRepositories;
using SkincareBookingSystem.Models.Domain;
using SkincareBookingSystem.Models.Dto.Booking.Order;
using SkincareBookingSystem.Models.Dto.Booking.SkinTherapist;
using SkincareBookingSystem.Models.Dto.OrderDetails;
using SkincareBookingSystem.Models.Dto.Response;
using SkincareBookingSystem.Services.Helpers.Responses;
using SkincareBookingSystem.Services.Helpers.Users;
using SkincareBookingSystem.Services.IServices;
using SkincareBookingSystem.Utilities.Constants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace SkincareBookingSystem.Services.Services
{
    public class BookingService : IBookingService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IAutoMapperService _autoMapperService;

        public BookingService
        (
            IUnitOfWork unitOfWork,
            IAutoMapperService autoMapperService
        )
        {
            _unitOfWork = unitOfWork;
            _autoMapperService = autoMapperService;
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

        public async Task<ResponseDto> GetOccupiedSlotsFromTherapist(Guid therapistId, DateTime dateToSearch)
        {
            IEnumerable<Slot> occupiedSlots = await _unitOfWork.Slot.GetAllAsync(
                filter: s => s.TherapistSchedules.Any(
                    ts => ts.TherapistId == therapistId && ts.Appointment.AppointmentDate == dateToSearch.Date),
                includeProperties: $"{nameof(Slot.TherapistSchedules)},{nameof(Slot.TherapistSchedules)}.{nameof(TherapistSchedule.Appointment)}"
                );

            if (occupiedSlots is null || !occupiedSlots.Any())
                return ErrorResponse.Build(StaticOperationStatus.Slot.NotFound, StaticOperationStatus.StatusCode.NotFound);

            var occupiedSlotIds = occupiedSlots.Select(s => s.SlotId).ToList();

            return SuccessResponse.Build(
                message: StaticOperationStatus.Slot.Found,
                statusCode: StaticOperationStatus.StatusCode.Ok,
                result: occupiedSlotIds);
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
                order.OrderId = Guid.NewGuid();
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
                await _unitOfWork.OrderDetail.AddRangeAsync(orderDetails);
                await _unitOfWork.SaveAsync();

                await transaction.CommitAsync();

                return SuccessResponse.Build(
                    message: StaticOperationStatus.Order.Created,
                    statusCode: StaticOperationStatus.StatusCode.Created,
                    result: new { order, orderDetails });
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                return ErrorResponse.Build(
                    message: $"{StaticOperationStatus.Order.Message.NotCreated}: {ex.Message}",
                    statusCode: StaticOperationStatus.StatusCode.InternalServerError);
            }
        }

        private static Int32 GetTotalPrice(IEnumerable<OrderDetail> orderDetails)
        {
            return Convert.ToInt32(orderDetails.Sum(detail => detail.Price));
        }
    }
}
