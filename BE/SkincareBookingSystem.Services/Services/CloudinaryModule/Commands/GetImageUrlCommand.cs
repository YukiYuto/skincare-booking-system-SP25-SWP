using SkincareBookingSystem.Services.IServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkincareBookingSystem.Services.Services.CloudinaryModule.Commands
{
    public class GetImageUrlCommand : ICommand
    {
        private readonly ICloudinaryService _cloudinaryService;
        private readonly string _publicId;

        public GetImageUrlCommand(ICloudinaryService cloudinaryService, string publicId)
        {
            _cloudinaryService = cloudinaryService;
            _publicId = publicId;
        }

        public async Task<string> ExecuteAsyncWithValue()
        {
            return await _cloudinaryService.GetImageUrlAsync(_publicId);
        }

        public async Task ExecuteAsync()
        {
            await _cloudinaryService.GetImageUrlAsync(_publicId);
        }
    }
}
