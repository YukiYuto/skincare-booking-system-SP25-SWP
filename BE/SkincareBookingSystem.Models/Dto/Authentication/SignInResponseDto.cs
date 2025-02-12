namespace SkincareBookingSystem.Models.Dto.Authentication;

public class SignInResponseDto
{
    public string AccessToken { get; set; } = null!;
    public string RefreshToken { get; set; } = null!;
}