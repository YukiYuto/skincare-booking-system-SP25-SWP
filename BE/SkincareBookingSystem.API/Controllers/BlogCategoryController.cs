using Microsoft.AspNetCore.Mvc;
using SkincareBookingSystem.Models.Dto.BlogCategory;
using SkincareBookingSystem.Services.IServices;
using System.Security.Claims;

namespace SkincareBookingSystem.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BlogCategoryController : ControllerBase
    {
        private readonly IBlogCategoryService _blogCategoryService;

        public BlogCategoryController(IBlogCategoryService blogCategoryService)
        {
            _blogCategoryService = blogCategoryService;
        }

        [HttpPost]
        [Route("create")]
        public async Task<ActionResult> CreateBlogCategory(CreateBlogCategoryDto blogCategoryDto)
        {
            var responseDto = await _blogCategoryService.CreateBlogCategory(User, blogCategoryDto);
            return StatusCode(responseDto.StatusCode, responseDto);
        }

        [HttpGet]
        [Route("list")]
        public async Task<ActionResult> GetAllBlogCategories()
        {
            var responseDto = await _blogCategoryService.GetAllBlogCategories();
            return StatusCode(responseDto.StatusCode, responseDto);
        }

        [HttpGet]
        [Route("{blogCategoryId}")]
        public async Task<ActionResult> GetBlogCategory(Guid blogCategoryId)
        {
            var responseDto = await _blogCategoryService.GetBlogCategory(blogCategoryId);
            return StatusCode(responseDto.StatusCode, responseDto);
        }

        [HttpPut]
        [Route("update")]
        public async Task<ActionResult> UpdateBlogCategory(UpdateBlogCategoryDto blogCategoryDto)
        {
            var responseDto = await _blogCategoryService.UpdateBlogCategory(User, blogCategoryDto);
            return StatusCode(responseDto.StatusCode, responseDto);
        }
    }
}
