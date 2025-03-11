using Microsoft.AspNetCore.Mvc;
using SkincareBookingSystem.DataAccess.IRepositories;
using SkincareBookingSystem.Models.Dto.BlogCategories;
using SkincareBookingSystem.Models.Dto.Response;
using SkincareBookingSystem.Services.IServices;
using Swashbuckle.AspNetCore.Annotations;
using System.Security.Claims;

namespace SkincareBookingSystem.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BlogCategoryController : ControllerBase
    {
        private readonly IBlogCategoryService _blogCategoryService;
        public BlogCategoryController(IBlogCategoryService blogCategoryService)
        {
            _blogCategoryService = blogCategoryService;
        }

        [HttpPost("create")]
        [SwaggerOperation(Summary = "API creates a new Blog Category", Description = "Requires admin role")]
        public async Task<ActionResult<ResponseDto>> CreateBlogCategory([FromBody] CreateBlogCategoryDto blogCategoryDto)
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
            var result = await _blogCategoryService.CreateBlogCategory(User, blogCategoryDto);
            return StatusCode(result.StatusCode, result);
        }

        [HttpGet("all")]
        [SwaggerOperation(Summary = "API gets all available Blog Categories", Description = "Requires admin role")]
        public async Task<ActionResult<ResponseDto>> GetAllBlogCategories()
        {
            var result = await _blogCategoryService.GetAllBlogCategories();
            return StatusCode(result.StatusCode, result);
        }

        [HttpGet("get/{blogCategoryId}")]
        [SwaggerOperation(Summary = "API gets a Blog Category by id", Description = "Requires admin role")]
        public async Task<ActionResult<ResponseDto>> GetBlogCategory(Guid blogCategoryId)
        {
            var result = await _blogCategoryService.GetBlogCategory(User, blogCategoryId);
            return StatusCode(result.StatusCode, result);
        }

        [HttpPut("update")]
        [SwaggerOperation(Summary = "API updates a Blog Category", Description = "Requires admin role")]
        public async Task<ActionResult<ResponseDto>> UpdateBlogCategory([FromBody] UpdateBlogCategoryDto blogCategoryDto)
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
            var result = await _blogCategoryService.UpdateBlogCategory(User, blogCategoryDto);
            return StatusCode(result.StatusCode, result);
        }
    }
}
