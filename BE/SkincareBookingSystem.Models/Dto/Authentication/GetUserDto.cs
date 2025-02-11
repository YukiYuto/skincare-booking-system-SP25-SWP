namespace SkincareBookingSystem.Models.Dto.Authentication;

public class GetUserDto
{
    public string Id { get; set; } = null!;
    public string FullName { get; set; } = null!;
    public string Email { get; set; } = null!;
    public string PhoneNumber { get; set; } = null!;
    public string? Address { get; set; } = null!;
    public int Age { get; set; }
    public string? ImageUrl { get; set; } = null!;
    public string UserName { get; set; } = null!;
    public List<string> Roles { get; set; } = new List<string>();
}