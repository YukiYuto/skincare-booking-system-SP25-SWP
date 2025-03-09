using Microsoft.AspNetCore.Mvc;
using SkincareBookingSystem.Models.Dto.Response;
using SkincareBookingSystem.Models.Dto.TestQuestion;
using SkincareBookingSystem.Services.IServices;

namespace SkincareBookingSystem.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TestQuestionController : ControllerBase
    {
        private readonly ITestQuestionService _testQuestionService;

        public TestQuestionController(ITestQuestionService testQuestionService)
        {
            _testQuestionService = testQuestionService;
        }

        [HttpPost("create")]
        public async Task<ActionResult<ResponseDto>> CreateTestQuestion([FromBody] CreateTestQuestionDto testQuestionDto)
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
            var result = await _testQuestionService.CreateTestQuestion(User, testQuestionDto);
            return StatusCode(result.StatusCode, result);
        }

        [HttpGet]
        public async Task<ActionResult<ResponseDto>> GetAllTestQuestions()
        {
            var result = await _testQuestionService.GetAllTestQuestions();
            return StatusCode(result.StatusCode, result);
        }

        [HttpGet("{testQuestionId}")]
        public async Task<ActionResult<ResponseDto>> GetTestQuestionById(Guid testQuestionId)
        {
            var result = await _testQuestionService.GetTestQuestionById(User, testQuestionId);
            return StatusCode(result.StatusCode, result);
        }

        [HttpPut]
        public async Task<ActionResult<ResponseDto>> UpdateTestQuestion([FromBody] UpdateTestQuestionDto testQuestionDto)
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
            var result = await _testQuestionService.UpdateTestQuestion(User, testQuestionDto);
            return StatusCode(result.StatusCode, result);
        }

        [HttpDelete("delete")]
        public async Task<ActionResult<ResponseDto>> DeleteTestQuestion(Guid testQuestionId)
        {
            var result = await _testQuestionService.DeleteTestQuestion(User, testQuestionId);
            return StatusCode(result.StatusCode, result);
        }
    }
}