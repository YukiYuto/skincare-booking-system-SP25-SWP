using SkincareBookingSystem.Services.IServices;
namespace SkincareBookingSystem.Services.Services.CloudinaryModule.Commands
{
    public class UploadVideoCommand : ICommand
    {
        private ICloudinaryService _cloudinaryService;
        public UploadVideoCommand(ICloudinaryService cloudinaryService)
        {
            _cloudinaryService = cloudinaryService;
        }
        public void Execute()
        {
            _cloudinaryService.UploadVideo();
        }
    }
}
