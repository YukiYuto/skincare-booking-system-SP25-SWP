using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SkincareBookingSystem.Models.Dto.Blog;
using SkincareBookingSystem.Models.Dto.Response;
using SkincareBookingSystem.Services.IServices;
using SkincareBookingSystem.Utilities.Constants;
using Swashbuckle.AspNetCore.Annotations;
using System.Security.Claims;

namespace SkincareBookingSystem.API.Controllers
{
    [ApiController]
    [Route("api/blog")]
    public class BlogController : ControllerBase
    {
        private readonly IBlogService _blogService;

        public BlogController(IBlogService blogService)
        {
            _blogService = blogService;
        }

        [HttpPost("create")]
        [Authorize(Roles = StaticUserRoles.ManagerStaff)]
        [SwaggerOperation(Summary = "API creates a new Blog", Description = "Requires user role")]
        public async Task<ActionResult<ResponseDto>> CreateBlog([FromBody] CreateBlogDto createBlogDto)
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

            var result = await _blogService.CreateBlog(User, createBlogDto);
            return StatusCode(result.StatusCode, result);
        }

        [HttpGet("all")]
        [SwaggerOperation(Summary = "API gets all available Blog(s)", Description = "Requires user role")]
        public async Task<ActionResult<ResponseDto>> GetAllBlog()
        {
            var result = await _blogService.GetAllBlog();
            return StatusCode(result.StatusCode, result);
        }

        [HttpGet("get/{blogId}")]
        [SwaggerOperation(Summary = "API gets a Blog by its Id", Description = "Requires user role")]
        public async Task<ActionResult<ResponseDto>> GetBlogById(Guid blogId)
        {
            var result = await _blogService.GetBlogById(User, blogId);
            return StatusCode(result.StatusCode, result);
        }

        [HttpPut("update")]
        [Authorize(Roles = StaticUserRoles.ManagerStaff)]
        [SwaggerOperation(Summary = "API updates a Blog by its id", Description = "Requires user role")]
        public async Task<ActionResult<ResponseDto>> UpdateBlog([FromBody] UpdateBlogDto updateBlogDto)
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

            var result = await _blogService.UpdateBlog(User, updateBlogDto);
            return StatusCode(result.StatusCode, result);
        }

        [HttpDelete("delete/{blogId}")]
        [Authorize(Roles = StaticUserRoles.ManagerStaff)]
        [SwaggerOperation(Summary = "API soft deletes an exist Blog", Description = "Requires staff, manager role")]
        public async Task<ActionResult<ResponseDto>> DeleteBlog(Guid blogId)
        {
            var result = await _blogService.DeleteBlog(User, blogId);
            return StatusCode(result.StatusCode, result);
        }
    }
}
