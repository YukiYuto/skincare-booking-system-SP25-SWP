using Microsoft.AspNetCore.Http;
using SkincareBookingSystem.DataAccess.IRepositories;
using SkincareBookingSystem.Models.Dto.FileStorage;
using SkincareBookingSystem.Models.Dto.Response;
using SkincareBookingSystem.Services.Helpers.Responses;
using SkincareBookingSystem.Services.IServices;
using SkincareBookingSystem.Services.Services.CloudinaryModule.Commands;
using SkincareBookingSystem.Services.Services.CloudinaryModule.Invoker;
using SkincareBookingSystem.Utilities.Constants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace SkincareBookingSystem.Services.Services
{
    public class FileStorageService : IFileStorageService
    {
        private readonly CloudinaryServiceControl _cloudinaryServiceControl;
        private readonly ICloudinaryService _cloudinaryService;

        public FileStorageService(CloudinaryServiceControl cloudinaryServiceControl, ICloudinaryService cloudinaryService)
        {
            _cloudinaryServiceControl = cloudinaryServiceControl;
            _cloudinaryService = cloudinaryService;
        }

        public Task<string> GetAvatarImageUrl(ClaimsPrincipal user)
        {
            throw new NotImplementedException();
        }

        public async Task<ResponseDto> UploadAvatarImage(UploadFileDto uploadFileDto, ClaimsPrincipal user)
        {
            if (uploadFileDto.File?.Length is 0)
            {
                return ErrorResponse.Build(
                    message: StaticOperationStatus.File.FileEmpty,
                    statusCode: StaticOperationStatus.StatusCode.BadRequest);
            }

            if (user.FindFirstValue(ClaimTypes.NameIdentifier) is null)
            {
                return ErrorResponse.Build(
                    message: StaticOperationStatus.User.UserNotFound,
                    statusCode: StaticOperationStatus.StatusCode.NotFound);
            }

            var folderPath = $"{StaticCloudinaryFolders.UserAvatars}/{user.FindFirstValue("FullName")}";

            _cloudinaryServiceControl.SetCommand(
                new UploadAvatarImageCommand(_cloudinaryService, uploadFileDto.File, folderPath));

            await _cloudinaryServiceControl.RunAsync();

            return SuccessResponse.Build(
                message: StaticOperationStatus.File.FileUploaded,
                statusCode: StaticOperationStatus.StatusCode.Ok,
                result: $"Path: {folderPath}");
        }
    }
}
