using Microsoft.AspNetCore.Http;
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
        private readonly IFormFile _file;
        private readonly string _folderPath;
        public UploadImageCommand(ICloudinaryService cloudinaryService, IFormFile file, string folderPath)
        {
            _cloudinaryService = cloudinaryService;
            _file = file;
            _folderPath = folderPath;
        }
        public async Task ExecuteAsync()
        {
            await _cloudinaryService.UploadImageAsync(_file, _folderPath);
        }
    }
}
