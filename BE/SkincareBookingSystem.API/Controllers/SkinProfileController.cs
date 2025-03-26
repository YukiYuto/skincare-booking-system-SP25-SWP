using Microsoft.AspNetCore.Mvc;
using SkincareBookingSystem.DataAccess.IRepositories;
using SkincareBookingSystem.Models.Dto.Response;
using SkincareBookingSystem.Models.Dto.SkinProfile;
using SkincareBookingSystem.Services.IServices;
using Swashbuckle.AspNetCore.Annotations;   

namespace SkincareBookingSystem.API.Controllers
{
    [ApiController]
    [Route("api/skinprofile")]
    public class SkinProfileController : ControllerBase
    {
        private readonly ISkinProfileService _skinProfileService;

        public SkinProfileController(ISkinProfileService skinProfileService)
        {
            _skinProfileService = skinProfileService;
        }

        [HttpPost("create")]
        [SwaggerOperation(Summary = "API creates a new SkinProfile", Description = "Requires staff role")]
        public async Task<ActionResult<ResponseDto>> CreateSkinProfile([FromBody] CreateSkinProfileDto createSkinProfileDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new ResponseDto
                {
                    IsSuccess = false,
                    Message = "Invalid input data",
                    Result = ModelState.Values.SelectMany(v => v.Errors.Select(e => e.ErrorMessage))
                });
            }
            var result = await _skinProfileService.CreateSkinProfile(User, createSkinProfileDto);
            return StatusCode(result.StatusCode, result);
        }

        [HttpGet("all")]
        [SwaggerOperation(Summary = "API gets all available SkinProfile(s)", Description = "Requires staff role")]
        public async Task<ActionResult<ResponseDto>> GetAllSkinProfiles()
        {
            var result = await _skinProfileService.GetAllSkinProfiles();
            return StatusCode(result.StatusCode, result);
        }

        [HttpGet("get/{skinProfileId}")]
        [SwaggerOperation(Summary = "API gets a SkinProfile by its Id", Description = "Requires staff role")]
        public async Task<ActionResult<ResponseDto>> GetSkinProfileById(Guid skinProfileId)
        {
            var result = await _skinProfileService.GetSkinProfileById(User, skinProfileId);
            return StatusCode(result.StatusCode, result);
        }

        [HttpPut("update")]
        [SwaggerOperation(Summary = "API updates an existing SkinProfile", Description = "Requires staff role")]
        public async Task<ActionResult<ResponseDto>> UpdateSkinProfile([FromBody] UpdateSkinProfileDto updateSkinProfileDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new ResponseDto
                {
                    IsSuccess = false,
                    Message = "Invalid input data",
                    Result = ModelState.Values.SelectMany(v => v.Errors.Select(e => e.ErrorMessage))
                });
            }
            var result = await _skinProfileService.UpdateSkinProfile(User, updateSkinProfileDto);
            return StatusCode(result.StatusCode, result);
        }

        [HttpDelete("delete/{skinProfileId}")]
        [SwaggerOperation(Summary = "API deletes an existing SkinProfile", Description = "Requires staff role")]
        public async Task<ActionResult<ResponseDto>> DeleteSkinProfile(Guid skinProfileId)
        {
            var result = await _skinProfileService.DeleteSkinProfile(User, skinProfileId);
            return StatusCode(result.StatusCode, result);
        }

    }
}
