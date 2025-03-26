using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SkincareBookingSystem.Models.Dto.FileStorage;
using SkincareBookingSystem.Services.IServices;
using System.Security.Claims;
using Swashbuckle.AspNetCore.Annotations;

namespace SkincareBookingSystem.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserManagementController : ControllerBase
    {
        private readonly IFileStorageService _fileStorageService;

        public UserManagementController(IFileStorageService fileStorageService)
        {
            _fileStorageService = fileStorageService;
        }

        [HttpPost("avatar")]
        [SwaggerOperation(Summary = "API uploads an avatar image", Description = "Requires user role")]
        public async Task<ActionResult> UploadAvatarImage(UploadFileDto uploadFileDto)
        {
            var responseDto = await _fileStorageService.UploadAvatarImage(uploadFileDto, User);
            return StatusCode(responseDto.StatusCode, responseDto);
        }
    }
}
