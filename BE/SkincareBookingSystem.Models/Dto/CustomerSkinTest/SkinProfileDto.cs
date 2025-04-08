namespace SkincareBookingSystem.Models.Dto.CustomerSkinTest;

public class SkinProfileDto
{
    public Guid SkinProfileId { get; set; }
    public string SkinName { get; set; }
    public string Description { get; set; }
    public int ScoreMin { get; set; }
    public int ScoreMax { get; set; }
}