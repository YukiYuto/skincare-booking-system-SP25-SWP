﻿using CloudinaryDotNet;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using SkincareBookingSystem.DataAccess.IRepositories;
using SkincareBookingSystem.Models.Domain;
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
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ITokenService _tokenService;

        public FileStorageService(
            CloudinaryServiceControl cloudinaryServiceControl,
            ICloudinaryService cloudinaryService,
            UserManager<ApplicationUser> userManager,
            IUnitOfWork unitOfWork,
            ITokenService tokenService)
        {
            _cloudinaryServiceControl = cloudinaryServiceControl;
            _cloudinaryService = cloudinaryService;
            _userManager = userManager;
            _unitOfWork = unitOfWork;
            _tokenService = tokenService;
        }

        public async Task<ResponseDto> GetAvatarImageUrl(ClaimsPrincipal user)
        {
            throw new NotImplementedException();
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
            try  // Upload image to Cloudinary
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
    }
}
