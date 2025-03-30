using Microsoft.AspNetCore.Mvc;
using SkincareBookingSystem.Models.Dto.Response;
using SkincareBookingSystem.Models.Dto.SkinTest;
using SkincareBookingSystem.Services.IServices;
using Swashbuckle.AspNetCore.Annotations;

namespace SkincareBookingSystem.API.Controllers
{
    [ApiController]
    [Route("api/skintest")]
    public class SkinTestController : ControllerBase
    {
        private readonly ISkinTestService _skinTestService;

        public SkinTestController(ISkinTestService skinTestService)
        {
            _skinTestService = skinTestService;
        }

        [HttpPost("create")]
        [SwaggerOperation(Summary = "API creates a new SkinTest", Description = "Requires staff role")]
        public async Task<ActionResult<ResponseDto>> CreateSkinTest([FromBody] CreateSkinTestDto createSkinTestDto)
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
            var result = await _skinTestService.CreateSkinTest(User, createSkinTestDto);
            return StatusCode(result.StatusCode, result);
        }

        [HttpGet("all")]
        [SwaggerOperation(Summary = "API gets all available SkinTest(s)", Description = "Requires staff role")]
        public async Task<ActionResult<ResponseDto>> GetAllSkinTests()
        {
            var result = await _skinTestService.GetAllSkinTests();
            return StatusCode(result.StatusCode, result);
        }

        [HttpGet("get/{skinTestId}")]
        [SwaggerOperation(Summary = "API gets a SkinTest by its Id", Description = "Requires staff role")]
        public async Task<ActionResult<ResponseDto>> GetSkinTestById(Guid skinTestId)
        {
            var result = await _skinTestService.GetSkinTestById(User, skinTestId);
            return StatusCode(result.StatusCode, result);
        }

        [HttpPut("update")]
        [SwaggerOperation(Summary = "API updates an existing SkinTest", Description = "Requires staff role")]
        public async Task<ActionResult<ResponseDto>> UpdateSkinTest([FromBody] UpdateSkinTestDto updateSkinTestDto)
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
            var result = await _skinTestService.UpdateSkinTest(User, updateSkinTestDto);
            return StatusCode(result.StatusCode, result);
        }

        [HttpDelete("delete/{skinTestId}")]
        [SwaggerOperation(Summary = "API deletes a SkinTest by its Id", Description = "Requires staff role")]
        public async Task<ActionResult<ResponseDto>> DeleteSkinTest(Guid skinTestId)
        {
            var result = await _skinTestService.DeleteSkinTest(User, skinTestId);
            return StatusCode(result.StatusCode, result);
        }

    }
}
