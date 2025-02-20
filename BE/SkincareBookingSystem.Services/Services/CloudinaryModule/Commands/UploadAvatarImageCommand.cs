using Microsoft.AspNetCore.Http;
using SkincareBookingSystem.Services.IServices;
using CloudinaryDotNet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SkincareBookingSystem.Utilities.Constants;

namespace SkincareBookingSystem.Services.Services.CloudinaryModule.Commands
{
    public class UploadAvatarImageCommand : ICommand
    {
        private ICloudinaryService _cloudinaryService;
        private readonly IFormFile _file;
        private readonly string _folderPath;
        public UploadAvatarImageCommand(ICloudinaryService cloudinaryService, IFormFile file, string folderPath)
        {
            _cloudinaryService = cloudinaryService;
            _file = file;
            _folderPath = folderPath;
        }
        public async Task ExecuteAsync()
        {
            await _cloudinaryService.UploadImageAsync(
                _file,
                _folderPath,
                new Transformation().Named(
                    StaticCloudinarySettings.AvatarTransformation));
        }

    }
}
