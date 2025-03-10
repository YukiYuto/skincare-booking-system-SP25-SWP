using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SkincareBookingSystem.Models.Dto.FileStorage;
using SkincareBookingSystem.Services.IServices;
using System.Security.Claims;
using Swashbuckle.AspNetCore.Annotations;

namespace SkincareBookingSystem.API.Controllers
{
    // Cần kiểm tra lại tại chatGPT kêu là khi làm file thì không nên chi ra nhiều như vậy mà nên dùng switch case
    [Route("api/files")]
    [ApiController]
    public class FileStorageController : ControllerBase
    {
        private readonly IFileStorageService _fileStorageService;

        public FileStorageController(IFileStorageService fileStorageService)
        {
            _fileStorageService = fileStorageService;
        }

        [HttpPost("service")]
        [SwaggerOperation(Summary = "API uploads a service image", Description = "Requires user role")]
        public async Task<ActionResult> UploadServiceImage(UploadFileDto uploadFileDto)
        {
            var responseDto = await _fileStorageService.UploadServiceImage(User, uploadFileDto);
            return StatusCode(responseDto.StatusCode, responseDto);
        }

        [HttpPost("service-combo")]
        [SwaggerOperation(Summary = "API uploads a service combo image", Description = "Requires user role")]
        public async Task<ActionResult> UploadServiceComboImage(UploadFileDto uploadFileDto)
        {
            var responseDto = await _fileStorageService.UploadServiceComboImage(User, uploadFileDto);
            return StatusCode(responseDto.StatusCode, responseDto);
        }
    }
}