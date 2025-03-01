using CloudinaryDotNet;
using SkincareBookingSystem.Models.Dto.FileStorage;
using SkincareBookingSystem.Models.Dto.Response;
using SkincareBookingSystem.Services.Helpers.Responses;
using SkincareBookingSystem.Services.IServices;
using SkincareBookingSystem.Services.Services.CloudinaryModule.Invoker;
using SkincareBookingSystem.Utilities.Constants;
using System.Security.Claims;


namespace SkincareBookingSystem.Services.Services
{
    public class FileStorageService : IFileStorageService
    {
        private readonly CloudinaryServiceControl _cloudinaryServiceControl;
        private readonly ICloudinaryService _cloudinaryService;

        public FileStorageService(
            CloudinaryServiceControl cloudinaryServiceControl,
            ICloudinaryService cloudinaryService)
        {
            _cloudinaryServiceControl = cloudinaryServiceControl;
            _cloudinaryService = cloudinaryService;
        }

        public async Task<ResponseDto> UploadAvatarImage(UploadFileDto uploadFileDto, ClaimsPrincipal user)
        {
            if (user.FindFirstValue(ClaimTypes.NameIdentifier) is null)
            {
                return ErrorResponse.Build(
                    message: StaticOperationStatus.User.UserNotFound,
                    statusCode: StaticOperationStatus.StatusCode.NotFound);
            }

            if (uploadFileDto.File?.Length is 0)
            {
                return ErrorResponse.Build(
                    message: StaticOperationStatus.File.FileEmpty,
                    statusCode: StaticOperationStatus.StatusCode.BadRequest);
            }

            var folderPath = $"{StaticCloudinaryFolders.UserAvatars}/{user.FindFirstValue("FullName")}";
            try // Upload image to Cloudinary
            {
                var uploadedImageUrl = await _cloudinaryService.UploadImageAsync(
                    uploadFileDto.File,
                    folderPath,
                    new Transformation().Named(StaticCloudinarySettings.AvatarTransformation));

                return SuccessResponse.Build(
                    message: StaticOperationStatus.File.FileUploaded,
                    statusCode: StaticOperationStatus.StatusCode.Ok,
                    result: uploadedImageUrl);
            }
            catch (Exception e)
            {
                return ErrorResponse.Build(
                    message: e.Message,
                    statusCode: StaticOperationStatus.StatusCode.InternalServerError);
            }
        }

        public async Task<ResponseDto> UploadServiceImage(ClaimsPrincipal user, UploadFileDto uploadFileDto)
        {
            if (user.FindFirstValue(ClaimTypes.NameIdentifier) is null)
            {
                return ErrorResponse.Build(
                    message: StaticOperationStatus.User.UserNotFound,
                    statusCode: StaticOperationStatus.StatusCode.NotFound);
            }

            if (uploadFileDto.File?.Length is 0)
            {
                return ErrorResponse.Build(
                    message: StaticOperationStatus.File.FileEmpty,
                    statusCode: StaticOperationStatus.StatusCode.BadRequest);
            }

            var folderPath = $"{StaticCloudinaryFolders.ServiceImages}";
            try // Upload image to Cloudinary
            {
                var uploadedImageUrl = await _cloudinaryService.UploadImageAsync(
                    uploadFileDto.File!,
                    folderPath,
                    new Transformation().Named(StaticCloudinarySettings.ServiceTransformation));

                return SuccessResponse.Build(
                    message: StaticOperationStatus.File.FileUploaded,
                    statusCode: StaticOperationStatus.StatusCode.Ok,
                    result: uploadedImageUrl);
            }
            catch (Exception e)
            {
                return ErrorResponse.Build(
                    message: e.Message,
                    statusCode: StaticOperationStatus.StatusCode.InternalServerError);
            }
        }

        public async Task<ResponseDto> UploadServiceComboImage(ClaimsPrincipal user, UploadFileDto uploadFileDto)
        {
            if (user.FindFirstValue(ClaimTypes.NameIdentifier) is null)
            {
                return ErrorResponse.Build(
                    message: StaticOperationStatus.User.UserNotFound,
                    statusCode: StaticOperationStatus.StatusCode.NotFound);
            }

            if (uploadFileDto.File?.Length is 0)
            {
                return ErrorResponse.Build(
                    message: StaticOperationStatus.File.FileEmpty,
                    statusCode: StaticOperationStatus.StatusCode.BadRequest);
            }

            var folderPath = $"{StaticCloudinaryFolders.ServiceComboImages}";
            try // Upload image to Cloudinary
            {
                var uploadedImageUrl = await _cloudinaryService.UploadImageAsync(
                    uploadFileDto.File!,
                    folderPath,
                    new Transformation().Named(StaticCloudinarySettings.ServiceComboTransformation));

                return SuccessResponse.Build(
                    message: StaticOperationStatus.File.FileUploaded,
                    statusCode: StaticOperationStatus.StatusCode.Ok,
                    result: uploadedImageUrl);
            }
            catch (Exception e)
            {
                return ErrorResponse.Build(
                    message: e.Message,
                    statusCode: StaticOperationStatus.StatusCode.InternalServerError);
            }
        }
    }
}