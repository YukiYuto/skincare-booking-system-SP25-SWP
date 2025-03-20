using System.Security.Claims;
using Microsoft.AspNetCore.Mvc.Filters;
using SkincareBookingSystem.DataAccess.IRepositories;
using SkincareBookingSystem.Models.Domain;
using SkincareBookingSystem.Models.Dto.Response;
using SkincareBookingSystem.Models.Dto.Staff;
using SkincareBookingSystem.Services.IServices;
using SkincareBookingSystem.Utilities.Constants;

namespace SkincareBookingSystem.Services.Services;

public class StaffService : IStaffService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IAutoMapperService _autoMapperService;

    public StaffService(IUnitOfWork unitOfWork, IAutoMapperService autoMapperService)
    {
        _unitOfWork = unitOfWork;
        _autoMapperService = autoMapperService;
    }


    public async Task<ResponseDto> GetCustomerByInfor(
        ClaimsPrincipal User,
        int pageNumber = 1,
        int pageSize = 10,
        string? filterQuery = null
    )
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (userId == null)
        {
            return new ResponseDto()
            {
                Message = "User not found",
                IsSuccess = false,
                StatusCode = 404
            };
        }

        var staff = await _unitOfWork.Staff.GetAsync(s => s.UserId == userId);
        if (staff == null)
        {
            return new ResponseDto()
            {
                Message = "You are not a staff",
                IsSuccess = false,
                StatusCode = 403
            };
        }

        var customer = await _unitOfWork.Customer.GetAllAsync(
            includeProperties: nameof(Customer.ApplicationUser)
        );

        if (!string.IsNullOrEmpty(filterQuery))
        {
            filterQuery = filterQuery.ToLower().Trim();

            customer = customer.Where(c =>
                c.ApplicationUser.FullName.ToLower().Contains(filterQuery) ||
                c.ApplicationUser.Email!.ToLower().Contains(filterQuery) ||
                c.ApplicationUser.PhoneNumber!.ToLower().Contains(filterQuery)
            ).ToList();
        }

        int totalCustomers = customer.Count();

        var paginatedCustomers = customer
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToList();

        if (customer == null)
        {
            return new ResponseDto()
            {
                Message = "No customers found",
                IsSuccess = false,
                StatusCode = 200
            };
        }

        var customerDto = _autoMapperService.MapCollection<Customer, GetCustomerInfoByStaffDto>(customer);

        return new ResponseDto()
        {
            Result = new
            {
                TotalCustomers = totalCustomers,
                TotalPages = (int)Math.Ceiling((double)totalCustomers / pageSize),
                PageSize = pageSize,
                CurrentPage = pageNumber,
                Customers = customerDto
            },
            Message = "Customers retrieved successfully",
            IsSuccess = true,
            StatusCode = 200
        };
    }

    public async Task<ResponseDto> CheckInCustomer(ClaimsPrincipal User, CheckInDto checkInDto)
    {
        var customer = await _unitOfWork.Customer.GetAsync
            (c => c.CustomerId == checkInDto.CustomerId);
        if (customer == null)
        {
            return new ResponseDto()
            {
                Message = "Customer not found",
                IsSuccess = false,
                StatusCode = 404
            };
        }

        var appointment = await _unitOfWork.Appointments.GetAsync
            (a => a.AppointmentId == checkInDto.AppointmentId && a.CustomerId == checkInDto.CustomerId);
        if (appointment == null)
        {
            return new ResponseDto()
            {
                Message = "Customer has no appointment",
                IsSuccess = false,
                StatusCode = 200
            };
        }

        if (appointment.CheckInTime != null)
        {
            return new ResponseDto()
            {
                Message = "Customer already checked in",
                IsSuccess = true,
                StatusCode = 200
            };
        }

        appointment.UpdatedBy = User.FindFirstValue(ClaimTypes.NameIdentifier);
        appointment.CheckInTime = DateTime.UtcNow.AddHours(7);
        appointment.Status = StaticOperationStatus.Appointment.CheckedIn;

        _unitOfWork.Appointments.UpdateStatus(appointment);
        await _unitOfWork.SaveAsync();

        var checkIn =
            _autoMapperService.Map<Appointments, CheckInSuccessfulDto>(appointment);

        return new ResponseDto()
        {
            Message = "Customer checked in successfully",
            IsSuccess = true,
            StatusCode = 200,
            Result = checkIn
        };
    }

    public async Task<ResponseDto> CheckOutCustomer(ClaimsPrincipal User, CheckInDto checkOutDto)
    {
        var customer = await _unitOfWork.Customer.GetAsync
            (c => c.CustomerId == checkOutDto.CustomerId);
        if (customer == null)
        {
            return new ResponseDto()
            {
                Message = "Customer not found",
                IsSuccess = false,
                StatusCode = 404
            };
        }

        var appointment = await _unitOfWork.Appointments.GetAsync(a => 
            a.AppointmentId == checkOutDto.AppointmentId &&
            a.CustomerId == checkOutDto.CustomerId &&
            a.Status == StaticOperationStatus.Appointment.CheckedIn);
        if (appointment == null)
        {
            return new ResponseDto()
            {
                Message = "Customer has no appointment",
                IsSuccess = false,
                StatusCode = 200
            };
        }

        if (appointment.CheckInTime != null)
        {
            return new ResponseDto()
            {
                Message = "Customer already checked in",
                IsSuccess = true,
                StatusCode = 200
            };
        }
        
        var therapistCompleted = _unitOfWork.TherapistSchedule.GetAsync
            (tc => tc.AppointmentId == checkOutDto.AppointmentId && tc.ScheduleStatus == ScheduleStatus.Completed);

        return null;
    }
}