using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SkincareBookingSystem.Models.Dto.FileStorage;
using SkincareBookingSystem.Services.IServices;
using System.Security.Claims;

namespace SkincareBookingSystem.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FileStorageController : ControllerBase
    {
        private readonly IFileStorageService _fileStorageService;

        public FileStorageController(IFileStorageService fileStorageService)
        {
            _fileStorageService = fileStorageService;
        }

        [HttpPost("service")]
        public async Task<ActionResult> UploadServiceImage(UploadFileDto uploadFileDto)
        {
            var responseDto = await _fileStorageService.UploadServiceImage(User, uploadFileDto);
            return StatusCode(responseDto.StatusCode, responseDto);
        }

        [HttpPost("service-combo")]
        public async Task<ActionResult> UploadServiceComboImage(UploadFileDto uploadFileDto)
        {
            var responseDto = await _fileStorageService.UploadServiceComboImage(User, uploadFileDto);
            return StatusCode(responseDto.StatusCode, responseDto);
        }
    }
}