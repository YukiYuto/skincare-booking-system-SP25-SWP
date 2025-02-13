namespace SkincareBookingSystem.Models.Dto.Email;

public class VerifyEmailDto
{
    public string UserId { get; set; } = null!;
    public string Token { get; set; } = null!;
    
}