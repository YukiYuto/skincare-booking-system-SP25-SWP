using SkincareBookingSystem.Services.IServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkincareBookingSystem.Services.Services.CloudinaryModule.Commands
{
    public class UploadImageCommand : ICommand
    {
        private ICloudinaryService _cloudinaryService;
        public UploadImageCommand(ICloudinaryService cloudinaryService)
        {
            _cloudinaryService = cloudinaryService;
        }
        public void Execute()
        {
            _cloudinaryService.UploadImage();
        }
    }
}
