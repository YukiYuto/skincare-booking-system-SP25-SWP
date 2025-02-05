using SkincareBookingSystem.DataAccess.IRepositories;
using SkincareBookingSystem.Models.Domain;
using SkincareBookingSystem.Models.Dto.BlogCategory;
using SkincareBookingSystem.Models.Dto.Response;
using SkincareBookingSystem.Services.IServices;

namespace SkincareBookingSystem.Services.Services
{
    public class BlogCategoryService : IBlogCategoryService
    {
        private readonly IUnitOfWork _unitOfWork;
        public BlogCategoryService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<ResponseDto> CreateBlogCategory(BlogCategoryDto blogCategoryDto)
        {
            BlogCategory blogCategory = new BlogCategory()
            {
                Name = blogCategoryDto.Name,
                Description = blogCategoryDto.Description,
                CreatedBy = "Admin",
                CreatedTime = DateTime.UtcNow,
                Status = "Created"
            };

            await _unitOfWork.BlogCategory.AddAsync(blogCategory);
            await _unitOfWork.SaveAsync();

            return new ResponseDto()
            {
                Result = blogCategory,
                IsSuccess = true,
                Message = "Blog Category created successfully",
                StatusCode = 201
            };
        }
    }
}
