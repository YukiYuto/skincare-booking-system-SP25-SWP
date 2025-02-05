using Microsoft.AspNetCore.Http;
using SkincareBookingSystem.Services.IServices;
using SkincareBookingSystem.Services.Services.CloudinaryModule.Commands;

namespace SkincareBookingSystem.Services.Services.CloudinaryModule.Invoker
{
    public class CloudinaryServiceControl
    {
        private ICommand _command;
        private ICloudinaryService _cloudinaryService;

        public CloudinaryServiceControl(ICloudinaryService cloudinaryService)
        {
            _cloudinaryService = cloudinaryService;
        }

        public void SetCommand(ICommand command)
        {
            _command = command;
        }
        public void Run(IFormFile inputFile, string fileType)
        {
            _cloudinaryService.SetFileData(inputFile, fileType);
            _command.Execute();
        }
    }
}
