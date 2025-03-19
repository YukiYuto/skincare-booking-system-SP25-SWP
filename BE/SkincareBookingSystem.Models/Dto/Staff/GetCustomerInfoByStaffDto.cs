namespace SkincareBookingSystem.Models.Dto.Staff;

public class GetCustomerInfoByStaffDto
{
    public Guid CustomerId { get; set; }
    public string FullName { get; set; } = null!;
    public string Email { get; set; } = null!;
    public string PhoneNumber { get; set; } = null!;
}