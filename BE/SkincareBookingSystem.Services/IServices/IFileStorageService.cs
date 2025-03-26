using Microsoft.AspNetCore.Http;
using SkincareBookingSystem.Models.Dto.FileStorage;
using SkincareBookingSystem.Models.Dto.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace SkincareBookingSystem.Services.IServices
{
    public interface IFileStorageService
    {
        Task<ResponseDto> UploadAvatarImage(UploadFileDto uploadFileDto, ClaimsPrincipal user);
        Task<ResponseDto> UploadServiceImage(ClaimsPrincipal user, UploadFileDto uploadFileDto);
        Task<ResponseDto> UploadServiceComboImage(ClaimsPrincipal user, UploadFileDto uploadFileDto);
    }
}
