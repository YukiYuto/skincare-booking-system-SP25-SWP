using Microsoft.AspNetCore.Mvc;
using SkincareBookingSystem.Models.Dto.Response;
using SkincareBookingSystem.Models.Dto.TestAnswer;
using SkincareBookingSystem.Services.IServices;
using Swashbuckle.AspNetCore.Annotations;   

namespace SkincareBookingSystem.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TestAnswerController : ControllerBase
    {
        private readonly ITestAnswerService _testAnswerService;

        public TestAnswerController(ITestAnswerService testAnswerService)
        {
            _testAnswerService = testAnswerService;
        }

        [HttpPost("create")]
        [SwaggerOperation(Summary = "Create a test answer", Description = "Requires staff, customer role")]
        public async Task<ActionResult<ResponseDto>> CreateTestAnswer([FromBody] CreateTestAnswerDto createTestAnswerDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new ResponseDto
                {
                    IsSuccess = false,
                    Message = "Invalid input data.",
                    Result = ModelState.Values.SelectMany(v => v.Errors.Select(e => e.ErrorMessage))
                });
            }
            var result = await _testAnswerService.CreateTestAnswer(User, createTestAnswerDto);
            return StatusCode(result.StatusCode, result);
        }

        [HttpPut("update")]
        [SwaggerOperation(Summary = "Update a test answer", Description = "Requires staff, customer role")]
        public async Task<ActionResult<ResponseDto>> UpdateTestAnswer([FromBody] UpdateTestAnswerDto updateTestAnswerDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new ResponseDto
                {
                    IsSuccess = false,
                    Message = "Invalid input data.",
                    Result = ModelState.Values.SelectMany(v => v.Errors.Select(e => e.ErrorMessage))
                });
            }
            var result = await _testAnswerService.UpdateTestAnswer(User, updateTestAnswerDto);
            return StatusCode(result.StatusCode, result);
        }

        [HttpGet("all")]
        [SwaggerOperation(Summary = "Get all test answers", Description = "Requires staff, customer role")]
        public async Task<ActionResult<ResponseDto>> GetAllTestAnswers()
        {
            var result = await _testAnswerService.GetAllTestAnswers();
            return StatusCode(result.StatusCode, result);
        }

        [HttpGet("get/{testAnswerId}")]
        [SwaggerOperation(Summary = "Get a test answer by id", Description = "Requires staff, customer role")]
        public async Task<ActionResult<ResponseDto>> GetTestAnswerById(Guid testAnswerId)
        {
            var result = await _testAnswerService.GetTestAnswerById(User, testAnswerId);
            return StatusCode(result.StatusCode, result);
        }

        [HttpGet("TestQuestion/{testQuestionId}")]
        [SwaggerOperation(Summary = "Get a test answer by test question id", Description = "Requires staff, customer role")]
        public async Task<ActionResult<ResponseDto>> GetTestAnswerByTestQuestionId(Guid testQuestionId)
        {
            var result = await _testAnswerService.GetTestAnswerByTestQuestionId(testQuestionId);
            return StatusCode(result.StatusCode, result);
        }

        [HttpDelete("delete/{testAnswerId}")]
        [SwaggerOperation(Summary = "Delete a test answer", Description = "Requires staff, customer role")]
        public async Task<ActionResult<ResponseDto>> DeleteTestAnswer(Guid testAnswerId)
        {
            var result = await _testAnswerService.DeleteTestAnswer(User, testAnswerId);
            return StatusCode(result.StatusCode, result);
        }
    }
}
