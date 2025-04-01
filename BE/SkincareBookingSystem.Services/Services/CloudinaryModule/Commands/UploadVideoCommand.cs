using Microsoft.AspNetCore.Http;
using SkincareBookingSystem.Services.IServices;
namespace SkincareBookingSystem.Services.Services.CloudinaryModule.Commands
{
    public class UploadVideoCommand : ICommand
    {
        private ICloudinaryService _cloudinaryService;
        private readonly IFormFile _file;
        private readonly string _folderPath;
        public UploadVideoCommand(ICloudinaryService cloudinaryService, IFormFile file, string folderPath)
        {
            _cloudinaryService = cloudinaryService;
            _file = file;
            _folderPath = folderPath;
        }
        public async Task ExecuteAsync()
        {
            await _cloudinaryService.UploadVideoAsync(_file, _folderPath);
        }
    }
}
